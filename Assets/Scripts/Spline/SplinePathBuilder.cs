using UnityEngine;
using DG.Tweening;

[ExecuteAlways]
public class SplinePathBuilder : MonoBehaviour
{
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField, HideInInspector] private Transform[] _checkpoints;

    [SerializeField] private float _duration = 5f;

    private Vector3[] _lastCheckpointPositions;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            bool positionsChanged = false;

            if (_pointA != null && _pointA.hasChanged)
                positionsChanged = true;

            if (_pointB != null && _pointB.hasChanged)
                positionsChanged = true;

            if (_checkpoints != null)
            {
                for (int i = 0; i < _checkpoints.Length; i++)
                {
                    if (_checkpoints[i] != null && _checkpoints[i].hasChanged)
                    {
                        positionsChanged = true;
                        break;
                    }
                }
            }

            if (positionsChanged)
            {
                CreateSplinePath();
                UpdateLastCheckpointPositions();
            }
        }
    }

    private void UpdateLastCheckpointPositions()
    {
        _lastCheckpointPositions = new Vector3[_checkpoints.Length];

        for (int i = 0; i < _checkpoints.Length; i++)
            _lastCheckpointPositions[i] = _checkpoints[i] != null ? _checkpoints[i].position : Vector3.zero;
    }
#endif

    private void CreateSplinePath()
    {
        Vector3[] pathPoints = new Vector3[_checkpoints.Length + 2];
        pathPoints[0] = _pointA.position;

        for (int i = 0; i < _checkpoints.Length; i++)
        {
            pathPoints[i + 1] = _checkpoints[i] != null ? _checkpoints[i].position : Vector3.zero;
        }

        pathPoints[pathPoints.Length - 1] = _pointB.position;

        transform.DOPath(pathPoints, _duration, PathType.CatmullRom, PathMode.Full3D)
            .SetOptions(false)
            .SetEase(Ease.Linear);

        if (gameObject.GetComponent<LineRenderer>() != null)
        {
            DestroyImmediate(gameObject.GetComponent<LineRenderer>());
        }
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = pathPoints.Length;
        lineRenderer.SetPositions(pathPoints);
    }
}
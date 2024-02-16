using SplineMesh;
using UnityEngine;

[RequireComponent(typeof(Spline))]
[RequireComponent(typeof(SplineMeshTiling))]
[RequireComponent(typeof(SplineSmoother))]
public class RandomSplineGenerator : MonoBehaviour
{
    [SerializeField] private int _numberOfNodes = 10;
    [SerializeField] private Vector3 _startPosition = Vector3.zero;
    [SerializeField] private Vector3 _endPosition = Vector3.right * 100f;
    [SerializeField] private float _yOffset = 5f;
    [SerializeField] private float _zOffset = 10f;

    private void Start()
    {
        GenerateSpline();
    }

    [ContextMenu("Generate Spline")]
    private void GenerateSpline()
    {
        Spline spline = gameObject.GetComponent<Spline>() ?? gameObject.AddComponent<Spline>();

        spline.nodes.Clear();
        spline.curves.Clear();

        for (int i = 0; i <= _numberOfNodes; i++)
        {
            float t = i / (float)_numberOfNodes;
            Vector3 position = Vector3.Lerp(_startPosition, _endPosition, t);

            position.y += Random.Range(-_yOffset, _yOffset);
            position.z += Random.Range(-_zOffset, _zOffset);

            SplineNode splineNode = new SplineNode(position, Vector3.zero);
            spline.AddNode(splineNode);

            if (i <= _numberOfNodes)
                splineNode.Direction = Vector3.Lerp(_startPosition, _endPosition, t + 1 / (float)_numberOfNodes) - position;
        }

        spline.RefreshCurves();
    }
}

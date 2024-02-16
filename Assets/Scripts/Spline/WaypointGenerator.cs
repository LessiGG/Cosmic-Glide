using UnityEngine;
using SplineMesh;
using System.Collections.Generic;

public class WaypointGenerator : MonoBehaviour
{
    [SerializeField] private Spline _spline;
    [SerializeField] private int _numberOfWaypoints;
    [SerializeField] private GameObject _waypointPrefab;
    [SerializeField] private float _maxWaypointRotation = 90f;
    [SerializeField] private float _startGapPercentage = 0.1f;
    [SerializeField] private float _endGapPercentage = 0.1f;

    private Transform _container;
    private List<GameObject> _waypoints = new List<GameObject>();
    public List<GameObject> Waypoints => _waypoints;

    private void Start()
    {
        CheckAndCreateContainer();
        GenerateWaypoints();
    }

    private void CheckAndCreateContainer()
    {
        _container = transform.Find("WaypointContainer");

        if (_container == null)
        {
            _container = new GameObject("WaypointContainer").transform;
            _container.parent = transform;
        }
    }

    private void GenerateWaypoints()
    {
        if (_spline == null || _waypointPrefab == null || _numberOfWaypoints <= 0) return;

        float totalSplineLength = _spline.Length;
        float startGapDistance = _startGapPercentage * totalSplineLength;
        float endGapDistance = _endGapPercentage * totalSplineLength;

        for (int i = 0; i < _numberOfWaypoints; i++)
        {
            float splineRateForWaypoint = i / (float)(_numberOfWaypoints - 1);
            float splineDistance = Mathf.Lerp(startGapDistance, totalSplineLength - endGapDistance, splineRateForWaypoint);

            CurveSample waypointSample = _spline.GetSampleAtDistance(splineDistance);

            Vector3 waypointPosition = waypointSample.location;
            Quaternion waypointRotation = waypointSample.Rotation;

            GameObject newWaypoint = Instantiate(_waypointPrefab, waypointPosition, waypointRotation, _container);
            _waypoints.Add(newWaypoint);
        }

        SetRandomZRotation();
    }

    private void SetRandomZRotation()
    {
        foreach (var waypoint in _waypoints)
        {
            float randomZRotation = Random.Range(-_maxWaypointRotation, _maxWaypointRotation);
            waypoint.transform.rotation = Quaternion.Euler(
                waypoint.transform.rotation.eulerAngles.x,
                waypoint.transform.rotation.eulerAngles.y,
                waypoint.transform.rotation.eulerAngles.z + randomZRotation
            );
        }
    }
}

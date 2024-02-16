using SplineMesh;
using UnityEngine;

public class GliderMover : MonoBehaviour
{
    [SerializeField] private Spline _spline;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    private float _splineRate = 0f;
    private float _zRotation = 0f;

    private void Update()
    {
        UpdateSplineRate();

        if (CanMoveOnSpline())
            PlaceOnSpline();
    }

    private void UpdateSplineRate()
    {
        _splineRate += _speed * Time.deltaTime;
    }

    private bool CanMoveOnSpline()
    {
        return _splineRate < _spline.nodes.Count - 1;
    }

    private void PlaceOnSpline()
    {
        CurveSample sample = _spline.GetSample(_splineRate);
        UpdateRotation();
        SetTransform(sample);
    }

    private void UpdateRotation()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        _zRotation += -horizontalInput * _rotationSpeed * Time.deltaTime;
    }

    private void SetTransform(CurveSample sample)
    {
        transform.localPosition = sample.location;
        Vector3 originalEulerAngles = sample.Rotation.eulerAngles;
        transform.localRotation = Quaternion.Euler(originalEulerAngles.x, originalEulerAngles.y, _zRotation);
    }
}
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private int _maxScore = 100;
    [SerializeField] private int _maxRotationDifference = 90;

    private ScorePopUp _popUp;

    private void Awake()
    {
        _popUp = FindObjectOfType<ScorePopUp>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Glider glider))
        {
            Quaternion waypointRotation = transform.rotation;
            Quaternion playerRotation = other.transform.rotation;

            float rotationDifference = Mathf.Abs(Mathf.DeltaAngle(waypointRotation.eulerAngles.z, playerRotation.eulerAngles.z));
            float normalizedDifference = Mathf.Clamp01(rotationDifference / _maxRotationDifference);

            int score = CalculateScore(normalizedDifference);
            glider.IncreaseScore(score);
            _popUp.Show(score);
        }
    }

    private int CalculateScore(float normalizedError)
    {
        int score = Mathf.RoundToInt((1f - normalizedError) * _maxScore / 10f) * 10;
        return Mathf.Clamp(score, 0, _maxScore);
    }
}
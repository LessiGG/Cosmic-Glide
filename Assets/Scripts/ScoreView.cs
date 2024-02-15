using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text _score;
    [SerializeField] private Glider _glider;

    private string _scoreHash = "Score : ";

    private void OnEnable()
    {
        _glider.ScoreChanged += OnScoreChanged;
    }

    private void OnDisable()
    {
        _glider.ScoreChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(int score)
    {
        _score.text = _scoreHash + score;
    }
}
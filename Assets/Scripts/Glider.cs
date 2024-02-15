using UnityEngine;
using UnityEngine.Events;

public class Glider : MonoBehaviour
{
    private int _score;

    public event UnityAction<int> ScoreChanged;

    public void IncreaseScore(int score)
    {
        _score += score;
        ScoreChanged?.Invoke(_score);
    }
}
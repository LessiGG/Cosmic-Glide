using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private List<LevelData> _levels;

    private int _currentLevelIndex = 1;

    public int LevelId => _currentLevelIndex;

    public LevelData GetCurrentLevel()
    {
        if (_currentLevelIndex >= 1 && _currentLevelIndex < _levels.Count)
            return _levels[_currentLevelIndex];

        return null;
    }

    public void LoadNextLevel()
    {
        _currentLevelIndex++;

        if (_currentLevelIndex >= _levels.Count)
            _currentLevelIndex = 1;
    }
}
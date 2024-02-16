using TMPro;
using UnityEngine;

public class LevelView : MonoBehaviour
{
    [SerializeField] private TMP_Text _level;
    [SerializeField] private LevelLoader _loader;

    private string _levelHash = "Level 0";

    private void Awake()
    {
        DisplayLevelId();
    }

    private void DisplayLevelId()
    {
        _level.text = _levelHash + _loader.LevelId;
    }
}
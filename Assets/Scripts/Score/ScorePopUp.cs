using System.Collections;
using TMPro;
using UnityEngine;

public class ScorePopUp : MonoBehaviour
{
    private TMP_Text _text;
    private string _pointsHash = "Points!";
    private float _disappearTimer = 1f;

    private void Awake()
    {
        _text = transform.GetComponent<TMP_Text>();
    }

    public void Show(int score)
    {
        _text.text = $"+{score} {_pointsHash}";
        StartCoroutine(DisplayAndHide());
    }

    private void Display()
    {
        SetAlpha(255);
    }

    private void Hide()
    {
        SetAlpha(0); 
    }
    private void SetAlpha(float newAlpha)
    {
        Color currentColor = _text.color;
        _text.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
    }

    private IEnumerator DisplayAndHide()
    {
        Display();
        yield return new WaitForSeconds(_disappearTimer);
        Hide();
    }
}
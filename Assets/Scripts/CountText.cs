using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CountText : MonoBehaviour
{
    public int count;
    private TextMeshProUGUI text;
    private string initialText;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        initialText = text.text;
    }

    private void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        text.text = count.ToString() + initialText;
        transform.DOShakePosition(0.5f, 10, 10, 90);
    }
}

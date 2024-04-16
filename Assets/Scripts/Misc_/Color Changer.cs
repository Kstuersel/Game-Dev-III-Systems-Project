using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Color DefaultColor { get; private set; }

    [SerializeField] private Color[] _colors;
    [SerializeField] private SpriteRenderer _fillSpriteRenderer;

    public void SetDefaultCOlor(Color color)
    {
        DefaultColor = color;
        SetColor(color);
    }

    public void SetColor(Color color)
    {
        _fillSpriteRenderer.color = color;
    }

    public void SetRandomColor()
    {
        int randomnum = Random.Range(0, _colors.Length);
        DefaultColor = _colors[randomnum];
        _fillSpriteRenderer.color = DefaultColor;
    }
}

﻿using UnityEngine;
using System.Collections;

public class BenColored : MonoBehaviour {
    public ObjectColor objectColor;
    public static Color GetRGB(ObjectColor color)
    {
        switch (color)
        {
            case ObjectColor.Red: return Color.red;
            case ObjectColor.Green: return Color.green;
            case ObjectColor.Blue: return Color.blue;
            case ObjectColor.Yellow: return Color.yellow;
            default: return Color.white;
        }
    }

    public void ChangeColor(ObjectColor color)
    {
        if (objectColor != color)
        {
            objectColor = color;
            gameObject.layer = LayerMask.NameToLayer(color.ToString());
        }
    }
}

public enum ObjectColor { Red, Green, Blue, Yellow, None };

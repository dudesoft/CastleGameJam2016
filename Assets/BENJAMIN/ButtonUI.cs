using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonUI : MonoBehaviour {

    public Image red, redBar;
    public Image green, greenBar;
    public Image blue, blueBar;
    public Image yellow, yellowBar;

    public static ButtonUI instance;

	public void UpdateWeapon()
    {

    }

    public void UpdateAmmo()
    {

    }

    Image GetButton(ObjectColor color)
    {
        switch (color)
        {
            case ObjectColor.Red: return red;
            case ObjectColor.Green: return green;
            case ObjectColor.Blue: return blue;
            case ObjectColor.Yellow: return yellow;
        }
        return red;
    }

    Image GetBar(ObjectColor color)
    {
        switch (color)
        {
            case ObjectColor.Red: return red;
            case ObjectColor.Green: return green;
            case ObjectColor.Blue: return blue;
            case ObjectColor.Yellow: return yellow;
        }
        return red;
    }
}

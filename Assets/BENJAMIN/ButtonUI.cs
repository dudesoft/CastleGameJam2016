using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonUI : MonoBehaviour {

    public Image red, redBar;
    public Image green, greenBar;
    public Image blue, blueBar;
    public Image yellow, yellowBar;

    public static ButtonUI instance;

    public Image currentButton;

    void Awake()
    {
        instance = this;
    }

	public void ChangeWeapon(ObjectColor color)
    {
        UpdateAmmo();

        if (currentButton != null)
            LeanTween.scale(currentButton.gameObject, Vector3.one, 0.4f);
        
        currentButton = GetButton(BenShip.instance.currentGun.objectColor);

        LeanTween.scale(currentButton.gameObject, Vector3.one * 1.5f, 0.2f).setEase(LeanTweenType.easeInBack);
    }

    public void UpdateAmmo()
    {
        GetBar(BenShip.instance.currentGun.objectColor).fillAmount = (BenShip.instance.GetCurrentAmmoCount() / (float)BenShip.instance.GetCurrentMaxAmmo());
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
            case ObjectColor.Red: return redBar;
            case ObjectColor.Green: return greenBar;
            case ObjectColor.Blue: return blueBar;
            case ObjectColor.Yellow: return yellowBar;
        }
        return red;
    }
}

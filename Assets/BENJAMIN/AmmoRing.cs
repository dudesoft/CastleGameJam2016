using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AmmoRing : MonoBehaviour {

    public Image ring;
    public Transform scaleParent;

    [Range(0f, 1f)]
    public float fill = 0.5f;

    public static AmmoRing instance;

	// Use this for initialization
	void Start () 
    {
        instance = this;
        GetComponent<Image>();
        Canvas.ForceUpdateCanvases();
	}
	
	// Update is called once per frame
	void Update () {
        ring.fillAmount = fill;
        transform.localRotation = Quaternion.AngleAxis(fill * 360 / 2, Vector3.forward);

        if (Input.GetKeyDown(KeyCode.U))
        {
            Pulse();
        }
	}

    public void ChangeWeapon(BenProjectileSpawner weapon)
    {
        StartCoroutine(ChangeWeaponRoutine(weapon));
    }

    IEnumerator ChangeWeaponRoutine(BenProjectileSpawner weapon)
    {
        LeanTween.cancel(gameObject, false);

        yield return null;

        scaleParent.localScale = Vector3.zero;
        LeanTween.scale(scaleParent.gameObject, Vector3.one, 0.4f).setEase(LeanTweenType.easeInOutBack).setDelay(0.2f);


        Color c = BenColored.GetRGB(weapon.objectColor);
        c.a = 0.075f;
        ring.color = c;

        lastWeaponChange = Time.time;
    }

    public void UpdateAmmo()
    {
        fill = (BenShip.instance.GetCurrentAmmoCount() / (float)BenShip.instance.GetCurrentMaxAmmo()) / 2;
    }

    public float lastWeaponChange = 0;

    public Color GetRingColor()
    {
        Color c = BenColored.GetRGB(BenShip.instance.currentGun.objectColor);
        c.a = 0.075f;
        return c;
    }

    public void Pulse(float amount = 1.2f, float duration = 0.1f)
    {
        if (Time.time - lastWeaponChange < 0.4f)
            return;

        scaleParent.localScale = Vector3.one * amount;
        LeanTween.scale(scaleParent.gameObject, Vector3.one, duration);
        ring.color = Color.Lerp(Color.white, GetRingColor(), 0.5f);
        LeanTween.value(gameObject, ring.color, GetRingColor(), duration).setOnUpdate(
        (Color val) =>
        {
            ring.color = val;
        });
    }
}

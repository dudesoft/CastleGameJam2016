using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AmmoRing : MonoBehaviour {

    public Image ring;
    public float pulseScale = 1.2f;

    [Range(0f, 1f)]
    public float fill = 1;

	// Use this for initialization
	void Start () {
        GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        ring.fillAmount = fill;
        transform.localRotation = Quaternion.AngleAxis(fill * 360 / 2,Vector3.forward);
        if (Input.GetKeyDown(KeyCode.U))
        {
            Pulse();
        }
	}

    public void Pulse()
    {
        transform.localScale = Vector3.one * pulseScale;
        LeanTween.scale(gameObject, Vector3.one, 0.1f);
    }

    public void ChangeColor(Color c)
    {

    }
}

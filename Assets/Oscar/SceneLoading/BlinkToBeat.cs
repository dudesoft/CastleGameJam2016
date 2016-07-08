using UnityEngine;
using System.Collections;

public class BlinkToBeat : MonoBehaviour {
    private UnityEngine.UI.Image image;
    private bool on = false;

    public Color OnColor;
    public Color OffColor;

    public float LerpSpeed;

    void Start() {
        image = GetComponent<UnityEngine.UI.Image>();
    }
	// Update is called once per frame
	void Update () {
        if (BeatManager.instance.beating) {
            on = !on;            
        }
        if (on) {
            //text.color = Color.Lerp(text.color, OnColor, LerpSpeed * Time.deltaTime);
            image.color = OnColor;
        } else {
            image.color = Color.Lerp(image.color, OffColor, LerpSpeed * Time.deltaTime);
        }
	}
}

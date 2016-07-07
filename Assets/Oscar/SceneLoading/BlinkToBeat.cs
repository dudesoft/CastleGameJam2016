using UnityEngine;
using System.Collections;

public class BlinkToBeat : MonoBehaviour {
    private UnityEngine.UI.Text text;
    private bool on = false;

    public Color OnColor;
    public Color OffColor;

    public float LerpSpeed;

    void Start() {
        text = GetComponent<UnityEngine.UI.Text>();
    }
	// Update is called once per frame
	void Update () {
        if (BeatManager.instance.beating) {
            on = !on;            
        }
        if (on) {
            //text.color = Color.Lerp(text.color, OnColor, LerpSpeed * Time.deltaTime);
            text.color = OnColor;
        } else {
            text.color = Color.Lerp(text.color, OffColor, LerpSpeed * Time.deltaTime);
        }
	}
}

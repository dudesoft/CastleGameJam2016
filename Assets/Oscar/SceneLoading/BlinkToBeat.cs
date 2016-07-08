using UnityEngine;
using System.Collections;

public class BlinkToBeat : MonoBehaviour {
    private SpriteRenderer spriteRend;
    private TextMesh text;
    private bool on = true;

    public Color OnColor;
    public Color OffColor;

    public float LerpSpeed;

    void Start() {
        spriteRend = GetComponent<SpriteRenderer>();
        text = GetComponent<TextMesh>();
    }
	// Update is called once per frame
	void Update () {
        if (BeatManager.instance.beating) {
            on = !on;            
        }
        if (on) {
            if(text != null)
                text.color = OnColor;
            if(spriteRend != null)
                spriteRend.color = OnColor;
        } else {
            if (text != null)
                text.color = Color.Lerp(text.color, OffColor, LerpSpeed * Time.deltaTime);
            if (spriteRend != null)
                spriteRend.color = Color.Lerp(spriteRend.color, OffColor, LerpSpeed * Time.deltaTime);
        }
	}
}

using UnityEngine;
using System.Collections;

public class TransformCharge : MonoBehaviour 
{
    public Renderer r;
    Material mat;
    Color color = Color.white;
    public float scale = 1;
    public static TransformCharge instance;
    public float lowAlpha, highAlpha;
    bool queued = false;

	// Use this for initialization
	void Awake () {
        instance = this;
        mat = r.material;
        mat.SetColor("_TintColor", color);
        c = color;
	}
    Color c;
	// Update is called once per frame
	void Update () {
        transform.localScale = Vector3.one * BeatManager.instance.TimeToNextTransform() * scale;
        if (BeatManager.instance.canTransform)
        {
            //mat.SetColor("_TintColor", noQueueColor);
            this.color = Color.white;
            queued = false;
            //Debug.Log("Charge");
        }

        c = color;
        c.a = Mathf.Lerp(lowAlpha, highAlpha, BeatManager.instance.TimeToNextTransform() / (BeatManager.instance.beatDuration * 4));

        if (queued)
        {
            
        }
        else
        {
            c.a /= 2;
        }
        mat.SetColor("_TintColor", c);
	}

    public void QueueColor(ObjectColor color)
    {

        if (!queued)
        {
            //if close add 10 styles, else remove 1
            //also enemy deaths = 1 style
        }

        queued = true;
        this.color = BenColored.GetRGB(color);
    }
}

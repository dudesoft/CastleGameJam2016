using UnityEngine;
using System.Collections;

public class TransformCharge : MonoBehaviour 
{
    public Renderer r;
    Material mat;
    public Color noQueueColor;
    public float scale = 1;
    public static TransformCharge instance;
    public float lowAlpha, highAlpha;
    bool queued = false;

	// Use this for initialization
	void Awake () {
        mat = r.material;
        mat.SetColor("_TintColor", noQueueColor);
        c = noQueueColor;
	}
    Color c;
	// Update is called once per frame
	void Update () {
        transform.localScale = Vector3.one * BeatManager.instance.TimeToNextTransform() * scale;
        if (BeatManager.instance.canTransform)
        {
            mat.SetColor("_TintColor", noQueueColor);
            queued = false;
        }

        c = mat.GetColor("_TintColor");
        if (queued)
        {

        }
        else
        {
            c.a = Mathf.Lerp(lowAlpha, highAlpha, BeatManager.instance.TimeToNextTransform())/2;
        }
        mat.SetColor("_TintColor", noQueueColor);
	}

    public void QueueColor(ObjectColor color)
    {
        queued = true;
    }
}

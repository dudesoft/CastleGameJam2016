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
    public ParticleSystem style;

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
            
            if (BeatManager.instance.TimeToNextTransform() < BeatManager.instance.beatDuration + 0.02f)
            {
                SFX.QueueColor();
                if (color != BenShip.instance.currentGun.objectColor)
                {
                    //Stylish
                    Timer.Style += 10;
                    style.transform.position = BenShip.instance.transform.position;
                    style.Emit(1);
                }
                //Trigger niceness
            }
            else if (BeatManager.instance.TimeToNextTransform() < 0.5f)
            {
                //nothing
                SFX.QueueBadColor();
            }
            else if (BeatManager.instance.TimeToNextTransform() > 1f)
            {
                Timer.Style -= 1;
                SFX.QueueBadColor();
            }
            //if close add 10 styles, else remove 1
            //also enemy deaths = 1 style
        }
        else
        {
            SFX.QueueBadColor();
        }

        queued = true;
        this.color = BenColored.GetRGB(color);
    }
}

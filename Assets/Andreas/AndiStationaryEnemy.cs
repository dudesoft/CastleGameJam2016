using UnityEngine;

public class AndiStationaryEnemy : FreBaseEnemy
{
    public BenProjectileSpawner[] childParts;
    public GameObject centerObject;
    public float rotationSpeed;
    private Vector3 point;
    private Vector3 pivot;

    void Update()
    {
        float sine = Mathf.Abs(Mathf.Sin(BeatManager.instance.TimeSinceLastBeat * Mathf.PI)) - 0.5f;
        float cosine = Mathf.Abs(Mathf.Cos(BeatManager.instance.TimeSinceLastBeat * Mathf.PI)) - 0.5f;
        if (objectColor == ObjectColor.Red)
        {
            centerObject.GetComponent<SpriteRenderer>().color = new Color(1, sine, sine);
        }
        else if (objectColor == ObjectColor.Blue)
        {
            centerObject.GetComponent<SpriteRenderer>().color = new Color(sine, sine, 1);
        }
        else if (objectColor == ObjectColor.Green)
        {
            centerObject.GetComponent<SpriteRenderer>().color = new Color(sine, 1, sine);
        }
        else if (objectColor == ObjectColor.Yellow)
        {
            centerObject.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, sine);
        }
        centerObject.transform.Rotate(new Vector3(0, 0, -3));

        foreach (BenProjectileSpawner child in childParts)
        {
            if (objectColor == ObjectColor.Red)
            {
                child.transform.RotateAround(transform.position, Vector3.forward, rotationSpeed);
                child.GetComponent<SpriteRenderer>().color = new Color(1, cosine, cosine);
                child.objectColor = objectColor;
            }
            else if (objectColor == ObjectColor.Blue)
            {
                child.transform.RotateAround(transform.position, Vector3.forward, rotationSpeed);
                child.GetComponent<SpriteRenderer>().color = new Color(cosine, cosine, 1);
                child.objectColor = objectColor;
            }
            else if (objectColor == ObjectColor.Green)
            {
                child.transform.RotateAround(transform.position, Vector3.forward, rotationSpeed);
                child.GetComponent<SpriteRenderer>().color = new Color(cosine, 1, cosine);
                child.objectColor = objectColor;
            }
            else if (objectColor == ObjectColor.Yellow)
            {
                child.transform.RotateAround(transform.position, Vector3.forward, rotationSpeed);
                child.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, cosine);
                child.objectColor = objectColor;
            }
        }
    }
}

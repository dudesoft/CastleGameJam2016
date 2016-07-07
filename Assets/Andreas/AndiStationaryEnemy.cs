using UnityEngine;

public class AndiStationaryEnemy : FreBaseEnemy
{
    public GameObject[] childParts;
    public GameObject centerObject;
    public float rotationSpeed;
    private Vector3 point;
    private Vector3 pivot;

    void Update()
    {
        float sine = Mathf.Abs(Mathf.Sin(Time.time * 10)) - 0.5f;
        float cosine = Mathf.Abs(Mathf.Cos(Time.time * 10)) - 0.5f;
        centerObject.GetComponent<SpriteRenderer>().color = new Color(1, sine, sine);

        centerObject.transform.Rotate(new Vector3(0, 0, -3));

        foreach (GameObject child in childParts)
        {
            child.transform.RotateAround(transform.position, Vector3.forward, rotationSpeed);
            child.GetComponent<SpriteRenderer>().color = new Color(1, cosine, cosine);
        }
    }
}

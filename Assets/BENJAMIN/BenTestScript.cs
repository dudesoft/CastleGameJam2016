using UnityEngine;
using System.Collections;

public class BenTestScript : BenColored {

    public static GameObject playerGO;
    public float suckingDistance = 1;
    public float suckingPower = 10;
	// Use this for initialization
	void Start () {
        playerGO = gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
            ChangeColor(ObjectColor.Red);

        if (Input.GetKeyDown(KeyCode.G))
            ChangeColor(ObjectColor.Green);

        if (Input.GetKeyDown(KeyCode.B))
            ChangeColor(ObjectColor.Blue);

        if (Input.GetKeyDown(KeyCode.Y))
            ChangeColor(ObjectColor.Yellow);

        float dist = 0;

        foreach (BenProjectile bp in BenProjectile.projectiles)
        {
            if ()
            dist = Vector3.Distance(bp.transform.position, transform.position);
            if (dist < suckingDistance)
            {
                bp.velocity -= (bp.transform.position - transform.position).normalized * (suckingPower * ((suckingDistance/2)/dist));
                if (bp.velocity.magnitude > bp.speed)
                    bp.velocity = bp.velocity.normalized * bp.speed;
            }
        }
	}

    void ChangeColor(ObjectColor color)
    {
        if (objectColor != color)
        {
            GetComponent<Renderer>().material.color = BenColored.GetRGB(color);
            objectColor = color;
            gameObject.layer = LayerMask.NameToLayer(color.ToString());
        }
    }
}

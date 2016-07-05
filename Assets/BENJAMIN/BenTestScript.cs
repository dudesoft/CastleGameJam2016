using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BenTestScript : BenColored {

    public static GameObject playerGO;
    public float suckingDistance = 1;
    public float suckingPower = 10;
    public float colliderDistance = 0.25f;
    List<BenProjectile> toDestroy = new List<BenProjectile>();
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
        toDestroy.Clear();
        foreach (BenProjectile bp in BenProjectile.projectiles)
        {
            if (bp.objectColor == objectColor)
            {
                dist = Vector3.Distance(bp.transform.position, transform.position);
                if (dist < colliderDistance)
                {
                    toDestroy.Add(bp);
                }
                else if (dist < suckingDistance)
                {
                    bp.velocity -= (bp.transform.position - transform.position).normalized * (suckingPower * ((suckingDistance / 2) / dist));
                    if (bp.velocity.magnitude > bp.speed)
                        bp.velocity = bp.velocity.normalized * bp.speed;
                }
            }
        }
        foreach (BenProjectile bp in toDestroy)
            bp.Destroy();
        
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, colliderDistance);
    }
}

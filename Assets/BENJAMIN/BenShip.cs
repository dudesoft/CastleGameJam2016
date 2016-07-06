using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BenShip : BenColored {

    public static GameObject playerGO;
    public float suckingDistance = 1;
    public float suckingPower = 10;
    public float colliderDistance = 0.25f;
    List<BenProjectile> toDestroy = new List<BenProjectile>();
    float _moveSpeed = 2;

	// Use this for initialization
	void Start () {
        playerGO = gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
            ChangeShipColor(ObjectColor.Red);

        if (Input.GetKeyDown(KeyCode.G))
            ChangeShipColor(ObjectColor.Green);

        if (Input.GetKeyDown(KeyCode.B))
            ChangeShipColor(ObjectColor.Blue);

        if (Input.GetKeyDown(KeyCode.Y))
            ChangeShipColor(ObjectColor.Yellow);

        float dist = 0;
        toDestroy.Clear();
        foreach (BenProjectile bp in BenProjectile.projectiles)
        {
            if (bp.objectColor == objectColor && bp.canHitPlayer)
            {
                dist = Vector3.Distance(bp.transform.position, transform.position);
                if (dist < colliderDistance)
                {
                    //Player picked up a bullet
                    toDestroy.Add(bp);
                }
                else if (dist < suckingDistance)
                {
                    bp.velocity -= (bp.transform.position - transform.position).normalized * (suckingPower * ((suckingDistance / 2) / dist));
                    if (bp.velocity.magnitude > bp.speed)
                        bp.velocity = bp.velocity.normalized * bp.speed * 1.2f;
                }
            }
        }
        foreach (BenProjectile bp in toDestroy)
            bp.Destroy();
        
	}

    public void ChangeShipColor(ObjectColor color)
    {
        if (objectColor != color)
        {
            GetComponent<Renderer>().material.color = BenColored.GetRGB(color);
            base.ChangeColor(color);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, colliderDistance);
    }
}

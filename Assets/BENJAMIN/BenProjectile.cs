using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BenProjectile : BenColored {

	// Use this for initialization
    public static List<BenProjectile> projectiles = new List<BenProjectile>();

    public float speed = 1, lifeTime = 5;
    public bool followPlayer, bounce, followEnemy, canHitPlayer, canHitEnemy;
    public Vector3 direction;

    [HideInInspector]
    public bool disabled;

	public void Init(Vector3 direction)
    {

    }

    void Update()
    {
        transform.position += direction.normalized * speed * Time.deltaTime;
    }

    void Destroy()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {

    }


}

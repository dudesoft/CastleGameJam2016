using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BenProjectile : BenColored {

	// Use this for initialization
    public static List<BenProjectile> projectiles = new List<BenProjectile>();
    

    public float speed = 1, lifeTime = 5;
    public bool followPlayer, bounce, followEnemy, canHitPlayer, canHitEnemy;
    public Vector3 direction;
    public Vector3 velocity;
    float countdown;
    public BenProjectileSpawner origin;
    public Rigidbody2D rigid;

    [HideInInspector]
    public bool disabled;

    public void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

	public void Init(Vector3 position, Vector3 direction, float timeOffset, float angle, ObjectColor color, BenProjectileSpawner origin)
    {
        this.origin = origin;
        ChangeColor(color);
        projectiles.Add(this);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        countdown = lifeTime;
        transform.position = position;
        this.direction = direction;

        //transform.position += direction.normalized * speed * timeOffset;
        velocity = direction.normalized * speed + new Vector3(origin.rigid.velocity.x, origin.rigid.velocity.y, 0);
        rigid.velocity = velocity;
    }

    void Update()
    {
        //transform.position += velocity * Time.deltaTime;
        rigid.velocity = velocity;
        countdown -= Time.deltaTime;
        if (countdown <= 0)
            this.Destroy();
    }

    public void Destroy()
    {
        projectiles.Remove(this);
        //Destroy(gameObject);
        origin.pool.Release(gameObject);
    }

    public void Impact(Vector3 pos)
    {
        origin.bulletImpact.startColor = Color.Lerp(Color.white, BenColored.GetRGB(objectColor), 0.33f);
        origin.bulletImpact.transform.position = pos;
        origin.bulletImpact.Emit(3);
        InAudio.PlayAtPosition(origin.gameObject, origin.bulletImpactAudio, pos);
        Destroy();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject != origin.gameObject)
        {
            Impact(col.contacts[0].point);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject != origin.gameObject)
        {
            Impact(transform.position);
        }
    }

}

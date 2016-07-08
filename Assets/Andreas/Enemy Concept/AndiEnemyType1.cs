using UnityEngine;
using System.Collections;

public class AndiEnemyType1 : FreBaseEnemy
{
    public float speed = 2;
    public float speedMultiplier;
    private Rigidbody2D rigidbody;

    protected override void Init()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
    }

    public virtual void Move()
    {
        if (BeatManager.instance.beating)
        {
            rigidbody.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * speedMultiplier, ForceMode2D.Impulse);
        }
    }
}

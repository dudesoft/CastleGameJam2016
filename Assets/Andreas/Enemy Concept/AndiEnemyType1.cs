using UnityEngine;
using System.Collections;

public class AndiEnemyType1 : FreBaseEnemy
{
    public float speed = 2;
    private Rigidbody2D rigidbody;

    protected override void Init()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(new Vector2(Random.Range(0.1f, 1), Random.Range(0.1f, 1)) * 10, ForceMode2D.Impulse);
    }

    void Update()
    {
        Move();
    }

    public virtual void Move()
    {
        if (Vector3.Magnitude(rigidbody.velocity) < 5)
        {
            rigidbody.AddForce(new Vector2(Random.Range(0.1f, 1), Random.Range(0.1f, 1)) * 10, ForceMode2D.Impulse);
        }
    }
}

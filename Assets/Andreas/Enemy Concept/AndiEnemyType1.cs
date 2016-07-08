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
            float xSpeed = Random.Range(0.5f, 1f);
            float ySpeed = Random.Range(0.5f, 1f);
            if (Random.Range(0, 1) == 1)
            {
                xSpeed *= -1;
            }
            if (Random.Range(0, 1) == 1)
            {
                ySpeed *= -1;
            }
            rigidbody.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * speedMultiplier, ForceMode2D.Impulse);
        }
    }
}

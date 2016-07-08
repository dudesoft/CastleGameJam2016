using UnityEngine;
using System.Collections;

public class AndiEnemyCharge : FreBaseEnemy
{
    private Vector3 targetLocation;

    public float speed = 2;
    private Vector2 stopLocation;
    private GameObject player;
    private Rigidbody2D rb;
    private bool triggerShot;
    public GameObject particleEmitter;
    private ParticleSystem particleSystem;

    protected override void Init()
    {
        stopLocation = new Vector2(0, 0);
        rb = GetComponent<Rigidbody2D>();
        particleSystem = particleEmitter.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        Move();
    }

    void TurnToTarget()
    {
        if (BeatManager.instance.beat < 3)
        {
            triggerShot = false;
            rb.velocity = new Vector2(0, 0);
            targetLocation = BenShip.instance.gameObject.transform.position;
            Vector3 vectorToTarget = targetLocation - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 10);
        }
        else if (BeatManager.instance.beat == 3)
        {
            if (BeatManager.instance.beating)
            {
                particleSystem.Play();
            }
        }
        else if (BeatManager.instance.beat == 4 && BeatManager.instance.beating)
        {
            if (!triggerShot)
            {
                triggerShot = true;
                stopLocation = BenShip.instance.gameObject.transform.position;
                rb.AddForce(transform.right * 15, ForceMode2D.Impulse);
            }
        }
    }

    public virtual void Move()
    {
        TurnToTarget();

        // rigidbody.velocity = transform.right * speed;
    }
}

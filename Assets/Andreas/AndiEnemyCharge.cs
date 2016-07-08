using UnityEngine;
using System;

public class AndiEnemyCharge : FreBaseEnemy
{
    private Vector3 targetLocation;

    public float speed = 2;
    private Vector2 stopLocation;
    private GameObject player;
    private Rigidbody2D rb;
    private bool triggerShot;
    public GameObject particleEmitter;
    private int[] beatPattern;
    private ParticleSystem particleSystem;

    protected override void Init()
    {
        beatPattern = createBeatPattern();
        stopLocation = new Vector2(0, 0);
        rb = GetComponent<Rigidbody2D>();
        particleSystem = particleEmitter.GetComponent<ParticleSystem>();
    }

    private int[] createBeatPattern()
    {
        int rnd = UnityEngine.Random.Range(0, 3);

        switch(rnd)
        {
            case 0:
                return new int[] { 1, 2, 3, 4 };
            case 1:
                return new int[] { 2, 3, 4, 1 };
            case 2:
                return new int[] { 3, 4, 1, 2 };
            default:
                return new int[] { 4, 1, 2, 3 };
        }
        
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (BeatManager.instance.beat == beatPattern[0] || BeatManager.instance.beat == beatPattern[1])
        {
            triggerShot = false;
            rb.velocity = new Vector2(0, 0);
            targetLocation = BenShip.instance.gameObject.transform.position;
            Vector3 vectorToTarget = targetLocation - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 10);
        }
        else if (BeatManager.instance.beat == beatPattern[2])
        {
            if (BeatManager.instance.beating)
            {
                particleSystem.Play();
            }
        }
        else if (BeatManager.instance.beat == beatPattern[3] && BeatManager.instance.beating)
        {
            if (!triggerShot)
            {
                triggerShot = true;
                stopLocation = BenShip.instance.gameObject.transform.position;
                rb.AddForce(transform.right * UnityEngine.Random.Range(13, 18), ForceMode2D.Impulse);
            }
        }
    }
}

using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private Vector3 targetLocation;

    public float speed = 2;
    public int life = 100;
    public int value = 100;

    private Camera camera2d;
    private bool moveAllowed = true;
    private bool slowed = false;

    void Start()
    {
        camera2d = Camera.main;

        targetLocation = GetNewTargetLocation();
    }

    void Update()
    {
        Shoot();

        if (moveAllowed)
        {
            Move();
        }

        if (life <= 0)
        {
            DestroyEnemy();
        }
    }

    public virtual void Shoot()
    {
        // Override if shooting necessary
    }

    public virtual void Move()
    {
        TurnToTarget();

        transform.position = transform.position + transform.right * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, targetLocation) < 1)
        {
            targetLocation = GetNewTargetLocation();
        }
    }

    public virtual void DestroyEnemy()
    {
        // handle destruction

        Destroy(gameObject);
    }

    public Vector2 GetNewTargetLocation()
    {
        float screenX = Random.Range(0.02f, 0.98f);
        float screenY = Random.Range(0.05f, 0.95f);
        Vector2 target = camera2d.GetComponent<Camera>().ViewportToWorldPoint(new Vector2(screenX, screenY));

        return target;
    }

    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        // handle collision somehow
    }

    void TurnToTarget()
    {
        Vector3 vectorToTarget = targetLocation - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
    }

    public void StopEnemyMovement()
    {
        moveAllowed = false;
    }

    public virtual void SlowEnemy()
    {
        if (!slowed)
        {
            SetSpeed(speed * 0.3f);
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 1);
        }
    }

    public void StartEnemyMovement()
    {
        moveAllowed = true;
    }

    public bool GetMoveAllowed()
    {
        return moveAllowed;
    }

    public void SetSpeed(float value)
    {
        speed = value;
    }

    public void TakeDamage(int value)
    {
        life -= value;
    }
}
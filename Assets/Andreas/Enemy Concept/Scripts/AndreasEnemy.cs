using UnityEngine;

public class AndreasEnemy : FreBaseEnemy
{
    private Vector3 targetLocation;

    public float speed = 2;

//	public FrePlayerMovement playerDummy;
    private bool engaging = true;

    private Camera camera2d;
    private bool moveAllowed = true;
    private bool slowed = false;

	void Awake()
    {
		Init();
    }

	protected override void Init()
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

    }

    public virtual void Shoot()
    {
        // Override if shooting necessary
    }

    public virtual void Move()
    {
        TurnToTarget();

        transform.position = transform.position + transform.right * speed * Time.deltaTime;

        if (engaging && Vector3.Distance(transform.position, targetLocation) < 2)
        {
            targetLocation = GetNewTargetLocationOutSide();
            engaging = !engaging;
            return;
        }

        if (Vector3.Distance(transform.position, targetLocation) < 1)
        {
            engaging = !engaging;
            targetLocation = GetNewTargetLocation();
        }
    }

    private Vector3 GetNewTargetLocationOutSide()
    {
		return RotateAroundPoint(playerObject.transform.position, transform.position, Quaternion.Euler(0, 0, Random.Range(-80, 80)));
    }


    public Vector2 GetNewTargetLocation()
    {
        float screenX = Random.Range(0.02f, 0.98f);
        float screenY = Random.Range(0.05f, 0.95f);
        Vector2 target = camera2d.GetComponent<Camera>().ViewportToWorldPoint(new Vector2(screenX, screenY));

        return target;
    }


    void TurnToTarget()
    {
        if (engaging)
        {
			targetLocation = playerObject.transform.position;
        }
        Vector3 vectorToTarget = targetLocation - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
    }

    public Vector3 RotateAroundPoint(Vector3 pivot, Vector3 point, Quaternion angle)
    {
        return (angle * (point - pivot) + pivot) + (pivot - point) * -Random.Range(3f, 5f);
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


}
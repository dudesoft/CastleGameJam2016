using UnityEngine;
using System.Collections;

public class BenProjectileSpawner : BenColored {

    public BenProjectile projectile;
    public float fireRate = 0.1f;
    float wait = 0;
    public float fireDistance = 0.25f;
    public bool fireing = false;
    public float randomSpread = 0f;
    public int poolSize = 10;

    public int simultaneousProjectiles = 1;
    public float spread = 0; //how much do the simultaneous bulltes spread out - 360 means you fire bullets in a circle

    public bool isPlayer = false;
    public ParticleSystem muzzle;
    public ParticleSystem bulletImpact;

    //public InAudioNode bulletImpactAudio, shootAudio;

    [HideInInspector]
    public Pool pool;
    [HideInInspector]
    public Rigidbody2D rigid;
    [HideInInspector]
    public FrePlayerMovement player;

	// Use this for initialization
	void Start () {
        if (isPlayer)
            player = GetComponent<FrePlayerMovement>();

        rigid = GetComponent<Rigidbody2D>();
        pool = AutoPool.GetPool(projectile.gameObject, poolSize);
        //pool.Initialize(projectile.gameObject, poolSize, 10f);
	}

    public float angle;

	// Update is called once per frame
	void Update () {
		 
		if (isPlayer)
            fireing = Input.GetMouseButton(0);

        //Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //if (isPlayer)
        //    angle = Mathf.Atan2(player.lookDirection.y, player.lookDirection.x) * Mathf.Rad2Deg;
        //else
            angle = Mathf.Atan2(-transform.up.x, transform.up.y) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        

        if (fireing && (BenShip.instance.canFire && isPlayer || !isPlayer))
        {
            Quaternion fromRot = transform.rotation;
            while (wait >= fireRate)
            {
                wait -= fireRate;

				float angleOffset = -spread/2 + Random.Range(-randomSpread/2,randomSpread/2);

                if (simultaneousProjectiles <= 1)
					angleOffset = Random.Range(-randomSpread/2,randomSpread/2);

                for (int i = 0; i < simultaneousProjectiles; i++)
                {
                    BenProjectile p = pool.Get().GetComponent<BenProjectile>();//Instantiate(projectile);

                    transform.rotation = Quaternion.AngleAxis(angle + angleOffset, Vector3.forward);

                    p.Init(transform.position + transform.right * fireDistance, transform.right, wait, angleOffset + angle + Random.Range(-randomSpread, randomSpread) * Random.Range(0, 1f), objectColor, this);
                    p.gameObject.SetActive(true);

                    if (simultaneousProjectiles > 1)
                        angleOffset += spread / (simultaneousProjectiles - 1);
                    
                }
                //InAudio.Play(gameObject, shootAudio);
                if (muzzle)
                    muzzle.Emit(5);
            }

            transform.rotation = fromRot;
        }
        
        wait += Time.deltaTime;

        if (!fireing || (BenShip.instance.canFire && isPlayer))
        {
            wait = Mathf.Clamp(wait, 0, fireRate);
        }
        
	}
}

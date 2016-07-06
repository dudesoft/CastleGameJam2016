using UnityEngine;
using System.Collections;

public class BenProjectileSpawner : BenColored {

    public BenProjectile projectile;
    public float fireRate = 0.1f;
    float wait = 0;
    public float fireDistance = 0.25f;
    public bool fireing = false;
    public float spread = 1f;
    public int poolSize = 10;

    public bool isPlayer = false;
    public ParticleSystem muzzle;
    public ParticleSystem bulletImpact;

    //public InAudioNode bulletImpactAudio, shootAudio;

    public Pool pool;
    [HideInInspector]
    public Rigidbody2D rigid;
    public FrePlayerMovement player;

	// Use this for initialization
	void Start () {
        player = GetComponent<FrePlayerMovement>();
        rigid = GetComponent<Rigidbody2D>();
        pool = AutoPool.GetPool(projectile.gameObject, poolSize);
        //pool.Initialize(projectile.gameObject, poolSize, 10f);
	}

    public float angle;

	// Update is called once per frame
	void Update () {

        if (player != null)
            fireing = Input.GetMouseButton(0);

        //Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        angle = Mathf.Atan2(player.lookDirection.y, player.lookDirection.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        

        if (fireing && BenShip.instance.canFire)
        {
            while (wait >= fireRate)
            {
                wait -= fireRate;
                BenProjectile p = pool.Get().GetComponent<BenProjectile>();//Instantiate(projectile);
                p.canHitPlayer = false;
                p.canHitEnemy = true;
                p.Init(transform.position + transform.right * fireDistance, transform.right, wait, angle + Random.Range(-spread, spread) * Random.Range(0, 1f), objectColor, this);
                p.gameObject.SetActive(true);
                //InAudio.Play(gameObject, shootAudio);
                muzzle.Emit(5);
            }
        }
        
        wait += Time.deltaTime;

        if (!fireing || !BenShip.instance.canFire)
        {
            wait = Mathf.Clamp(wait, 0, fireRate);
        }
        
	}
}

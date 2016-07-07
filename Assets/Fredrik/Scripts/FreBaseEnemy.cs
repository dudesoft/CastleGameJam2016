using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]
public class FreBaseEnemy : BenColored {

	int curentHealth;
	public int health =50;
	public int collisionDamage;
	public int bulletDamage;
	protected static FrePlayerMovement playerObject;
	public delegate void EnemyDiedEventHandler(FreBaseEnemy enemy);
	public event EnemyDiedEventHandler Died;


	Color enemyColor;
	SpriteRenderer spr;
	bool hitFlash;
	// Use this for initialization
	void Start () {
		if(playerObject == null)
		{
			playerObject = (FrePlayerMovement) FindObjectOfType<FrePlayerMovement>();
		}
		spr = GetComponent<SpriteRenderer>();
		UpdateColor();
	}

	void OnEnable()
	{
		curentHealth = health;
		Init();
	}

	protected virtual void Init()
	{

	}
	// Update is called once per frame
	void LateUpdate () {
		UpdateColor();
	}
		
	void DealDamage(int damage)
	{

		curentHealth -= damage;
		if(curentHealth <= 0)
		{
			EnemyDies();
		}
	}

	void EnemyDies()
	{

		Died(this);
	}

	void UpdateColor()
	{
		enemyColor = BenColored.GetRGB(objectColor);

		if(hitFlash)
			enemyColor.a =0.5f;

		hitFlash = false;
		spr.color = enemyColor;
	}

	void ProjectileHit(BenProjectile p)
	{
		if(p != null) 
		{
			if(p.objectColor != objectColor && p.canHitEnemy)
			{
				DealDamage(p.damage);
				hitFlash = true;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		BenProjectile p = col.gameObject.GetComponent<BenProjectile>();

		ProjectileHit(p);
	}
		

	void OnTriggerEnter2D(Collider2D col)
	{
		BenProjectile p = col.gameObject.GetComponent<BenProjectile>();
		ProjectileHit(p);
		
	}
}

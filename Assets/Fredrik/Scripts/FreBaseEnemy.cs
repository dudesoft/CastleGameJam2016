using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]
public class FreBaseEnemy : BenColored {

	public int health =50;
	public int collisionDamage;
	public int bulletDamage;

	public delegate void EnemyDiedEventHandler(FreBaseEnemy enemy);
	public event EnemyDiedEventHandler Died;


	Color enemyColor;
	SpriteRenderer spr;
	bool hitFlash;
	// Use this for initialization
	void Awake () {
		
		spr = GetComponent<SpriteRenderer>();
		UpdateColor();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		UpdateColor();
	}
		
	void DealDamage(int damage)
	{

		health -= damage;
		if(health <= 0)
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

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
	protected float scale = 1.5f;

	Color enemyColor;
	SpriteRenderer spr;
	bool hitFlash;
	// Use this for initialization
	void Start () {
		if(playerObject == null)
		{
			playerObject = (FrePlayerMovement) FindObjectOfType<FrePlayerMovement>();
		}
		tag = "Enemy";
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
		scale = Mathf.MoveTowards(scale,1, Time.deltaTime);
		 
	//	transform.localScale = scale * Vector3.one;
	}
		
	public void DealDamage(int damage)
	{
		hitFlash=true;
		curentHealth -= damage;
		scale = 0.9f;
		print(curentHealth);
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
		enemyColor = Color.Lerp(enemyColor, BenColored.GetRGB(objectColor),Time.deltaTime*5);

		if(hitFlash)
			enemyColor.a =0.1f;

		hitFlash = false;
		spr.color = enemyColor;
	}

}

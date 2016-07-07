using UnityEngine;
using System.Collections;

public class FreSqrEnemy : FreBaseEnemy {

	public GameObject target;
	public float timeBetweenJumps;
	public float jumpDistance;
	float timeToNextJump;
	float scale;
	public float speed;
	Vector2 goalPos;
	// Use this for initialization
	protected override void Init()
	{
		goalPos =transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		timeToNextJump += Time.deltaTime;
		if(timeToNextJump > 0)
		{
			timeToNextJump -= timeBetweenJumps;
			scale = 1.5f;  

			Vector2 posDifference;
			posDifference =  target.transform.position-transform.position; 
			if(Mathf.Abs(posDifference.x) > Mathf.Abs(posDifference.y))
			{
				if(posDifference.x >0)
				{
					goalPos.x += jumpDistance;
				}
				else
				{
					goalPos.x -= jumpDistance;
				}

			}
			else
			{
				if(posDifference.y >0)
				{
					goalPos.y += jumpDistance;
				}
				else
				{
					goalPos.y -= jumpDistance;
				}
			}
		}

		scale = Mathf.MoveTowards(scale,1,Time.deltaTime*2);

		transform.localScale = scale * Vector3.one;

		transform.position = Vector3.MoveTowards(transform.position,goalPos,speed*Time.deltaTime);
	}
}

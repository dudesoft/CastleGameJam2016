using UnityEngine;
using System.Collections;

public class FreSqrEnemy : FreBaseEnemy {



	public float jumpDistance;

	public float speed;
	Vector2 goalPos;
	// Use this for initialization
	protected override void Init()
	{
//		BeatManager.instance.
		goalPos =transform.position;
	
	}
	
	// Update is called once per frame
	void Update () {

	//	timeToNextJump += Time.deltaTime;
		if(BeatManager.instance.beating)
		{
	//		timeToNextJump -= timeBetweenJumps;
			scale = 1.5f;  

			Vector2 posDifference;
			posDifference =  playerObject.transform.position-transform.position; 
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

	//	scale = Mathf.MoveTowards(scale,1,Time.deltaTime*2);

		transform.localScale = scale * Vector3.one;

		transform.position = Vector3.MoveTowards(transform.position,goalPos,speed*Time.deltaTime);
	}
}

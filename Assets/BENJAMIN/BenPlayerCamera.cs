using UnityEngine;
using System.Collections;

public class BenPlayerCamera : MonoBehaviour 
{
    bool fixedTarget = false;
    public float lerpSpeed = 3;

    public FrePlayerMovement player;

	// Use this for initialization
	void Start () 
    {
	    
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ScreenShake(3f, 0.2f);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            ScreenShake(5f, 0.2f);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            ScreenShake(7f, 0.2f);
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
	    if (fixedTarget)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position /*fixed target here*/, Time.deltaTime * lerpSpeed);
            //Lerp camera scale as well;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, PlayerTarget() + shake, Time.deltaTime * lerpSpeed);
        }
	}

    Vector3 PlayerTarget()
    {
        return player.transform.position - Vector3.forward * 10 + player.lookDirection.normalized * player.lookDistance;
    }

    Vector3 shake = new Vector3();

    void ScreenShake(float magnitude, float duration)
    {
        StartCoroutine(ShakeRoutine(magnitude, duration));
    }

    IEnumerator ShakeRoutine(float magnitude, float duration)
    {
        float f = 0;
        while (f < duration)
        {
            f += Time.deltaTime;
            shake = new Vector3(Random.Range(-magnitude, magnitude), Random.Range(-magnitude, magnitude), 0);
            shake *= 1 - (f / duration);
            yield return null;
        }
        shake = new Vector3();
    }
}

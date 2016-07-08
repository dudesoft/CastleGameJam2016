using UnityEngine;
using System.Collections;

public class BenPlayerCamera : MonoBehaviour 
{
    bool fixedTarget = false;
    public float lerpSpeed = 3;
    public float cameraSize = 5;
    public static BenPlayerCamera instance;

    public FrePlayerMovement player;
    Camera camera;

	// Use this for initialization
	void Start () 
    {
        instance = this;
        camera = GetComponent<Camera>();
	}

    void Update()
    {

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
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, cameraSize, Time.deltaTime * lerpSpeed);

	}

    Vector3 PlayerTarget()
    {
        return player.transform.position - Vector3.forward * 10 + player.lookDirection.normalized * player.lookDistance;
    }

    Vector3 shake = new Vector3();

    public void ScreenShake(float magnitude, float duration)
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

using UnityEngine;
using System.Collections;

public class DummyEnemy : AbstractEnemy {
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // Die if outside of viewport
        float yHalfSize = Camera.main.orthographicSize;
        float xHalfSize = Camera.main.aspect * yHalfSize;
        float camX = Camera.main.transform.position.x;
        float camY = Camera.main.transform.position.y;
        if (Mathf.Abs(transform.position.x - camX) > xHalfSize || Mathf.Abs(transform.position.y - camY) > xHalfSize) {
            Die();
        }
	}
}

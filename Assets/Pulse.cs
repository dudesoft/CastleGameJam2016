using UnityEngine;
using System.Collections;

public class Pulse : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	// Update is called once per frame
	void Update () {
        transform.localScale = Vector3.one * (1 + (Mathf.Sin(Time.time * 5) * 0.25f));
	}
}

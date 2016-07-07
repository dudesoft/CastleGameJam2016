using UnityEngine;
using System.Collections;

public class DisableOnLoadStart : MonoBehaviour {
    private SceneLoader loader;
	// Use this for initialization
	void Start () {
        SceneLoader loader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();
        loader.LoadStarted += Disable;
    }
	
    public void Disable() {
        gameObject.SetActive(false);
    }
}

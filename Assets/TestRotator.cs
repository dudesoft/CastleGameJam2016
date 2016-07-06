using UnityEngine;
using System.Collections;

public class TestRotator : MonoBehaviour {

    public GameObject reference;
    public int test;
    private Vector3 huehue;

	void Start () {
        huehue = reference.transform.position - transform.position;

    }
	
	void Update () {
        Debug.DrawLine(transform.position, transform.position + huehue);
        huehue = Quaternion.Euler(0, 0, test) * transform.position;
    }
}

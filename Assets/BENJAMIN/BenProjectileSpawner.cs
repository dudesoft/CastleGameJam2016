using UnityEngine;
using System.Collections;

public class BenProjectileSpawner : MonoBehaviour {

    public BenProjectile projectile;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

	    if (Input.GetMouseButton(0))
        {
            BenProjectile p = Instantiate(projectile);
            p.Init(Vector3.left);
        }
	}
}

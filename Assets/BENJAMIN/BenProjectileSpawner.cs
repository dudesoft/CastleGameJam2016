using UnityEngine;
using System.Collections;

public class BenProjectileSpawner : BenColored {

    public BenProjectile projectile;
    public float fireRate = 0.1f;
    float wait = 0;
    float fireDistance = 0.25f;
    bool fireing = false;
    public float spread = 1f;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

        fireing = Input.GetMouseButton(0);

        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        if (fireing)
        {
            while (wait >= fireRate)
            {
                wait -= fireRate;
                BenProjectile p = Instantiate(projectile);
                p.Init(transform.position + transform.right * fireDistance, transform.right, wait, angle + Random.Range(-spread, spread) * Random.Range(0, 1f), objectColor);
            }
        }
        
        wait += Time.deltaTime;

        if (!fireing)
        {
            wait = Mathf.Clamp(wait, 0, fireRate);
        }
        
	}
}

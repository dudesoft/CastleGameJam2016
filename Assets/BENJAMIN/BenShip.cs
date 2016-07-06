using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BenShip : BenColored {

    public static BenShip instance;
    public static GameObject playerGO;
    public float suckingDistance = 1;
    public float suckingPower = 10;
    public float colliderDistance = 0.25f;
    List<BenProjectile> toDestroy = new List<BenProjectile>();
    float _moveSpeed = 2;

    public bool canFire = true;

    public ParticleSystem colorPulse, muzzle;

    public BenProjectileSpawner redGun, greenGun, blueGun, yellowGun;

    public BenProjectileSpawner currentGun;

    public GameObject shipCore, shipRWing, shipLWing, shipFront, shipRRocket, shipLRocket;

    public ShipConfiguration redShip, greenShip, blueShip, yellowShip;


	// Use this for initialization
	void Start () {
        
        currentGun = redGun;
        playerGO = gameObject;
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
            ChangeShipColor(ObjectColor.Red);

        if (Input.GetKeyDown(KeyCode.G))
            ChangeShipColor(ObjectColor.Green);

        if (Input.GetKeyDown(KeyCode.B))
            ChangeShipColor(ObjectColor.Blue);

        if (Input.GetKeyDown(KeyCode.Y))
            ChangeShipColor(ObjectColor.Yellow);

        float dist = 0;
        toDestroy.Clear();
        foreach (BenProjectile bp in BenProjectile.projectiles)
        {
            if (bp.objectColor == objectColor && bp.canHitPlayer)
            {
                dist = Vector3.Distance(bp.transform.position, transform.position);
                if (dist < colliderDistance)
                {
                    //Player picked up a bullet
                    toDestroy.Add(bp);
                }
                else if (dist < suckingDistance)
                {
                    bp.velocity -= (bp.transform.position - transform.position).normalized * (suckingPower * ((suckingDistance / 2) / dist));
                    if (bp.velocity.magnitude > bp.speed)
                        bp.velocity = bp.velocity.normalized * bp.speed * 1.2f;
                }
            }
        }
        foreach (BenProjectile bp in toDestroy)
            bp.Destroy();
        
	}

    public void DoTransformation(ObjectColor color)
    {
        canFire = false;
        switch (color)
        {
            case ObjectColor.Red: StartCoroutine(TransformToRed(redShip)); break;
            case ObjectColor.Green: StartCoroutine(TransformToRed(greenShip)); break;
            case ObjectColor.Blue: StartCoroutine(TransformToRed(blueShip)); break;
            case ObjectColor.Yellow: StartCoroutine(TransformToRed(yellowShip)); break;
            default: break;
        }
    }

    public IEnumerator TransformToRed(ShipConfiguration ship)
    {
        LeanTween.rotateLocal(shipCore, ship.shipCore.localEulerAngles, 0.8f);
        LeanTween.rotateLocal(shipRWing, ship.shipRWing.localEulerAngles, 0.8f);
        yield return new WaitForSeconds(0.1f);

        LeanTween.moveLocal(shipCore, ship.shipCore.localPosition, 0.2f);
        LeanTween.rotateLocal(shipCore, ship.shipCore.localEulerAngles, 0.8f);
        LeanTween.moveLocal(shipRWing, ship.shipRWing.localPosition, 0.2f);
        LeanTween.rotateLocal(shipLWing, ship.shipLWing.localEulerAngles, 0.8f);

        yield return new WaitForSeconds(0.1f);
        LeanTween.moveLocal(shipLWing, ship.shipLWing.localPosition, 0.2f);
        LeanTween.moveLocal(shipFront, ship.shipFront.localPosition, 0.2f);
        LeanTween.rotateLocal(shipFront, ship.shipFront.localEulerAngles, 0.8f);

        yield return new WaitForSeconds(0.1f);

        LeanTween.moveLocal(shipRRocket, ship.shipRRocket.localPosition, 0.2f);
        LeanTween.rotateLocal(shipRRocket, ship.shipRRocket.localEulerAngles, 0.8f);

        LeanTween.moveLocal(shipLRocket, ship.shipLRocket.localPosition, 0.2f);
        LeanTween.rotateLocal(shipLRocket, ship.shipLRocket.localEulerAngles, 0.8f);

        yield return null;
        canFire = true;
    }

    public void ChangeShipColor(ObjectColor color)
    {
        if (objectColor != color)
        {
            DoTransformation(color);
            if (currentGun != null)
                currentGun.enabled = false;
            currentGun = GetGun(color);
            if (currentGun != null)
                currentGun.enabled = true;
            Color c = BenColored.GetRGB(color);
            GetComponent<Renderer>().material.color = c;
            c = Color.Lerp(c, Color.white, 0.5f);
            c.a = 0.66f;
            colorPulse.startColor = c;
            colorPulse.Emit(1);
            muzzle.startColor = c;
            base.ChangeColor(color);
        }
    }

    public BenProjectileSpawner GetGun(ObjectColor color)
    {
        switch (color)
        {
            case ObjectColor.Red: return redGun;
            case ObjectColor.Green: return greenGun;
            case ObjectColor.Blue: return blueGun;
            case ObjectColor.Yellow: return yellowGun;
        }
            return redGun;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, colliderDistance);
    }
}

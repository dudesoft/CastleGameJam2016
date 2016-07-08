using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class BenShip : BenColored {

    public Color shipRed, shipGreen, shipBlue, shipYellow;

    public static BenShip instance;
    public static GameObject playerGO;
    public float suckingDistance = 1;
    public float suckingPower = 10;
    public float colliderDistance = 0.25f;
    List<BenProjectile> toDestroy = new List<BenProjectile>();
    float _moveSpeed = 2;

    public bool canFire = true;

    public int redAmmo = 200, maxRedAmmo = 500;
    public int greenAmmo = 30, maxGreenAmmo = 40;
    public int blueAmmo = 140, maxBlueAmmo = 200;
    public int yellowAmmo = 15, maxYellowAmmo = 20;

    public ParticleSystem colorPulse, muzzle;

    public BenProjectileSpawner redGun, greenGun, blueGun, yellowGun;

    public BenProjectileSpawner currentGun;

    public GameObject shipCore, shipRWing, shipLWing, shipFront, shipRRocket, shipLRocket;

    public ShipConfiguration redShip, greenShip, blueShip, yellowShip;

    public int TransformRedAmmoRefill;
    public int TransformGreenAmmoRefill;
    public int TransformBlueAmmoRefill;
    public int TransformYellowAmmoRefill;


    // Use this for initialization
    void Start () {
        
        currentGun = redGun;
        playerGO = gameObject;
        instance = this;
        ChangeShipColor(ObjectColor.Red);
	}
	
	// Update is called once per frame

    public ObjectColor nextTransform = ObjectColor.Red;

	void Update () {

        if (BeatManager.instance.beating)
        {
            BenShip.instance.transform.localScale = Vector3.one * 1.1f;
            LeanTween.scale(gameObject, Vector3.one, 0.1f);
        }

        if (Input.GetButtonDown("Red"))
        {
            nextTransform = ObjectColor.Red;
            TransformCharge.instance.QueueColor(nextTransform);
            SFX.QueueColor();
        }
        if (Input.GetButtonDown("Green"))
        {
            nextTransform = ObjectColor.Green;
            TransformCharge.instance.QueueColor(nextTransform);
            SFX.QueueColor();
        }
        if (Input.GetButtonDown("Blue"))
        {
            nextTransform = ObjectColor.Blue;
            TransformCharge.instance.QueueColor(nextTransform);
            SFX.QueueColor();
        }
        if (Input.GetButtonDown("Yellow"))
        {
            nextTransform = ObjectColor.Yellow;
            TransformCharge.instance.QueueColor(nextTransform);
            SFX.QueueColor();
        }
        
        if (BeatManager.instance.canTransform && nextTransform != objectColor)
        {
            ChangeShipColor(nextTransform);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            RefillAmmo();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }

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
                    PickUpAmmo(bp);

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

    

    void RefillAmmo()
    {
        redAmmo = maxRedAmmo;
        greenAmmo = maxGreenAmmo;
        blueAmmo = maxBlueAmmo;
        yellowAmmo = maxYellowAmmo;

        AmmoRing.instance.UpdateAmmo();
        ButtonUI.instance.UpdateAmmo();
    }

    public bool CanUseAmmo(ObjectColor color)
    {
        switch (color)
        {
            case ObjectColor.Red: return redAmmo > 0;
            case ObjectColor.Green: return greenAmmo > 0;
            case ObjectColor.Blue: return blueAmmo > 0;
            case ObjectColor.Yellow: return yellowAmmo > 0;
        }

        return false;
    }

    /// <summary>
    /// Returns true if 1 ammo was consumed, and the ProjectileSpawner can shoot
    /// </summary>
    /// <param name="bp"></param>
    /// <returns></returns>
 
    public bool Shoot(BenProjectileSpawner bp)
    {
        if (CanUseAmmo(bp.objectColor))
        {
            AmmoRing.instance.Pulse(1.05f, 0.1f);
            //Trigger UI update
            
            switch (bp.objectColor)
            {
                case ObjectColor.Red: redAmmo--; break;
                case ObjectColor.Green: greenAmmo--; break;
                case ObjectColor.Blue: blueAmmo--; break;
                case ObjectColor.Yellow: yellowAmmo--; break;
            }

            AmmoRing.instance.UpdateAmmo();
            ButtonUI.instance.UpdateAmmo();

            return true;
        }
        else
        {
            SFX.NoAmmo();
            AmmoRing.instance.UpdateAmmo();
            ButtonUI.instance.UpdateAmmo();
            return false;
        }
    }

    public void PickUpAmmo(BenProjectile p)
    {
        switch (p.objectColor)
        {
            case ObjectColor.Red: 
                redAmmo = Mathf.Clamp(redAmmo + p.ammoValue, 0, maxRedAmmo); 
                break;

            case ObjectColor.Green:
                greenAmmo = Mathf.Clamp(greenAmmo + p.ammoValue, 0, maxGreenAmmo);
                break;

            case ObjectColor.Blue:
                blueAmmo = Mathf.Clamp(blueAmmo + p.ammoValue, 0, maxBlueAmmo);
                break;
            case ObjectColor.Yellow:
                yellowAmmo = Mathf.Clamp(yellowAmmo + p.ammoValue, 0, maxYellowAmmo);
                break;

            default: break;
        }

        AmmoRing.instance.Pulse();
        AmmoRing.instance.UpdateAmmo();
        ButtonUI.instance.UpdateAmmo();
    }

    public int GetCurrentAmmoCount()
    {
        switch (currentGun.objectColor)
        {
            case ObjectColor.Red: return redAmmo;
            case ObjectColor.Green: return greenAmmo;
            case ObjectColor.Blue: return blueAmmo;
            case ObjectColor.Yellow: return yellowAmmo;
        }
        return 0;
    }

    public int GetCurrentMaxAmmo()
    {
        switch (currentGun.objectColor)
        {
            case ObjectColor.Red: return maxRedAmmo;
            case ObjectColor.Green: return maxGreenAmmo;
            case ObjectColor.Blue: return maxBlueAmmo;
            case ObjectColor.Yellow: return maxYellowAmmo;
        }
        return 1;
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

    Color GetCoreColor(ObjectColor color)
    {
        switch (color)
        {
            case ObjectColor.Red: return shipRed;
            case ObjectColor.Green: return shipGreen;
            case ObjectColor.Blue: return shipBlue;
            case ObjectColor.Yellow: return shipYellow;
            default: return shipRed;
        }
    }

    public IEnumerator TransformToRed(ShipConfiguration ship)
    {
        GetComponent<PolygonCollider2D>().points = ship.GetComponent<PolygonCollider2D>().points;
        shipCore.GetComponent<Renderer>().material.SetColor("_EmissionColor", GetCoreColor(ship.colorMode));

        LeanTween.rotateLocal(shipCore, ship.shipCore.localEulerAngles, 0.8f).setEase(LeanTweenType.easeInOutBack);
        LeanTween.rotateLocal(shipRWing, ship.shipRWing.localEulerAngles, 0.8f).setEase(LeanTweenType.easeInOutBack);
        yield return new WaitForSeconds(0.1f);

        LeanTween.moveLocal(shipCore, ship.shipCore.localPosition, 0.2f).setEase(LeanTweenType.easeInOutBack);
        LeanTween.rotateLocal(shipCore, ship.shipCore.localEulerAngles, 0.8f).setEase(LeanTweenType.easeInOutBack);
        LeanTween.moveLocal(shipRWing, ship.shipRWing.localPosition, 0.2f).setEase(LeanTweenType.easeInOutBack);
        LeanTween.rotateLocal(shipLWing, ship.shipLWing.localEulerAngles, 0.8f).setEase(LeanTweenType.easeInOutBack);

        yield return new WaitForSeconds(0.1f);
        LeanTween.moveLocal(shipLWing, ship.shipLWing.localPosition, 0.2f).setEase(LeanTweenType.easeInOutBack);
        LeanTween.moveLocal(shipFront, ship.shipFront.localPosition, 0.2f).setEase(LeanTweenType.easeInOutBack);
        LeanTween.rotateLocal(shipFront, ship.shipFront.localEulerAngles, 0.8f).setEase(LeanTweenType.easeInOutBack);

        yield return new WaitForSeconds(0.1f);

        LeanTween.moveLocal(shipRRocket, ship.shipRRocket.localPosition, 0.2f).setEase(LeanTweenType.easeInOutBack);
        LeanTween.rotateLocal(shipRRocket, ship.shipRRocket.localEulerAngles, 0.8f).setEase(LeanTweenType.easeInOutBack);

        LeanTween.moveLocal(shipLRocket, ship.shipLRocket.localPosition, 0.2f).setEase(LeanTweenType.easeInOutBack);
        LeanTween.rotateLocal(shipLRocket, ship.shipLRocket.localEulerAngles, 0.8f).setEase(LeanTweenType.easeInOutBack);

        yield return null;
        canFire = true;
    }

    public void ChangeShipColor(ObjectColor color)
    {
        if (objectColor != color)
        {
            SFX.Transform();
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
            if (currentGun)
                AmmoRing.instance.ChangeWeapon(currentGun);

            // Refill ammo for other colors
            redAmmo = Mathf.Min(color == ObjectColor.Red ? redAmmo : redAmmo + TransformRedAmmoRefill, maxRedAmmo);
            greenAmmo = Mathf.Min(color == ObjectColor.Green ? greenAmmo : greenAmmo + TransformGreenAmmoRefill, maxGreenAmmo);
            blueAmmo = Mathf.Min(color == ObjectColor.Blue ? blueAmmo : blueAmmo + TransformBlueAmmoRefill, maxBlueAmmo);
            yellowAmmo = Mathf.Min(color == ObjectColor.Yellow ? yellowAmmo : yellowAmmo + TransformYellowAmmoRefill, maxYellowAmmo);

            AmmoRing.instance.UpdateAmmo();
            ButtonUI.instance.UpdateAmmo();

            AmmoRing.instance.UpdateAmmo();
            ButtonUI.instance.ChangeWeapon(currentGun.objectColor);
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

    public void TakeDamage()
    {

    }

    public void Die()
    {
        BeatManager.instance.StartCoroutine(PlayerDeath());
    }

    IEnumerator PlayerDeath()
    {
        BenShip.instance.gameObject.SetActive(false);
        ParticleSystem explosion = ((GameObject)Instantiate(Resources.Load("Explosion"))).GetComponent<ParticleSystem>();
        explosion.transform.position = transform.position;
        explosion.Play();
        SFX.PlayerDeath();
        
        Destroy(explosion.gameObject, 8);
        yield return new WaitForSeconds(3);
        SFX.ReviveSnapshot();
        BenShip.instance.gameObject.SetActive(true);
    }
}

using UnityEngine;
using System.Collections;

public class BenTestScript : BenColored {

    public static GameObject playerGO;
    public float suckingDistance = 1;
	// Use this for initialization
	void Start () {
        playerGO = gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
            ChangeColor(ObjectColor.Red);

        if (Input.GetKeyDown(KeyCode.G))
            ChangeColor(ObjectColor.Green);

        if (Input.GetKeyDown(KeyCode.B))
            ChangeColor(ObjectColor.Blue);

        if (Input.GetKeyDown(KeyCode.Y))
            ChangeColor(ObjectColor.Yellow);

        foreach (BenProjectile bp in BenProjectile.projectiles)
        {

        }
	}

    void ChangeColor(ObjectColor color)
    {
        if (objectColor != color)
        {
            GetComponent<Renderer>().material.color = BenColored.GetRGB(color);
            objectColor = color;
            gameObject.layer = LayerMask.NameToLayer(color.ToString());
        }
    }
}

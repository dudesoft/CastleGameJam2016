using UnityEngine;
using System.Collections;

public class BenTestScript : BenColored {

	// Use this for initialization
	void Start () {
	    
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

using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour 
{
    public static Timer instance;
    public TextMesh mesh1, mesh2;
    public bool counting = false;

	// Use this for initialization
	void Awake() 
    {
        instance = this;
        mesh1 = GetComponent<TextMesh>();
	}

    float time = 0;

    int hours, minutes, seconds, milliseconds;

	void Update () 
    {
        if (counting)
        {
            time += Time.deltaTime;

            int t = (int)(time * 1000);


            string s = TimeText(t, time);

            mesh1.text = s;
            mesh2.text = s;
        }

	}

    string TimeText(int ms, float s)
    {
        milliseconds = (int)((s - (int)s) * 100);
        seconds = (int)(ms / 1000) % 60;
        minutes = (int)((ms / (1000 * 60)) % 60);
        hours = (int)((ms / (1000 * 60 * 60)) % 24);

        return minutes + ":" + seconds + ":" + milliseconds;
    }
}

using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour 
{
    public static Timer instance;
    public TextMesh mesh1, mesh2;
    public bool counting = false;
    public static int Score = 0;
    public static int Deaths = 0;
    public static int Style = 0;

	// Use this for initialization
	void Awake() 
    {
        instance = this;
        mesh1 = GetComponent<TextMesh>();
	}

    void Start()
    {
        StartGame();
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

        return (hours > 0 ? (hours + ":") : "") + minutes + ":" + seconds + ":" + milliseconds;
    }

    public void StartGame()
    {
        time = 0;
        counting = true;
        ScoreScreen.instance = null;

        Score = 0;
        Style = 0;
        Deaths = 0;
    }

    public void Complete()
    {
        counting = false;
        //Score screen stuff here
        Application.LoadLevelAdditive(2);
        //load score screen
        ScoreScreen.instance.EnterScoreScreen(hours, minutes, seconds, milliseconds, (int)(time*1000), Deaths, Style);
    }
}

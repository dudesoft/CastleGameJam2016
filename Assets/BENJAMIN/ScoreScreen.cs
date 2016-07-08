using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScoreScreen : MonoBehaviour 
{
    public List<float> scoreLimits;
    public List<string> rewardName;

    public Text complete, time, timeScore, deaths, deathScore, gradeScore, style, styleScore, scoreText;
    public Image bg;
    public static ScoreScreen instance;

    int score;
    string grade;

	// Use this for initialization
	void Awake () 
    {
        instance = this;
        EnterScoreScreen(0, 3, 43, 74, 12494, 4, 350);
	}
	
	public void EnterScoreScreen(int hours, int minutes, int seconds, int ms, int totalms, int deaths, int styles)
    {
        score = (int)(30000 - ((totalms / 25f) + (deaths * 1000))) + (styles * 100);

        grade = GetGrade(score);

        StartCoroutine(ScoreMenuRoutine(hours, minutes, seconds, ms, totalms, deaths, styles));
    }

    IEnumerator ScoreMenuRoutine(int hours, int minutes, int seconds, int ms, int totalms, int deaths, int styles)
    {
        LeanTween.color(bg.GetComponent<RectTransform>(), Color.black, 1);

        yield return new WaitForSeconds(1.5f);

        timeScore.text = (hours > 0 ? (hours + ":") : "") + minutes + ":" + seconds + ":" + ms;

        yield return new WaitForSeconds(0.5f);

        deathScore.text = deaths + "";

        yield return new WaitForSeconds(0.5f);

        styleScore.text = styles + "";

        yield return new WaitForSeconds(1f);

        complete.text = "";
        time.text = "";
        this.deaths.text = "";
        style.text = "";

        timeScore.text = "";
        deathScore.text = "";
        styleScore.text = "";

        yield return new WaitForSeconds(0.5f);
        complete.text = "SCORE:";

        yield return new WaitForSeconds(0.5f);
        scoreText.text = score + "";

        yield return new WaitForSeconds(1f);
        gradeScore.text = grade;

        yield return new WaitForSeconds(10);

        Application.LoadLevel(0);
    }

    public string GetGrade(int score)
    {
        for (int i = 0; i < scoreLimits.Count; i++)
        {
            if (score < scoreLimits[i])
            {
                return rewardName[i];
            }
        }
        return rewardName[rewardName.Count - 1];
    }
}

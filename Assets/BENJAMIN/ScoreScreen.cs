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

    public AmplifyBloom.AmplifyBloomEffect bloom; 

    int score;
    string grade;

	// Use this for initialization
	void Awake () 
    {
        instance = this;
        bloomIntensity = bloom.OverallIntensity;
	}

    float bloomIntensity = 0.3f;
    public float bloomLowIntensity = 0.1f;
	
    void Update()
    {
        if (BeatManager.instance.beating)
        {
            bloom.OverallIntensity = bloomIntensity;
            LeanTween.value(bloom.gameObject, bloom.OverallIntensity, bloomLowIntensity, 0.2f).setOnUpdate(
            (float val) =>
            {
                bloom.OverallIntensity = val;
            });
        }
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
        BenShip.instance.gameObject.SetActive(false);
        while (!BeatManager.instance.canTransform)
            yield return null;

        timeScore.text = (hours > 0 ? (hours + ":") : "") + minutes + ":" + seconds + ":" + ms;
        SFX.Transform();
        yield return null;
        
        while (!BeatManager.instance.canTransform)
            yield return null;
        
        deathScore.text = deaths + "";
        SFX.Transform();
        yield return null;

        while (!BeatManager.instance.canTransform)
            yield return null;
        
        styleScore.text = styles + "";
        SFX.Transform();
        yield return null;

        while (!BeatManager.instance.canTransform)
            yield return null;

        complete.text = "";
        time.text = "";
        this.deaths.text = "";
        style.text = "";

        timeScore.text = "";
        deathScore.text = "";
        styleScore.text = "";
        yield return null;

        while (!BeatManager.instance.canTransform)
            yield return null;
        complete.text = "SCORE:";
        yield return null;

        while (!BeatManager.instance.canTransform)
            yield return null;
        scoreText.text = score + "";
        SFX.Transform();
        yield return null;

        while (!BeatManager.instance.canTransform)
            yield return null;
        gradeScore.text = grade;
        SFX.Transform();
        SFX.Respawn();

        yield return new WaitForSeconds(12);

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

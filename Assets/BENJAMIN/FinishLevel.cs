using UnityEngine;
using System.Collections;

public class FinishLevel : MonoBehaviour {

    bool finised = false;
    public Transform childCircle;

    void Start()
    {
        StartCoroutine(Animate());
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (!finised)
            {
                finised = true;
                Timer.instance.Complete();
            }
        }
    }

    IEnumerator Animate()
    {
        while (true)
        {
            while (!BeatManager.instance.beating)
            {
                yield return null;
            }
            if (BeatManager.instance.beat % 2 == 0)
                LeanTween.rotate(gameObject, transform.localEulerAngles + new Vector3(0, 0, Random.Range(45f, 90f)), BeatManager.instance.beatDuration * 0.75f).setEase(LeanTweenType.easeOutBack);
            else
                LeanTween.rotate(childCircle.gameObject, childCircle.localEulerAngles - new Vector3(0, 0, Random.Range(45f, 90f)), BeatManager.instance.beatDuration * 0.75f).setEase(LeanTweenType.easeOutBack);
            yield return null;
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeatManager : MonoBehaviour {

    public static BeatManager instance;
    public float bpm = 100;
    public float beatLength
    {
        get { return 60 / bpm; }
    }

    public float countdown = 0;

    public AudioSource[] tracks;
    public bool beating = false;
    public bool canTransform = false;
    

    void Awake()
    {
        instance = this;
    }

    public int beat = 0;
    float lastBeat = 0;

    public void TransformPrebeat()
    {

    }

    public void Beat()
    {
        lastBeat = Time.time;
        beat++;
        if (beat > 4)
            beat = 1;

        beating = true;
        StartCoroutine(UnBeat());

        if (beat == 3)
        {
            canTransform = true;
            StartCoroutine(UnTransform());
        }

        BenShip.instance.transform.localScale = Vector3.one * 1.1f;
        LeanTween.scale(BenShip.instance.gameObject, Vector3.one, 0.1f);
    }

    IEnumerator UnBeat()
    {
        yield return null;
        beating = false;
    }

    IEnumerator UnTransform()
    {
        yield return null;
        canTransform = false;
    }
}

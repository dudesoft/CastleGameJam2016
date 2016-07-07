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

    AmplifyBloom.AmplifyBloomEffect bloom;
    public float bloomLowIntensity = 0.3f;
    float bloomIntensity;


    void Awake()
    {
        bloom = Camera.main.GetComponent<AmplifyBloom.AmplifyBloomEffect>();
        bloomIntensity = bloom.OverallIntensity;
        bloom.OverallIntensity = 0;
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

        bloom.OverallIntensity = bloomIntensity;
        LeanTween.value(bloom.gameObject, bloom.OverallIntensity, bloomLowIntensity, 0.2f).setOnUpdate(
        (float val) =>
        {
            bloom.OverallIntensity = val;
        });
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

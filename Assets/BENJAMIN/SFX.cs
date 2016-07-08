using UnityEngine;
using System.Collections;

public class SFX : MonoBehaviour {

    private static SFX instance;

    public InAudioNode TransformShip, noAmmo, impactSFX, queueColor;

	// Use this for initialization
	void Awake () {
        instance = this;
	}
	
    public static void Transform()
    {
        InAudio.Play(instance.gameObject, instance.TransformShip);
    }

    public static void NoAmmo()
    {
        InAudio.Play(instance.gameObject, instance.noAmmo);
    }

    public static void QueueColor()
    {
        InAudio.Play(instance.gameObject, instance.queueColor);
    }

    public static void Play(InAudioNode node)
    {
        InAudio.Play(instance.gameObject, node);
    }
}

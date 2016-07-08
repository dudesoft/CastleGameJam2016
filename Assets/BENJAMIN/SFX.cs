using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class SFX : MonoBehaviour {

    private static SFX instance;

    public InAudioNode TransformShip, noAmmo, impactSFX, queueColor, playerDeath, respawn, enemyDeath;

    public AudioMixer effectedMixer;

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

    public static void PlayerDeath()
    {
        InAudio.Play(instance.gameObject, instance.playerDeath);
        instance.effectedMixer.FindSnapshot("Dead").TransitionTo(0);
    }

    public static void Respawn()
    {
        InAudio.Play(instance.gameObject, instance.respawn);
    }

    public static void ReviveSnapshot()
    {
        instance.effectedMixer.FindSnapshot("Alive").TransitionTo(1f);
    }

    public static void EnemyDeath()
    {
        InAudio.Play(instance.gameObject, instance.enemyDeath);
    }
}

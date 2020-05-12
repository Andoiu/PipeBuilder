using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSound : Singleton<PipeSound>
{
    // Pertaining to sound
    public AudioClip clickA;
    public AudioClip clickB;

    private AudioSource source;
    private float lowPitchRange = .75F;
    private float highPitchRange = 1.5F;
    //private float velToVol = .2F;
    //private float velocityClipSplit = 10F;

    void Awake() {
        source = gameObject.GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayPipeConnectSound(Transform sourceTransform) {
        source.pitch = Random.Range(lowPitchRange, highPitchRange);
        float vol = Random.Range(0.1f, 0.3f);
        source.PlayOneShot(Random.Range(0.0f, 1.0f) < 0.5 ? clickB : clickA, vol);
    }
}

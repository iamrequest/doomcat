using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip intro, loop;

    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(intro);
        audioSource.clip = loop;

        // https://gamedevbeginner.com/ultimate-guide-to-playscheduled-in-unity/#queue_clips
        double duration = (double)intro.samples / intro.frequency;
        audioSource.PlayScheduled(duration + AudioSettings.dspTime);
    }
}

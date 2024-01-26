using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneListener : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private float sensitivity = 100;
    [SerializeField]
    private float loudness = 0;
    [SerializeField]
    private float pitch = 0;

    [SerializeField]
    private float rmsValue;
    [SerializeField]
    private float dbValue;
    [SerializeField]
    private float pitchValue;

    [SerializeField]
    private bool startMicOnStartup = true;

    [SerializeField]
    private bool stopMicrophoneListener = false;
    [SerializeField]
    private bool startMicrophoneListener = false;

    [SerializeField]
    private bool microphoneListenerOn = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(null, false, 1000, 44100);
        while (!(Microphone.GetPosition(null) > 0)) ;
        audioSource.Play();
    }

    public void StopMicrophoneListener()
    {

    }

    void Update()
    {
        
    }
}

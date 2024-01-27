using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class MicrophoneListener : MonoBehaviour
{
    [SerializeField]
    private AudioMixer masterMixer;

    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private GameObject player;

    private float sensitivity = 100;

    [SerializeField]
    private float loudness = 0;

    [SerializeField]
    private float rimitTime = 0.2f;
    private float currTime = 0;

    [SerializeField]
    private float pitch = 0;


    private float rmsValue;
    private float dbValue;
    private float pitchValue;

    private const int QSamples = 1024;
    private const float refValue = 0.1f;
    private const float threshold = 0.02f;

    [SerializeField]
    private Vector3 maxScale = new Vector3(5, 5, 5);

    private Vector3 minScale = new Vector3(1, 1, 1);

    private Vector3 currScale;
    private Vector3 moveScale = new Vector3(1.0f, 1.0f, 1.0f);

    [SerializeField]
    private float scaleSpeed = 1.0f;

    float[] _samples;
    private float[] _spectrum;
    private float _fSample;

    private bool startMicOnStartup = true;

    private bool stopMicrophoneListener = false;
    private bool startMicrophoneListener = false;

    private bool microphoneListenerOn = false;

    private bool disableOutputSound = false;

    float timeSineRestart = 0;

    private void Start()
    {
        if (startMicOnStartup)
        {
            RestartMicrophoneListener();
            StartMicrophoneListener();

            audio = GetComponent<AudioSource>();
            audio.clip = Microphone.Start(null, true, 256, 44100);
            while (!(Microphone.GetPosition(null) > 0)) { };
            audio.Play();
            _samples = new float[QSamples];
            _spectrum = new float[QSamples];
            _fSample = AudioSettings.outputSampleRate;
            currScale = player.transform.localScale;
        }
    }

    private void FixedUpdate()
    {
        if (stopMicrophoneListener)
        {
            StopMicrophoneListener();
        }
        if (startMicrophoneListener)
        {
            StartMicrophoneListener();
        }

        stopMicrophoneListener = false;
        startMicrophoneListener = false;

        MicrophoneIntoAudioSource(microphoneListenerOn);

        DisableSound(!disableOutputSound);

        loudness = GetAveragedVolume() * sensitivity;

        //if(Mathf.Floor(loudness * 100) == Mathf.Floor(lastLoudness * 100)) {
        //    lastLoudness = loudness; 
        //}

        if (loudness <= 0.2f)
        {
            currTime += Time.deltaTime;
        }
        else
        {
            currTime = 0.0f;
        }

        

        //家府 目咙 > 农扁 累酒咙
        if (currTime < rimitTime)
        {
            if (player.transform.localScale.x > minScale.x)
            {
                player.transform.localScale -= new Vector3(1.0f, 1.0f, 1.0f) * scaleSpeed * Time.deltaTime;
                //currScale -= new Vector3(1.0f, 1.0f, 1.0f) * scaleSpeed * Time.deltaTime;
            }
        }
        else if (player.transform.localScale.x < maxScale.x)
        {
            player.transform.localScale += new Vector3(1.0f, 1.0f, 1.0f) * scaleSpeed * Time.deltaTime;
            //currScale += new Vector3(1.0f, 1.0f, 1.0f) * scaleSpeed * Time.deltaTime;
        }






        //if (player.transform.localScale.x > currScale.x && !(player.transform.localScale.x < minScale.x))
        //{
        //    player.transform.localScale -= (moveScale) * scaleSpeed * Time.deltaTime;
        //    //player.transform.localScale = Vector3.Lerp(minScale, maxScale, moveScale.x);
        //}
        //else if (player.transform.localScale.x < currScale.x && !(player.transform.localScale.x > maxScale.x))
        //{
        //    player.transform.localScale += (moveScale) * scaleSpeed * Time.deltaTime;
        //    //player.transform.localScale = Vector3.Lerp(minScale, maxScale, moveScale.x);
        //}

        //*scaleSpeed * Time.deltaTime

        //if (player.transform.localScale.x > minScale.x && player.transform.localScale.x < maxScale.x)
        //{
        //    player.transform.localScale += currScale;
        //}

        //GetPitch();
    }



    float GetAveragedVolume()
    {
        float[] data = new float[256];
        float a = 0;
        
        audio.GetOutputData(data, 0);
        foreach (float s in data)
        {
            a += Mathf.Abs(s);
        }
        return a / 256;
    }

    void GetPitch()
    {
        GetComponent<AudioSource>().GetOutputData(_samples, 0); // fill array with samples
        int i;
        float sum = 0;
        for (i = 0; i < QSamples; i++)
        {
            sum += _samples[i] * _samples[i]; // sum squared samples
        }
        rmsValue = Mathf.Sqrt(sum / QSamples);
        dbValue = 20 * Mathf.Sqrt(rmsValue / refValue);
        if (dbValue < -160) dbValue = -160;
        GetComponent<AudioSource>().GetSpectrumData(_spectrum, 0, FFTWindow.BlackmanHarris);
        float maxV = 0;
        var maxN = 0;
        for (i = 0; i < QSamples; i++)
        {
            if (!(_spectrum[i] > maxV) || !(_spectrum[i] > threshold))
                continue;
            maxV = _spectrum[i];
            maxN = i;
        }
        float freqN = maxN;
        if (maxN > 0 && maxN < QSamples - 1)
        {
            var dL = _spectrum[maxN - 1] / _spectrum[maxN];
            var dR = _spectrum[maxN + 1] / _spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }
        pitchValue = freqN * (_fSample / 2) / QSamples;

    }

    public void StopMicrophoneListener()
    {
        microphoneListenerOn = false;
        disableOutputSound = false;
        audio.Stop();
        audio.clip = null;

        Microphone.End(null);
    }

    public void StartMicrophoneListener()
    {
        microphoneListenerOn = true;
        disableOutputSound = true;
        RestartMicrophoneListener();
    }

    public void DisableSound(bool SoundOn)
    {
        float volume = 0;

        if (SoundOn)
        {
            volume = 0.0f;
        }
        else
        {
            volume = -80.0f;
        }
        masterMixer.SetFloat("MasterVolume", volume);
    }

    public void RestartMicrophoneListener()
    {
        audio = GetComponent<AudioSource>();
        audio.clip = null;

        timeSineRestart = Time.time;
    }

    void MicrophoneIntoAudioSource(bool MicrophoneListenerOn)
    {
        if (MicrophoneListenerOn)
        {
            if (Time.time - timeSineRestart > 0.5f && !Microphone.IsRecording(null))
            {
                audio.clip = Microphone.Start(null, false, 256, 44100);

                while (!(Microphone.GetPosition(null) > 0))
                {

                }
                audio.Play();
            }
        }
    }
}

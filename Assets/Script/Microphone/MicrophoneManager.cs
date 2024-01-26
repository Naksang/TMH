using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MicrophoneManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixer masterMixer;

    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private Rigidbody2D playerRig;

    [SerializeField]
    private float sensitivity = 100;

    //public float loudness
    //{
    //    get { return loudness; }
    //    set
    //    {
    //        loudness = value;
    //    }
    //}
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

    private const int QSamples = 1024;
    private const float refValue = 0.1f;
    private const float threshold = 0.02f;

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
            audio.clip = Microphone.Start(null, false, 1000, 44100);
            while (!(Microphone.GetPosition(null) > 0)) { };
            audio.Play();
            _samples = new float[QSamples];
            _spectrum = new float[QSamples];
            _fSample = AudioSettings.outputSampleRate;
        }
    }

    void Update()
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

        if (loudness > 0 && loudness <= 100f)
        {
            //playerRig.AddForce(Vector2.up *  loudness, ForceMode2D.Impulse);
            playerRig.AddForce(Vector2.up * 1.0f, ForceMode2D.Impulse);
        }
        else if (loudness > 100f && loudness <= 200f)
        {
            playerRig.AddForce(Vector2.up * 2.0f, ForceMode2D.Impulse);
        }
        else if (loudness > 200f && loudness <= 300f)
        {
            playerRig.AddForce(Vector2.up * 3.0f, ForceMode2D.Impulse);
        }
        else if (loudness > 300f && loudness <= 400f)
        {
            playerRig.AddForce(Vector2.up * 4.0f, ForceMode2D.Impulse);
        }



        GetPitch();
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
        GetComponent<AudioSource>().GetOutputData(_samples, 0);
        int i;
        float sum = 0;
        for (i = 0; i < QSamples; i++)
        {
            sum += _samples[i] * _samples[i];
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
                audio.clip = Microphone.Start(null, false, 10, 44100);

                while (!(Microphone.GetPosition(null) > 0))
                {

                }
                audio.Play();
            }
        }
    }
}

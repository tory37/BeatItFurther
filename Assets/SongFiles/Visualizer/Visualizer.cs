using UnityEngine;
using System.Collections;

public class Visualizer : MonoBehaviour {

    AudioSource audio;
    float[] spectrum = new float[64];

    [SerializeField]
    GameObject[] bumpers;   //change to a list?
    [SerializeField]
    GameObject bumperPrefab;

    float[] output = new float[64];

    [Range(0, 32.5f)]
    [SerializeField]
    float sampleRate = 4.4f;
    float nextActionTime = 0.0f;

    private bool playNow;

    public static Visualizer Instance
    {
        get
        {
            return instance;
        }
        set
        {
            if (instance == null)
                instance = value;
            else
                Destroy(value.gameObject);
        }
    }

    private static Visualizer instance;


    // Use this for initialization
    void Awake()
    {
        Instance = this;
        audio = GetComponent<AudioSource>();
        playNow = false;
    }

    public void SetTempo(int tempo)
    {
        sampleRate = (float)(tempo * (2f / 15f));
        //Debug.Log("set temp " + tempo + " sample rate " + sampleRate);
        playNow = true;
    }


    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Time: " + Time.time + " nextActionTime: " + nextActionTime);
        if (Time.time > nextActionTime)
        {
            //Debug.Log("Playing");
            //  audio.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
            if (playNow)
            {
                audio.GetOutputData(spectrum, 0);

                output = FFT(spectrum, 64);

                for (int i = 0; i < 32; i++)
                {
                    if (bumpers[i].transform.localScale.y < 15 * spectrum[i] || Mathf.Abs(spectrum[i]) <= 0.5f)
                    {
                        bumpers[i].transform.localScale = new Vector3(0.5f, 15 * (spectrum[i * 2] + spectrum[i * 2 + 1]) / 2, .1f);
                    }
                    else
                    {
                        bumpers[i].transform.localScale += new Vector3(0f, -.2f, 0f);
                    }
                }
            }
            nextActionTime = Time.time + (1.0f / sampleRate) - (Time.time - nextActionTime);
        }
    }

    public float[] FFT(float[] input, int N)
    {
        float[] output = new float[N];

        for (int k = 0; k < N; k++)
        {
            float sumReal = 0.0f;
            float sumImg = 0.0f;

            for (int n = 0; n < N; n++)
            {
                float angle = ((2 * Mathf.PI * k * n) / N);
                sumReal = input[n] * Mathf.Cos(angle) * 0.48829f + (0.14128f * Mathf.Cos(2 * angle)) - (0.01168f * Mathf.Cos(3 * angle));
                sumImg = input[n] * Mathf.Sin(angle) * 0.48829f + (0.14128f * Mathf.Sin(2 * angle)) - (0.01168f * Mathf.Sin(3 * angle));
            }

            output[k] = sumReal - sumImg;
        }

        return output;
    }
}

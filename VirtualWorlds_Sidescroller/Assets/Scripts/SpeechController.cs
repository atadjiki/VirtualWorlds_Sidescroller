using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechController : MonoBehaviour
{

    private TextMeshPro speechText;
    private string currentSpeech;
    private int speechIndex;
    private bool animate = false;

    [SerializeField]
    private float changeEverySeconds = 0.25f;
    private float currentTime = 0;

    [SerializeField]
    private string initialSpeech = "";

    private void Awake()
    {
        speechText = GetComponent<TextMeshPro>();
        SetText(initialSpeech, true);
    }

    // Update is called once per frame
    void Update()
    {
        if(animate)
        {
            if (speechIndex < currentSpeech.Length && currentTime >= changeEverySeconds)
            {
                speechText.text = currentSpeech.Substring(0, speechIndex);
                speechIndex++;
            }
            else if (speechIndex >= currentSpeech.Length)
            {
                speechIndex = 0;
            }

            if (currentTime < changeEverySeconds)
            {
                currentTime += Time.deltaTime;
            }
            else if (currentTime >= changeEverySeconds)
            {
                currentTime = 0;
            }
        }
        

    }

    public void SetText(string text, bool DoAnimate)
    {
        currentSpeech = text;
        speechIndex = 0;

        if(DoAnimate)
        {
            animate = true;
            speechText.text = currentSpeech.Substring(0, speechIndex);
        }
        else
        {
            animate = false;
            speechText.text = currentSpeech;
        }
        
    }
}

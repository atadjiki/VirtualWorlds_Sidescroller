using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance { get { return _instance; } }

    public TextMeshPro TitleText;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Build();
    }

    private void Build()
    {

    }

    public void HideTitleText()
    {
        StartCoroutine(FadeTitleText());
    }

    internal IEnumerator FadeTitleText()
    {
        float max_secs = 100f;
        float current = 0;

        while(current < max_secs)
        {
            TitleText.color = Color.Lerp(TitleText.color, Color.clear, current / max_secs);
            current += Time.fixedDeltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}

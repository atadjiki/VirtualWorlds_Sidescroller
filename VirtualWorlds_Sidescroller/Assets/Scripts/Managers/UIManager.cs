using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance { get { return _instance; } }

    public TextMeshPro TitleText;
    public GameObject GameOver;
    public GameObject Success;

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
        float max_secs = 5000f;
        float current = 0;

        while(current < max_secs)
        {
            TitleText.color = Color.Lerp(TitleText.color, Color.clear, current / max_secs);
            current += Time.fixedDeltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public void ToggleGameOverText(bool flag)
    {
        GameOver.SetActive(flag);
    }

    public void ToggleTitle(bool flag)
    {
        TitleText.gameObject.SetActive(flag);
    }

    public void ToggleSuccess(bool flag)
    {
        Success.gameObject.SetActive(flag);
    }
}

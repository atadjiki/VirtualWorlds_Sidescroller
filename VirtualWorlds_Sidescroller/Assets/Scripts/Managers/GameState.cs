using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    private static GameState _instance;

    public static GameState Instance { get { return _instance; } }

    private bool waiting = false;

    public bool skip_start = true;

    public enum State { Start, InGame, Paused, End };
    public State currentState;

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

        UIManager.Instance.ToggleGameOverText(false);

        if (skip_start)
        {
            currentState = State.InGame;
            UIManager.Instance.ToggleTitle(false);
            CameraRig.Instance.SwitchTo(CameraRig.CameraType.Main);
        }
        else
        {
            currentState = State.Start;
            UIManager.Instance.ToggleTitle(true);
            CameraRig.Instance.SwitchTo(CameraRig.CameraType.Start);
        }
    }

    private void Update()
    {
        if(currentState == State.Start && Input.anyKeyDown && waiting == false)
        {
            StartCoroutine(StartGame());
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    IEnumerator StartGame()
    {
        waiting = true;
        PostProcessingEffects.Instance.ChromaticAbberation();
        UIManager.Instance.HideTitleText();
        CameraRig.Instance.SwitchTo(CameraRig.CameraType.Player_Front);
        yield return new WaitForSeconds(4f);
        CameraRig.Instance.SwitchTo(CameraRig.CameraType.Main);
        yield return new WaitForSeconds(1f);
        currentState = State.InGame;
        waiting = false;
    }

    public void Spotted()
    {
        PlayerController.Instance.Reset();
        CameraRig.Instance.SwitchTo(CameraRig.CameraType.Player_Front);
        UIManager.Instance.ToggleGameOverText(true);
        GameState.Instance.currentState = GameState.State.End;
        StartCoroutine(DelayFailed());
    }

    public void FellToDeath()
    {
        PlayerController.Instance.Reset();
        UIManager.Instance.ToggleGameOverText(true);
        CameraRig.Instance.SwitchTo(CameraRig.CameraType.Start);
        GameState.Instance.currentState = GameState.State.End;
        StartCoroutine(DelayFailed());
    }

    IEnumerator DelayFailed()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    private static GameState _instance;

    public static GameState Instance { get { return _instance; } }

    private bool waiting = false;

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
        currentState = State.Start;
        CameraRig.Instance.SwitchTo(CameraRig.CameraType.Start);
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
        CameraRig.Instance.SwitchTo(CameraRig.CameraType.Main);
        yield return new WaitForSeconds(0.25f);
        currentState = State.InGame;
        waiting = false;
    }

    public void Failed()
    {
        PlayerController.Instance.Reset();
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

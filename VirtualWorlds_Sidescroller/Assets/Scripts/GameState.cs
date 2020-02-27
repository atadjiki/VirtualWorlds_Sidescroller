using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    IEnumerator StartGame()
    {
        waiting = true;
        CameraRig.Instance.SwitchTo(CameraRig.CameraType.Main);
        yield return new WaitForSeconds(0.25f);
        currentState = State.InGame;
        waiting = false;
    }

}

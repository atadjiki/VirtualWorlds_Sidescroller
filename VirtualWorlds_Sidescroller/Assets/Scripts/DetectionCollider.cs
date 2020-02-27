using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DetectionCollider : MonoBehaviour
{

    private Collider dc;

    private float timeDetected;

    [SerializeField]
    private float timeUntilDetected = 3;

    private bool Detecting;

    private bool DetectedPlayer;

    private void Awake()
    {
        dc = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Detecting && DetectedPlayer == false)
        {
            if (PlayerController.Instance.Running() && PlayerController.Instance.Crouching() == false)
            {
                timeDetected += Time.deltaTime;

                if (timeDetected > timeUntilDetected)
                {
                    DetectedPlayer = true;
                    Debug.Log("Detected Player!");
                    GameState.Instance.Failed();
                    this.transform.parent.GetComponentInChildren<SpeechController>().SetText("Got you!", false);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerDetectionCollider>())
        {
            Detecting = true;
            timeDetected = 0;
            Debug.Log("Beginning detection");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerDetectionCollider>())
        {
            Detecting = false;
            timeDetected = 0;
            Debug.Log("Ending detection");
        }
    }
}

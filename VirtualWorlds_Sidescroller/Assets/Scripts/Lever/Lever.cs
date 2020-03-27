using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Material Mat_On;
    public Material Mat_Off;

    float offAngle = -25;
    float onAngle = 25;

    public GameObject pivot;
    public Renderer lever;

    public enum State { On, Off};
    public State currentState;

    public bool OneTime;
    private int interactions = 0;

    public LeverBehavior attachedBehavior;
    public GameObject highlightSphere;

    private bool isStaying = false;
    private bool canBeInteracted = false;

    // Start is called before the first frame update
    void Start()
    {
        highlightSphere.SetActive(false);
        SetToState(currentState);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canBeInteracted)
        {
            if (interactions > 1 && OneTime)
            {
                Debug.Log("Already interacted with lever");
            }
            else
            {
                PlayerController.Instance.Interact(this);
            }
        }
    }

    void SetToState(State state)
    {
        if(state == State.On)
        {
            lever.material = Mat_On;
            currentState = State.On;
            pivot.transform.eulerAngles = new Vector3(0, 0, onAngle);
        }
        else
        {
            lever.material = Mat_Off;
            currentState = State.Off;
            pivot.transform.eulerAngles = new Vector3(0, 0, offAngle);

        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        if(other != null && other.GetComponent<InteractCollider>() 
            && PlayerController.Instance.Jumping() == false)
        {
            if(isStaying == false)
            {
                highlightSphere.SetActive(true);
                AudioManager.Instance.PlaySFX(AudioManager.SFX.lever_interact);
                isStaying = true;
            }

            canBeInteracted = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isStaying = false;
        canBeInteracted = false;
        highlightSphere.SetActive(false);
    }

    public void LeverCallback()
    {
        if (currentState == State.On)
        {
            SetToState(State.Off);
            attachedBehavior.Off_Behavior();
        }
        else
        {
            SetToState(State.On);
            attachedBehavior.On_Behavior();
        }

        Debug.Log("Interacted with lever");
        interactions++;
    }
}

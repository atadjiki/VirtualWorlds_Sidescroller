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

    // Start is called before the first frame update
    void Start()
    {
        SetToState(currentState);
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
        if(other.transform.parent.GetComponent<PlayerController>() 
            && other.transform.parent.GetComponent<PlayerController>().Jumping() == false && Input.GetKeyDown(KeyCode.F))
        {
            if(interactions > 1 && OneTime)
            {
                Debug.Log("Already interacted with lever");
            }
            else
            {

                other.transform.parent.GetComponent<PlayerController>().Interact(this);

                
            }
            
        }
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

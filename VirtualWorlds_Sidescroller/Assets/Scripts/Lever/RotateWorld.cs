using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWorld : LeverBehavior
{

    private bool active = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    public override void On_Behavior()
    {
        if (active == false)
        {
            active = true;
            base.On_Behavior();

            PlayerController.Instance.rotated = true;
            PlayerController.Instance.transform.eulerAngles = new Vector3(0, 90, 0);
            PlayerController.Instance.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
            CameraRig.Instance.SwitchTo(CameraRig.CameraType.Rotated);
            StartCoroutine(Delay());
        }
    }

    public override void Off_Behavior()
    {
        if(active == false)
        {
            active = true;
            base.Off_Behavior();

            PlayerController.Instance.rotated = false;
            PlayerController.Instance.transform.eulerAngles = new Vector3(0, 0, 0);
            PlayerController.Instance.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            CameraRig.Instance.SwitchTo(CameraRig.CameraType.Main);
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
        active = false;
    }

}

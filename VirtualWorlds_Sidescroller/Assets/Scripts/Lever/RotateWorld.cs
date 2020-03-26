using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWorld : LeverBehavior
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public override void On_Behavior()
    {
        base.On_Behavior();

        PlayerController.Instance.rotated = true;
        PlayerController.Instance.transform.eulerAngles = new Vector3(0, 90, 0);
        PlayerController.Instance.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
        CameraRig.Instance.SwitchTo(CameraRig.CameraType.Rotated);

    }

    public override void Off_Behavior()
    {
        base.Off_Behavior();

        PlayerController.Instance.rotated = false;
        PlayerController.Instance.transform.eulerAngles = new Vector3(0, 0, 0);
        PlayerController.Instance.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        CameraRig.Instance.SwitchTo(CameraRig.CameraType.Main);
    }

}

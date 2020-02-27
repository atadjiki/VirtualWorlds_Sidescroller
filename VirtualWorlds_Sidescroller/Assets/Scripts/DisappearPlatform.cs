using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearPlatform : LeverBehavior
{
    public GameObject platform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void On_Behavior()
    {
        base.On_Behavior();

        platform.GetComponent<Rigidbody>().isKinematic = false;
    }
}

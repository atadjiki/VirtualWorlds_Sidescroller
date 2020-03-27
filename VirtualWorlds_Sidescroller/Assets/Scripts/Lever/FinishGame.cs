using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : LeverBehavior
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public override void On_Behavior()
    {
        base.On_Behavior();

        GameState.Instance.FinishGame();
  
    }

}

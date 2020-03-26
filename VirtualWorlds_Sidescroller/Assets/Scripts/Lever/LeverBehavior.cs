using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverBehavior : MonoBehaviour
{ 
    public virtual void On_Behavior() 
    {
        AudioManager.Instance.PlaySFX(AudioManager.SFX.player_lever);
        PostProcessingEffects.Instance.ChromaticAbberation();
    }
    public virtual void Off_Behavior() 
    {
        AudioManager.Instance.PlaySFX(AudioManager.SFX.player_lever);
        PostProcessingEffects.Instance.ChromaticAbberation();
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraRig : MonoBehaviour
{
    private static CameraRig _instance;

    public static CameraRig Instance { get { return _instance; } }

    public CinemachineVirtualCamera CM_Main;
    public CinemachineVirtualCamera CM_Rotated;
    public CinemachineVirtualCamera CM_Start;
    public CinemachineVirtualCamera CM_Start_2;
    public CinemachineVirtualCamera CM_Player_Front;

    public enum CameraType { Main, Rotated, Start, Start_2, Player_Front };

    private List<CinemachineVirtualCamera> cameras;

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
        cameras = new List<CinemachineVirtualCamera>() { CM_Main, CM_Rotated, CM_Start, CM_Start_2, CM_Player_Front };
    }

    public void SwitchTo(CameraType camera)
    {
        SetAll(false);

        if(camera == CameraType.Main)
        {
            CM_Main.enabled = true;
        }
        else if(camera == CameraType.Start)
        {
            CM_Start.enabled = true;
        }
        else if(camera == CameraType.Player_Front)
        {
            CM_Player_Front.enabled = true;
        }
        else if(camera == CameraType.Rotated)
        {
            CM_Rotated.enabled = true;
        }
        else if(camera == CameraType.Start_2)
        {
            CM_Start_2.enabled = true;
        }
    }

    private void SetAll(bool flag)
    {
        foreach(CinemachineVirtualCamera cm in cameras)
        {
            cm.enabled = flag;
        }
    }
}

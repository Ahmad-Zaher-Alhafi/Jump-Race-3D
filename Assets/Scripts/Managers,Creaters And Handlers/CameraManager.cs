using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    private CinemachineVirtualCamera mainVCam;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<CameraManager>();
        }

        mainVCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void OnPrepareNewRace()
    {
        mainVCam.Priority = 20;
    }

    public void OnRaceFinish()
    {
        mainVCam.Priority = 0;
    }
}

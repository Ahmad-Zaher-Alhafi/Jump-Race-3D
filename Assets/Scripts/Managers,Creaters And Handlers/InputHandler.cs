using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;

    private bool hasGameStarted;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<InputHandler>();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == null)
        {
            if (!hasGameStarted)
            {
                hasGameStarted = true;
                EventsManager.OnNewLevelStart();
            }

            if (!GameManager.Instance.HasRaceFinished)
            {
                EventsManager.OnLeftMouseInput(true);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            EventsManager.OnLeftMouseInput(false);
        }
    }

    public void OnPrepareNewRace()
    {
        hasGameStarted = false;
        EventsManager.OnLeftMouseInput(false);
    }

    public void OnRaceFinish()
    {
        EventsManager.OnLeftMouseInput(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventsManager
{
    public static event Action<bool> onLeftMouseInput;
    public static event Action onNewRaceStart;
    public static event Action onPrepareNewRace;


    public static void OnLeftMouseInput(bool isItClickDown)
    {
        onLeftMouseInput?.Invoke(isItClickDown);
    }

    public static void OnNewLevelStart()
    {
        onNewRaceStart?.Invoke();
    }

    public static void OnPrepareNewRace()
    {
        onPrepareNewRace?.Invoke();
    }
}
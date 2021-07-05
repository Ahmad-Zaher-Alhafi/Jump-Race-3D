using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventsManager
{
    public static event Action<bool> onLeftMouseInput;
    public static event Action onPlayerDeath;
    public static event Action onNewRaceStart;

    public static void OnLeftMouseInput(bool isItClickDown)
    {
        onLeftMouseInput?.Invoke(isItClickDown);
    }

    public static void OnPlayerDeath()
    {
        onPlayerDeath?.Invoke();
    }

    public static void OnNewLevelStart()
    {
        onNewRaceStart?.Invoke();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventsManager
{
    public static event Action<bool> onLeftMouseInput;
    public static event Action onPlayerDeath;


    public static void OnLeftMouseInput(bool isItClickDown)
    {
        onLeftMouseInput?.Invoke(isItClickDown);
    }

    public static void OnPlayerDeath()
    {
        onPlayerDeath?.Invoke();
    }
}

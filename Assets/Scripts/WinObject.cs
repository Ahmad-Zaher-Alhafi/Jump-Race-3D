using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinObject : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        gameManager.FinishRace(true);
    }
}

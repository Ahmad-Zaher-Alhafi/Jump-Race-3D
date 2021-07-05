using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    private void Awake()
    {
        PlayerPrefs.DeleteAll();

        if (PlayerPrefs.HasKey(Constances.levelNumKey))
        {
            gameManager.CurrentLevelNum = PlayerPrefs.GetInt(Constances.levelNumKey);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            PlayerPrefs.SetInt(Constances.levelNumKey, gameManager.CurrentLevelNum);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(Constances.levelNumKey, gameManager.CurrentLevelNum);
    }
}

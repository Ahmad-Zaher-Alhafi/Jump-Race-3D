using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    [Header("For testing only")]
    [SerializeField] private bool hasToStartFromFirstLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<DataManager>();
        }

        if (hasToStartFromFirstLevel)
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public int GetCurrentLevelNum() 
    {
        if (PlayerPrefs.HasKey(Constances.levelNumKey))
        {
            return PlayerPrefs.GetInt(Constances.levelNumKey);
        }
        else
        {
            return 0;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            PlayerPrefs.SetInt(Constances.levelNumKey, GameManager.Instance.CurrentLevelNum);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(Constances.levelNumKey, GameManager.Instance.CurrentLevelNum);
    }
}

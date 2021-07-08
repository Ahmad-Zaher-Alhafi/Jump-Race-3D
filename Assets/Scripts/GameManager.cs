using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool hasRaceFinished;
    public bool HasRaceFinished => hasRaceFinished;

    private int currentLevelNum;
    public int CurrentLevelNum
    {
        get => currentLevelNum;
        set => currentLevelNum = value;
    }

    private Dictionary<string, float> racersRanks = new Dictionary<string, float>();

    private bool isThereMoreLevels
    {
        get
        {
            return currentLevelNum + 1 < JumpObjectsCreater.Instance.GetPathsNumber();
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<GameManager>();
        }

        currentLevelNum = DataManager.Instance.GetCurrentLevelNum();
    }

    #region Races Managing
    public void PrepareNewRace()
    {
        EventsManager.OnPrepareNewRace();
        JumpObjectsCreater.Instance.OnPrepareNewRace(currentLevelNum);
        RacersCreater.Instance.OnPrepareNewRace();
        Player.Instance.OnPrepareNewRace(JumpObjectsCreater.Instance.JumpObjects[0]);
        MainCanves.Instance.OnPrepareNewRace();
        InputHandler.Instance.OnPrepareNewRace();

        racersRanks.Clear();
    }

    private void StartNewRace()
    {
        hasRaceFinished = false;
        Player.Instance.OnRaceStart();

        for (int i = 0; i < RacersCreater.Instance.racers.Count; i++)
        {
            RacersCreater.Instance.racers[i].OnRaceStart();
        }
    }

    public void FinishRace(bool hasWon)
    {
        if (hasRaceFinished)
        {
            return;
        }

        hasRaceFinished = true;

        if (hasWon && isThereMoreLevels)
        {
            currentLevelNum++;
        }
        
        SetPlayerRank(hasWon);
        SetRacersRank();

        racersRanks = racersRanks.OrderByDescending(t => t.Value).ToDictionary(k => k.Key, k=>k.Value);

        MainCanves.Instance.OnRaceFinish(racersRanks);
    }

    public void StartNextLevel()
    {
        StartNewRace();
    }
    #endregion Races Managing

    #region Ranks Setting
    private void SetPlayerRank(bool hasWon)
    {
        if (hasWon)
        {
            racersRanks.Add("You", Player.Instance.Rank);
        }
        else
        {
            if (Player.Instance.IsDead)
            {
                racersRanks.Add("You", 0);
            }
            else
            {
                racersRanks.Add("You", Player.Instance.GetCurrentJumpObject().JumpObjectIndex);
            }
        }
    }

    private void SetRacersRank()
    {
        foreach (Racer racer in RacersCreater.Instance.racers)
        {
            if (racer.HasFinishedTheRace)
            {
                racersRanks.Add(racer.RacerName, racer.Rank);
            }
            else
            {
                if (!racersRanks.ContainsKey(racer.RacerName))
                {
                    if (racer.IsDead)
                    {
                        racersRanks.Add(racer.RacerName, 0);
                    }
                    else
                    {
                        racersRanks.Add(racer.RacerName, racer.GetCurrentJumpObject().JumpObjectIndex);
                    }
                }
                else
                {
                    Debug.LogError("This racer name was used more than one time which leads to error, Make sure that all the racers has uniq names");
                }
            }
        }
    }
    #endregion Ranks Setting
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private RacersCreater racersCreater;
    [SerializeField] private JumpObjectsCreater jumpObjectsCreater;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private MainCanves mainCanves;

    private bool hasRaceFinished;
    public bool HasRaceFinished => hasRaceFinished;
    private int currentLevelNum;
    public int CurrentLevelNum
    {
        get => currentLevelNum;
        set => currentLevelNum = value;
    }

    private void StartNewRace()
    {
        hasRaceFinished = false;
        player.OnRaceStart();
        for (int i = 0; i < racersCreater.racers.Count; i++)
        {
            racersCreater.racers[i].OnRaceStart();
        }
    }

    public void PrepareNewRace()
    {
        player.OnPrepareNewRace(jumpObjectsCreater.jumpObjects[0]);
        
        for (int i = 0; i < racersCreater.racers.Count; i++)
        {
            racersCreater.racers[i].OnPrepareNewRace(jumpObjectsCreater.jumpObjects[i + 1]);
        }

        racersCreater.OnPrepareNewRace();
        mainCanves.OnPrepareNewRace();
        inputHandler.OnPrepareNewRace();
    }

    public void FinishRace(bool hasWon)
    {
        if (hasRaceFinished)
        {
            return;
        }

        if (hasWon)
        {
            currentLevelNum++;
        }
        
        hasRaceFinished = true;

        Dictionary<string, float> racersRanks = new Dictionary<string, float>();

        if (player.HasWonTheRace)
        {
            racersRanks.Add("You", player.PlayerRank);
        }
        else
        {
            racersRanks.Add("You", player.GetCurrentJumpObject().JumpObjectIndex);
        }

        foreach (Racer racer in racersCreater.racers)
        {
            if (racer.HasFinishedTheRace)
            {
                racersRanks.Add(racer.RacerName, racer.RacerRank);
            }
            else
            {
                if (!racersRanks.ContainsKey(racer.RacerName))
                {
                    racersRanks.Add(racer.RacerName, racer.GetCurrentJumpObject().JumpObjectIndex);
                }
                else
                {
                    Debug.LogError("This racer name was used more than one time which leads to error, Make sure that all the racers has uniq names");
                }
            }
        }

        racersRanks = racersRanks.OrderByDescending(t => t.Value).ToDictionary(k => k.Key, k=>k.Value);
        mainCanves.OnRaceFinish(racersRanks);
    }

    public void StartNextLevel()
    {
        StartNewRace();
    }
}

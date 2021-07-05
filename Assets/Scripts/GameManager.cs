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
        player.OnPrepareNewRace();
        player.transform.position = jumpObjectsCreater.jumpObjects[0].transform.position + Vector3.up * 5;

        for (int i = 0; i < racersCreater.racers.Count; i++)
        {
            racersCreater.racers[i].OnPrepareNewRace();
            racersCreater.racers[i].transform.position = jumpObjectsCreater.jumpObjects[i + 1].transform.position + Vector3.up * 5;
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
        racersCreater.racers = racersCreater.racers.OrderByDescending(t => t.GetCurrentJumpObject().JumpObjectIndex).ToList();
        mainCanves.OnRaceFinish(racersCreater.racers);
    }

    public void StartNextLevel()
    {
        StartNewRace();
    }
}

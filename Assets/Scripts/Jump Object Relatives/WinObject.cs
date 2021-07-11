using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinObject : MonoBehaviour
{
    private const int highestRank = 1000;
    private bool hasSomeoneWonTheRace;
    private int counter = highestRank;

    private void Awake()
    {
        EventsManager.onPrepareNewRace += OnPrepareNewRace;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Constances.PlayerLayerNum)
        {
            Player.Instance.Rank = counter;

            if (!hasSomeoneWonTheRace)
            {
                hasSomeoneWonTheRace = true;
                Player.Instance.OnFinishTheRace(true);
                GameManager.Instance.FinishRace(true);
            }
            else
            {
                Player.Instance.OnFinishTheRace(false);
                GameManager.Instance.FinishRace(false);
            }

            hasSomeoneWonTheRace = true;
            
            counter--;
        }
        else if (other.gameObject.layer == Constances.RacerLayerNum)
        {
            Racer racer =  other.GetComponent<Racer>();
            racer.HasFinishedTheRace = true;
            racer.Rank = counter;

            if (!hasSomeoneWonTheRace)
            {
                hasSomeoneWonTheRace = true;
                racer.OnFinishTheRace(true);
            }
            else
            {
                racer.OnFinishTheRace(false);
            }
            
            counter--;
        }
    }

    public void OnPrepareNewRace()
    {
        hasSomeoneWonTheRace = false;
    }

    private void OnApplicationQuit()
    {
        EventsManager.onPrepareNewRace -= OnPrepareNewRace;
    }
}
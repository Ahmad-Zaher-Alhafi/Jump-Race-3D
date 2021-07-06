using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinObject : MonoBehaviour
{
    [HideInInspector] public GameManager GameManager;

    private const int highestRank = 1000;
    private bool hasSomeoneWonTheRace;
    private int counter = highestRank;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Constances.PlayerLayerNum)
        {
            Player player =  other.GetComponent<Player>();
            player.PlayerRank = counter;

            if (!hasSomeoneWonTheRace)
            {
                hasSomeoneWonTheRace = true;
                player.OnFinishTheRace(true);
                GameManager.FinishRace(true);
            }
            else
            {
                player.OnFinishTheRace(false);
                GameManager.FinishRace(false);
            }

            hasSomeoneWonTheRace = true;
            other.transform.position = transform.position;
            counter--;
        }
        else if (other.gameObject.layer == Constances.RacerLayerNum)
        {
            Racer racer =  other.GetComponent<Racer>();
            racer.HasFinishedTheRace = true;
            racer.RacerRank = counter;

            if (!hasSomeoneWonTheRace)
            {
                hasSomeoneWonTheRace = true;
                racer.OnFinishTheRace(true);
            }
            else
            {
                racer.OnFinishTheRace(false);
            }

            
            other.transform.position = transform.position;
            counter--;
        }
    }

    public void OnPrepareNextLevel()
    {
        hasSomeoneWonTheRace = false;
    }
}
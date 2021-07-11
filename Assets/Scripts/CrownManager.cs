using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrownManager : MonoBehaviour
{
    [SerializeField] private float crownMovementSmoothSpeed;
    [SerializeField] private float crownRotatingSpeed;
    private List<Racer> racers = new List<Racer>();
    private Transform racerWithHeighstRank;

    void Start()
    {
        EventsManager.onRacerUpdateRank += UpdateCrownPos;
        Invoke("GetRacers", .1f);
    }

    private void FixedUpdate()
    {
        if (racerWithHeighstRank == null)
        {
            return;
        }

        transform.Rotate(Vector3.forward * crownRotatingSpeed);
        transform.position = Vector3.Lerp(transform.position, racerWithHeighstRank.position + Vector3.up * 1.7f, crownMovementSmoothSpeed);
    }

    private void UpdateCrownPos()
    {
        racers = racers.OrderByDescending(t => t.Rank).ToList();

        if (racers[0].Rank < Player.Instance.Rank)
        {
             
            racerWithHeighstRank = Player.Instance.transform;
        }
        else
        {
            racerWithHeighstRank = racers[0].transform;
        }

        transform.parent = racerWithHeighstRank;
    }

    private void GetRacers()
    {
        foreach (Racer racer in RacersCreater.Instance.racers)
        {
            racers.Add(racer);
        }
    }

    private void OnDestroy()
    {
        EventsManager.onRacerUpdateRank -= UpdateCrownPos;
    }
}

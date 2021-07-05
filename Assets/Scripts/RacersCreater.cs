using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacersCreater : MonoBehaviour
{
    [HideInInspector] public List<Racer> racers = new List<Racer>();

    [SerializeField] private Transform racersParent;
    [SerializeField] private JumpObjectsCreater jumpObjectsCreater;
    [SerializeField] private Racer racerPrefab;
    [Tooltip("The offset between the jump object and the racer that is gonna be created above it")]
    [SerializeField] private Vector3 racerCreatPosOffset;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private List<RacerInfo> racerInfos;

    void Start()
    {
        for (int i = 0; i < racerInfos.Count; i++)
        {
            JumpObject jumpObject = jumpObjectsCreater.jumpObjects[i + 1];
            Racer racer = Instantiate(racerPrefab, jumpObject.transform.position + racerCreatPosOffset, racerPrefab.transform.rotation, racersParent);
            racer.SetCurrentJumpObject(jumpObject);
            racer.SetRacerColor(racerInfos[i].RacerColor);
            racer.RacerName = racerInfos[i].RacerName;
            racer.name = racer.RacerName;
            racer.NumOfJumpsToMoveToNextObject = racerInfos[i].NumOfJumpsToMoveToNextObject;
            racers.Add(racer);
        }
    }

    public int GetRacersCount()
    {
        return racerInfos.Count;
    }

    public void OnPrepareNewRace()
    {
        for (int i = 0; i < racers.Count; i++)
        {
            JumpObject jumpObject = jumpObjectsCreater.jumpObjects[i + 1];
            racers[i].SetCurrentJumpObject(jumpObject);
        }
    }
}

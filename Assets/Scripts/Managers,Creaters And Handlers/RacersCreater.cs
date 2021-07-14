using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacersCreater : MonoBehaviour
{
    public static RacersCreater Instance;

    [HideInInspector] public List<Racer> racers = new List<Racer>();

    [SerializeField] private Transform racersParent;
    [SerializeField] private Racer racerPrefab;
    [Tooltip("The offset between the jump object and the racer that is gonna be created above it")]
    [SerializeField] private Vector3 racerCreatPosOffset;
    [SerializeField] private List<RacerInfo> racerInfos;

    private bool hasRacersBeenCreated;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<RacersCreater>();
        }
    }

    public int GetRacersCount()
    {
        return racerInfos.Count;
    }

    public void OnPrepareNewRace()
    {
        if (!hasRacersBeenCreated)
        {
            hasRacersBeenCreated = true;

            for (int i = 0; i < racerInfos.Count; i++)
            {
                JumpObject jumpObject = JumpObjectsCreater.Instance.JumpObjects[i + 1];

                Racer racer = Instantiate(racerPrefab, jumpObject.transform.position + racerCreatPosOffset, racerPrefab.transform.rotation, racersParent);
                racer.Initialize(jumpObject, racerInfos[i].RacerColor, racerInfos[i].RacerName, racerInfos[i].RacerDifficulty, racerInfos[i].NumOfJumpsToMoveToNextObject);

                racers.Add(racer);
            }

            MainCanves.Instance.CreateRacersStatePanels(racers.Count + 1);//The (+1) is the player itself
        }
        else
        {
            for (int i = 0; i < racers.Count; i++)
            {
                JumpObject jumpObject = JumpObjectsCreater.Instance.JumpObjects[i + 1];
                racers[i].OnPrepareNewRace(jumpObject);
            }
        }
    }
}
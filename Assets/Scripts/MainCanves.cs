using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainCanves : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI stateTxt;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI currentLevelTxt;
    [SerializeField] private TextMeshProUGUI nextLevelTxt;
    [SerializeField] private  JumpObjectsCreater jumpObjectsCreater;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject statePanel;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private RacerStatePanel racerStatePanelPrefab;
    [SerializeField] private Transform racersStatePanelsParent;

    private List<RacerStatePanel> racerStatePanels = new List<RacerStatePanel>();

    void Start()
    {
        EventsManager.onNewRaceStart += StartNewRace;

        progressSlider.minValue = 0;
        progressSlider.maxValue = jumpObjectsCreater.GetPathPointsNum();

        PrepareNextRace();
    }

    public void SetStateTxt(Constances.StateTxtWords word)
    {
        stateTxt.gameObject.SetActive(true);
        StartCoroutine(DeactivateStateTxt());

        switch (word)
        {
            case Constances.StateTxtWords.Prefect:
                stateTxt.text = "Prefect";
                break;
            case Constances.StateTxtWords.LongJump:
                stateTxt.text = "Long Jump";
                break;
            case Constances.StateTxtWords.Good:
                stateTxt.text = "Good";
                break;
            case Constances.StateTxtWords.Killer:
                stateTxt.text = "Killer";
                break;
            default:
                break;
        }
    }

    private IEnumerator DeactivateStateTxt()
    {
        yield return new WaitForSeconds(1);
        stateTxt.gameObject.SetActive(false);
    }

    private void ShowRacerNamesTxts(Dictionary<string, float> racers)
    {
        int i = 0;
        foreach (string racerName in racers.Keys)
        {
            //print(i);
            racerStatePanels[i].SetPanelTxt(racerName);
            i++;
        }

        statePanel.SetActive(true);
    }

    public void SetSliderValue(int num)
    {
        progressSlider.value = num;
    }

    private void SetLevelsTextsValues(int currentLevlNum)
    {
        currentLevelTxt.text = currentLevlNum.ToString();
        nextLevelTxt.text = (currentLevlNum + 1).ToString();
    }

    public void OnPrepareNewRace()
    {
        progressSlider.value = 0;
    }

    public void StartNewRace()
    {
        startPanel.SetActive(false);
        gameManager.StartNextLevel();
    }

    public void PrepareNextRace()
    {
        gameManager.PrepareNewRace();
        SetLevelsTextsValues(gameManager.CurrentLevelNum);
        statePanel.SetActive(false);
        startPanel.SetActive(true);
    }

    public void OnRaceFinish(Dictionary<string, float> racers)
    {
        ShowRacerNamesTxts(racers);
    }

    public void CreateRacersStatePanels(int numOfRacers)
    {
        for (int i = 0; i < numOfRacers; i++)
        {
            RacerStatePanel racerStatePanel =  Instantiate(racerStatePanelPrefab, racersStatePanelsParent);
            racerStatePanels.Add(racerStatePanel);
        }
    }

    private void OnDestroy()
    {
        EventsManager.onNewRaceStart -= StartNewRace;
    }
}

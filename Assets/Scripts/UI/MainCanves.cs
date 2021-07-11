using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainCanves : MonoBehaviour
{
    public static MainCanves Instance;

    [SerializeField] private TextMeshProUGUI stateTxt;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private TextMeshProUGUI currentLevelTxt;
    [SerializeField] private TextMeshProUGUI nextLevelTxt;
    [SerializeField] private GameObject ranksPanel;
    [SerializeField] private RacerStatePanel racerStatePanelPrefab;
    [SerializeField] private Transform racersStatePanelsParent;

    private List<RacerStatePanel> racerStatePanels = new List<RacerStatePanel>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<MainCanves>();
        }

        EventsManager.onNewRaceStart += StartNewRace;
    }

    private void Start()
    {
        PrepareNewRace();

        progressSlider.minValue = 0;
        progressSlider.maxValue = JumpObjectsCreater.Instance.GetPathPointsNum();
    }

    #region Characers State Management
    public void SetStateTxt(Constances.StateTxtWords word)
    {
        if (stateTxt.gameObject.activeInHierarchy)
        {
            return;
        }

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

    private IEnumerator ShowRacerNamesTxts(bool hasWon, Dictionary<string, float> racers)
    {
        if (hasWon)
        {
            yield return new WaitForSeconds(4);
        }
        
        int i = 0;

        foreach (string racerName in racers.Keys)
        {
            racerStatePanels[i].SetPanelTxt(racerName);
            i++;
        }

        ranksPanel.SetActive(true);
    }

    public void CreateRacersStatePanels(int numOfRacers)
    {
        for (int i = 0; i < numOfRacers; i++)
        {
            RacerStatePanel racerStatePanel = Instantiate(racerStatePanelPrefab, racersStatePanelsParent);
            racerStatePanels.Add(racerStatePanel);
        }
    }
    #endregion Characers State Management

    #region Top Progress Panel
    public void SetSliderValue(int num)
    {
        progressSlider.value = num;
    }

    private void SetLevelsTextsValues(int currentLevlNum)
    {
        currentLevelTxt.text = currentLevlNum.ToString();
        nextLevelTxt.text = (currentLevlNum + 1).ToString();
    }
    #endregion Top Progress Panel

    #region Race Management
    public void PrepareNewRace()
    {
        progressSlider.value = 0;
        GameManager.Instance.PrepareNewRace();
    }

    public void OnPrepareNewRace()
    {
        SetLevelsTextsValues(GameManager.Instance.CurrentLevelNum);
        progressSlider.maxValue = JumpObjectsCreater.Instance.GetPathPointsNum();
        ranksPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    public void StartNewRace()
    {
        startPanel.SetActive(false);
        GameManager.Instance.StartNextLevel();
    }

    public void OnRaceFinish(bool hasWon, Dictionary<string, float> racers)
    {
        StartCoroutine(ShowRacerNamesTxts(hasWon, racers));
    }
    #endregion Race Management

    private void OnDestroy()
    {
        EventsManager.onNewRaceStart -= StartNewRace;
    }
}
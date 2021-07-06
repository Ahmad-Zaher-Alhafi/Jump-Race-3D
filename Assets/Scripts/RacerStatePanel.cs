using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RacerStatePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI racerStatePanelTxt;

    public void SetPanelTxt(string racerName)
    {
        racerStatePanelTxt.text = racerName;
    }
}

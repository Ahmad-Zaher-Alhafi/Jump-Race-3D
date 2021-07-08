using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JumpObjectCanves : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI jumpObjectNumberTxt;
    [SerializeField] private JumpObject jumpObject;

    private void Start()
    {
        if (!jumpObject.IsItPathJumpObject)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetJumpObjectPanelNumber(int jumpObjectNum)
    {
        jumpObjectNumberTxt.text = jumpObjectNum.ToString();
    }
}

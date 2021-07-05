using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JumpObjectCanves : MonoBehaviour
{
    [SerializeField] private RectTransform numperPanel;
    [SerializeField] private TextMeshProUGUI jumpObjectNumberTxt;
    [SerializeField] private JumpObject jumpObject;

    private Vector3 initialAngle;
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        initialAngle = numperPanel.eulerAngles;

        if (!jumpObject.IsItPathJumpObject)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        //LookTowardsPlayer();

    }

    public void SetJumpObjectPanelNumber(int jumpObjectNum)
    {
        jumpObjectNumberTxt.text = jumpObjectNum.ToString();
    }

    //private void LookTowardsPlayer()
    //{
    //    numperPanel.LookAt(Camera.main.transform);
    //    numperPanel.transform.Rotate(Vector3.up * 180);
    //}
}

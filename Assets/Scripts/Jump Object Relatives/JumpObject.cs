using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpObject : MonoBehaviour
{
    [HideInInspector] public bool IsItLastJumpObject;
    [HideInInspector] public bool IsItPathJumpObject;//if it is a jump object that is on the main path

    
    [SerializeField] private JumpObjectCanves jumpObjectCanves;
    [SerializeField] private JumpObjectBase jumpObjectBase;

    private int jumpObjectIndex;
    public int JumpObjectIndex
    {
        get => jumpObjectIndex;
        set
        {
            jumpObjectIndex = value;
        }
    }

    private Material material;
    private Renderer render;
    

    private void Awake()
    {
        render = GetComponent<Renderer>();
        material = render.material;  
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Constances.PlayerLayerNum)
        {
            if (!IsItLastJumpObject && IsItPathJumpObject)
            {
                OrderToRotate();
                DeactivateCenterPoint();
            }

            Player.Instance.OnTriggerEnterWithJumpableObject(false, this, IsItPathJumpObject);
        }
        else if (other.gameObject.layer == Constances.RacerLayerNum)
        {
            if (IsItPathJumpObject)
            {
                other.GetComponent<Racer>().OnTriggerEnterWithJumpableObject(this);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == Constances.PlayerLayerNum)
        {
            Player.Instance.OnTriggerExitFromJumpAbleObject();
        }
        else if (other.gameObject.layer == Constances.RacerLayerNum)
        {
            other.GetComponent<Racer>().OnTriggerExitFromJumpAbleObject();
        }
    }

    public void Initialize(bool isItPathJumpObject, int jumpObIndex, Color jumpObColor, int pathObjectsCount)
    {
        if (isItPathJumpObject)
        {
            tag = Constances.PathJumpObjectTag;
            SetMaterialColor(jumpObColor);
            jumpObjectIndex = jumpObIndex;
            SetJumpObjectPanelNumber(pathObjectsCount - jumpObIndex);
            IsItPathJumpObject = true;

            if (jumpObIndex + 1 == pathObjectsCount)
            {
                IsItLastJumpObject = true;
                gameObject.AddComponent<WinObject>();

                transform.localScale *= 2;
            }

            if (jumpObIndex <= RacersCreater.Instance.GetRacersCount())
            {
                jumpObjectBase.IsItRacerStartJumpObject = true;
            }
        }
        else
        {
            tag = "Untagged";
            SetMaterialColor(jumpObColor);
        }
    }

    public void OrderToRotate()
    {
        jumpObjectBase.OrderToRotate();
    }

    public void SetJumpObjectPanelNumber(int Num)
    {
        jumpObjectCanves.SetJumpObjectPanelNumber(Num);
    }

    public void SetMaterialColor(Color color)
    {
        material.SetColor("_EmissionColor", color);
    }

    public void DeactivateCenterPoint()
    {
        jumpObjectBase.DeactivateCenterPoint();
    }
}
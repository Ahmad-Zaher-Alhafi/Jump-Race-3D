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

    public void OrderToRotate()
    {
        jumpObjectBase.OrderToRotate();
    }

    public void SetPanelNumber(int Num)
    {
        jumpObjectCanves.SetJumpObjectPanelNumber(Num);
    }

    public void SetMaterialColor(Color color)
    {
        material.SetColor("_EmissionColor", color);
    }
}

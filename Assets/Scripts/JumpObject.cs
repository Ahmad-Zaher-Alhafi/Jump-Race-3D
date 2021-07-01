using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpObject : MonoBehaviour
{
    [HideInInspector] public bool IsItLastJumpObject;
    [HideInInspector] public bool IsItPathJumpObject;//if it is a jump object that is on the main path
    [SerializeField] private JumpObjectCanves jumpObjectCanves;

    private int jumpObjectNumper;
    public int JumpObjectNumper
    {
        get => jumpObjectNumper;
        set
        {
            jumpObjectNumper = value;
            jumpObjectCanves.SetJumpObjectNumber(JumpObjectNumper);
        }
    }
    private Material material;
    private Renderer render;
    

    private void Awake()
    {
        render = GetComponent<Renderer>();
        material = render.material;
    }

    public void SetMaterialColor(Color color)
    {
        material.SetColor("_EmissionColor", color);
    }
}

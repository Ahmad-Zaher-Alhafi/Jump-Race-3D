using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpObject : MonoBehaviour
{
    [HideInInspector] public int JumpObjectNumper;
    [HideInInspector] public bool IsItLastJumpObject;

   

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

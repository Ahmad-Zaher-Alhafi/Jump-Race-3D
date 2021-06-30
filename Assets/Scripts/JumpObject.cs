using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpObject : MonoBehaviour
{
    private Material material;
    private Renderer render;

    private void Awake()
    {
        render = GetComponent<Renderer>();
        material = render.material;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMaterialColor(Color color)
    {
        material.SetColor("_EmissionColor", color);
    }
}

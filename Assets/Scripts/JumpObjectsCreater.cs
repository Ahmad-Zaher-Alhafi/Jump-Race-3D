using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

public class JumpObjectsCreater : MonoBehaviour
{
    [SerializeField] private Spline pathCreater;
    [SerializeField] private GameObject jumpObjectBase;
    [SerializeField] private Color[] jumpObjectsColors;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < pathCreater.nodes.Count; i++)
        {
            GameObject jumpOb = Instantiate(jumpObjectBase, pathCreater.transform.TransformPoint(pathCreater.nodes[i].Position), jumpObjectBase.transform.rotation);
            Color jumpObColor = jumpObjectsColors[Random.Range(0, jumpObjectsColors.Length)];
            jumpOb.transform.GetChild(0).GetComponent<JumpObject>().SetMaterialColor(jumpObColor);

        }
    }
}

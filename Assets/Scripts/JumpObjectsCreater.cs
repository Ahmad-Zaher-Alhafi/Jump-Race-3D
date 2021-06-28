using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

public class JumpObjectsCreater : MonoBehaviour
{
    [SerializeField] private Spline pathCreater;
    [SerializeField] private GameObject jumpObject;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < pathCreater.nodes.Count; i++)
        {
            Instantiate(jumpObject, pathCreater.transform.TransformPoint(pathCreater.nodes[i].Position), jumpObject.transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

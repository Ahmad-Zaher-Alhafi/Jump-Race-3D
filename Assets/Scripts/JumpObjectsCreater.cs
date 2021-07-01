using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

public class JumpObjectsCreater : MonoBehaviour
{
    public static JumpObjectsCreater Instance;

    [SerializeField] private Spline pathCreater;
    [SerializeField] private GameObject jumpObjectBase;
    [SerializeField] private Color[] jumpObjectsColors;

    private List<JumpObject> jumpObjects = new List<JumpObject>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<JumpObjectsCreater>();
        }
    }

    void Start()
    {
        for (int i = 0; i < pathCreater.nodes.Count; i++)
        {
            GameObject jumpOb = Instantiate(jumpObjectBase, pathCreater.transform.TransformPoint(pathCreater.nodes[i].Position), jumpObjectBase.transform.rotation);
            Color jumpObColor = jumpObjectsColors[Random.Range(0, jumpObjectsColors.Length)];
            JumpObject jumpObject = jumpOb.transform.GetChild(0).GetComponent<JumpObject>();
            jumpObjects.Add(jumpObject);
            jumpObject.SetMaterialColor(jumpObColor);
            jumpObject.JumpObjectNumper = i;

            if (i + 1 == pathCreater.nodes.Count)
            {
                jumpObject.IsItLastJumpObject = true;
            }
        }
    }

    public JumpObject GetNextJumpObject(int currentJumpObjectNum)
    {
        if (currentJumpObjectNum + 1 < jumpObjects.Count)
        {
            return jumpObjects[currentJumpObjectNum + 1];
        }
        else
        {
            return null;
        }
    }
}

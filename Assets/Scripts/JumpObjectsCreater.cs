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
    [SerializeField] private Transform ground;
    [SerializeField] private Vector3 exestraJumpObjectBaseOffset;
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
        CreatePathJumpObjects();
        CreatExestraJumpObjects();
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

    /// <summary>
    /// Create the jump objects of the normal path
    /// </summary>
    private void CreatePathJumpObjects()
    {
        for (int i = 0; i < pathCreater.nodes.Count; i++)
        {
            GameObject jumpOb = Instantiate(jumpObjectBase, pathCreater.transform.TransformPoint(pathCreater.nodes[i].Position), jumpObjectBase.transform.rotation);
            jumpOb.tag = Constances.PathJumpObjectTag;
            Color jumpObColor = jumpObjectsColors[Random.Range(1, jumpObjectsColors.Length)];
            JumpObject jumpObject = jumpOb.transform.GetChild(0).GetComponent<JumpObject>();
            jumpObject.tag = Constances.PathJumpObjectTag;
            jumpObjects.Add(jumpObject);
            jumpObject.SetMaterialColor(jumpObColor);
            jumpObject.JumpObjectNumper = i;
            jumpObject.IsItPathJumpObject = true;

            if (i + 1 == pathCreater.nodes.Count)
            {
                jumpObject.IsItLastJumpObject = true;
            }
        }
    }

    /// <summary>
    /// Creat jump objects that are gonna be placed above the ground to help the player not lose easly
    /// </summary>
    private void CreatExestraJumpObjects()
    {
        for (int i = 0; i < pathCreater.nodes.Count; i += 2)
        {
            Vector3 jumpObjectBasePos = pathCreater.transform.TransformPoint(pathCreater.nodes[i].Position);
            float randX = Random.Range(jumpObjectBasePos.x - exestraJumpObjectBaseOffset.x, jumpObjectBasePos.x + exestraJumpObjectBaseOffset.x);
            float randZ = Random.Range(jumpObjectBasePos.z - exestraJumpObjectBaseOffset.z, jumpObjectBasePos.z + exestraJumpObjectBaseOffset.z);
            Vector3 randJumpObjectBasePos = new Vector3(randX, ground.position.y + 2, randZ);
            GameObject jumpOb = Instantiate(jumpObjectBase, randJumpObjectBasePos, jumpObjectBase.transform.rotation);
            jumpOb.tag = "Untagged";
            Color jumpObColor = jumpObjectsColors[0];
            JumpObject jumpObject = jumpOb.transform.GetChild(0).GetComponent<JumpObject>();
            jumpObject.tag = "Untagged";
            jumpObject.SetMaterialColor(jumpObColor);
        }
    }
}

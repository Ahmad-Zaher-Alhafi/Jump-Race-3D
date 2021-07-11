using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

public class JumpObjectsCreater : MonoBehaviour
{
    public static JumpObjectsCreater Instance;

    [HideInInspector] public List<JumpObject> JumpObjects = new List<JumpObject>();

    [SerializeField] private Spline[] pathCreaters;
    [SerializeField] private GameObject jumpObjectBasePrefab;
    [SerializeField] private Color[] jumpObjectsColors;
    [SerializeField] private Transform ground;
    [SerializeField] private Vector3 exestraJumpObjectBaseOffset;//the offset between the exestra jump object and it's corresponding path jump object that is above it

    private Spline currentLevelPathCreater;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<JumpObjectsCreater>();
        }
    }

    public void CreateLevelJumpObjects(int levelNum)
    {
        if (currentLevelPathCreater != null)
        {
            currentLevelPathCreater.gameObject.SetActive(false);
        }

        currentLevelPathCreater = pathCreaters[levelNum];
        currentLevelPathCreater.gameObject.SetActive(true);

        if (!currentLevelPathCreater.WasItsJumObjectsCreated)
        {
            JumpObjects.Clear();
            CreatePathJumpObjects();
            CreatExestraJumpObjects();

            currentLevelPathCreater.WasItsJumObjectsCreated = true;
        }
    }

    private void CreatePathJumpObjects()
    {
        for (int i = 0; i < currentLevelPathCreater.nodes.Count; i++)
        {
            Vector3 pathPointWorldPos = currentLevelPathCreater.transform.TransformPoint(currentLevelPathCreater.nodes[i].Position);
            GameObject jumpObBase = Instantiate(jumpObjectBasePrefab, pathPointWorldPos, jumpObjectBasePrefab.transform.rotation, currentLevelPathCreater.transform);
            JumpObjectBase jumpObjectBase = jumpObBase.GetComponent<JumpObjectBase>();
            Color jumpObColor = jumpObjectsColors[Random.Range(1, jumpObjectsColors.Length)];
            jumpObjectBase.Initialize(true, i, jumpObColor, currentLevelPathCreater.nodes.Count);
            JumpObjects.Add(jumpObjectBase.JumpObjectOfThisBase);
        }
    }

    /// <summary>
    /// Creat jump objects that are gonna be placed On the ground to help the player not lose easly
    /// </summary>
    private void CreatExestraJumpObjects()
    {
        for (int i = 0; i < currentLevelPathCreater.nodes.Count; i += 2)
        {
            Vector3 randExestraJumpObjectBasePos = GetRandomExestraJumpObPos(currentLevelPathCreater.nodes[i].Position);
            GameObject jumpObBase = Instantiate(jumpObjectBasePrefab, randExestraJumpObjectBasePos, jumpObjectBasePrefab.transform.rotation, currentLevelPathCreater.transform);
            JumpObjectBase jumpObjectBase = jumpObBase.GetComponent<JumpObjectBase>();
            Color jumpObColor = jumpObjectsColors[0];
            jumpObjectBase.Initialize(false, i, jumpObColor, currentLevelPathCreater.nodes.Count);
        }
    }

    private Vector3 GetRandomExestraJumpObPos(Vector3 pathNodePos)
    {
        Vector3 pathPointWorldPos = currentLevelPathCreater.transform.TransformPoint(pathNodePos);
        float randX = Random.Range(pathPointWorldPos.x - exestraJumpObjectBaseOffset.x, pathPointWorldPos.x + exestraJumpObjectBaseOffset.x);
        float randZ = Random.Range(pathPointWorldPos.z - exestraJumpObjectBaseOffset.z, pathPointWorldPos.z + exestraJumpObjectBaseOffset.z);
        return new Vector3(randX, ground.position.y + 2, randZ);
    }

    public JumpObject GetNextJumpObject(int currentJumpObjectNum)
    {
        if (currentJumpObjectNum + 1 < JumpObjects.Count)
        {
            return JumpObjects[currentJumpObjectNum + 1];
        }
        else
        {
            return null;
        }
    }

    public int GetPathPointsNum()
    {
        return currentLevelPathCreater.nodes.Count;
    }

    public void OnPrepareNewRace(int levelNum)
    {
        CreateLevelJumpObjects(levelNum);
    }

    public int GetPathsNumber()
    {
        return pathCreaters.Length;
    }
}

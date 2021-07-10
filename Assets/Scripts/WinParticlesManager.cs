using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinParticlesManager : MonoBehaviour
{
    public static WinParticlesManager Instance;

    [SerializeField] private ParticleSystem[] winParticles;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<WinParticlesManager>();
        }
    }

    public void PlayWinParticles()
    {
        foreach (ParticleSystem winParticle in winParticles)
        {
            winParticle.transform.position = JumpObjectsCreater.Instance.JumpObjects[JumpObjectsCreater.Instance.JumpObjects.Count - 1].transform.position;
            winParticle.Play();
        }
    }
}
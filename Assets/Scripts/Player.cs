using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float jumpForce;

    private Rigidbody rig;
    private bool hastoMove;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        
    }

    
}

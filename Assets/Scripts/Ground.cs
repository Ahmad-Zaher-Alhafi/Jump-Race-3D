using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private ParticleSystem waterParticle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Constances.PlayerLayerNum)
        {
            other.GetComponent<Player>().Die();
            waterParticle.transform.position = new Vector3(other.transform.position.x ,transform.position.y, other.transform.position.z);
            waterParticle.Play();
        }
    }
}

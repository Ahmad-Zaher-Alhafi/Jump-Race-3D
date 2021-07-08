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
            waterParticle.transform.position = new Vector3(Player.Instance.transform.position.x ,transform.position.y, Player.Instance.transform.position.z);
            waterParticle.Play();
            Player.Instance.Die();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathParticleController : MonoBehaviour {

    public List<ParticleSystem> deathParticles;
    

    [ContextMenu("Die!")]
    public void Die()
    {
        transform.parent = null;

        foreach (var ps in deathParticles)
        {
            ps.gameObject.SetActive(true);
        }

        Destroy(gameObject, 10);
    }
}

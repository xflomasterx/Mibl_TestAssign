using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleController : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _particleSystems;
    public void Emit()
    {
        foreach (ParticleSystem ps in _particleSystems)
                ps.Play();
    }
    public void StopEmit()
    {
        foreach (ParticleSystem ps in _particleSystems)
                ps.Stop();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Particle Emission")]
public class ParticleEmissionConfig : ScriptableObject
{

    public float startLifetime;
    public float startSpeed;
    public float startSize;
    public float gravityModifier;

    public int emissionRate;

    public ParticleSystemSimulationSpace simulationSpace;

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Particle System")]
public class ParticlesConfig : ScriptableObject {

    public Color color;
    public float startLifetime;
    public float emissionRate;

    public ParticleSystemSimulationSpace simulationSpace;

    [SortingLayer]
    public int sortingLayer;

    public int orderInLayer;

    public Material renderMaterial;
}

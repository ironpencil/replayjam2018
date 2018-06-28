using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Particle System")]
public class ParticlesConfig : ScriptableObject {

    public Color color;

    [SortingLayer]
    public int sortingLayer;

    public int orderInLayer;

    public Material renderMaterial;
}

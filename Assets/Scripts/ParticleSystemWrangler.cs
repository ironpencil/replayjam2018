using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemWrangler : MonoBehaviour {

    public GameObject particleSystemPrefab;

    public ParticlesConfig tintedConfig;
    public ParticlesConfig whiteConfig;

    public bool playOnAwake = true;

    private ParticleSystem tintedParticles;
    private ParticleSystem whiteParticles;

    private uint randomSeed;

    private void Awake()
    {
        randomSeed = (uint) Random.Range(0, uint.MaxValue);

        tintedParticles = Instantiate(particleSystemPrefab, transform).GetComponent<ParticleSystem>();
        whiteParticles = Instantiate(particleSystemPrefab, transform).GetComponent<ParticleSystem>();

        ConfigureParticleSystem(tintedParticles, tintedConfig);
        ConfigureParticleSystem(whiteParticles, whiteConfig);

        if (playOnAwake)
        {
            tintedParticles.Play();
            whiteParticles.Play();
        }
    }

    private void ConfigureParticleSystem(ParticleSystem ps, ParticlesConfig config)
    {
        var main = ps.main;
        var emission = ps.emission;
        var renderer = ps.GetComponent<ParticleSystemRenderer>();

        main.startLifetime = config.startLifetime;
        main.simulationSpace = config.simulationSpace;
        main.startColor = config.color;

        emission.rateOverTime = config.emissionRate;

        renderer.sortingLayerID = config.sortingLayer;
        renderer.sortingOrder = config.orderInLayer;
        renderer.material = config.renderMaterial;

        ps.randomSeed = randomSeed;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

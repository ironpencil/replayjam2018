using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemWrangler : MonoBehaviour {

    public GameObject particleSystemPrefab;

    public ParticlesConfig tintedConfig;
    public ParticlesConfig whiteConfig;

    public ParticleEmissionConfig defaultEmissionConfig;
    public ParticleEmissionConfig blackoutEmissionConfig;

    public bool playOnAwake = true;

    private ParticleSystem tintedParticles;
    private ParticleSystem whiteParticles;

    private ParticleEmissionConfig emissionConfig;

    private uint randomSeed;

    private void Awake()
    {
        randomSeed = (uint) Random.Range(0, uint.MaxValue);

        tintedParticles = Instantiate(particleSystemPrefab, transform).GetComponent<ParticleSystem>();
        whiteParticles = Instantiate(particleSystemPrefab, transform).GetComponent<ParticleSystem>();

        emissionConfig = defaultEmissionConfig;

        ConfigureParticleSystems();

        if (playOnAwake)
        {
            tintedParticles.Play();
            whiteParticles.Play();
        }
    }

    private void ConfigureParticleSystem(ParticleSystem ps, ParticlesConfig config)
    {
        if (ps != null && config != null)
        {
            var main = ps.main;
            var emission = ps.emission;
            var renderer = ps.GetComponent<ParticleSystemRenderer>();

            main.startColor = config.color;

            main.startLifetime = emissionConfig.startLifetime;
            main.simulationSpace = emissionConfig.simulationSpace;
            main.startSize = emissionConfig.startSize;
            main.startSpeed = emissionConfig.startSpeed;
            main.gravityModifier = emissionConfig.gravityModifier;

            emission.rateOverTime = emissionConfig.emissionRate;

            renderer.sortingLayerID = config.sortingLayer;
            renderer.sortingOrder = config.orderInLayer;
            renderer.material = config.renderMaterial;

            if (!ps.isPlaying)
            {
                ps.randomSeed = randomSeed;
            }
        }
    }

    public void ConfigureParticleSystems(bool andRestart = false)
    {
        ConfigureParticleSystem(tintedParticles, tintedConfig);
        ConfigureParticleSystem(whiteParticles, whiteConfig);

        if (andRestart)
        {
            RestartParticleSystems();
        }
    }

    [ContextMenu("Restart Particle Systems")]
    public void RestartParticleSystems()
    {
        tintedParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        whiteParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);

        tintedParticles.Play();
        whiteParticles.Play();
    }

    [ContextMenu("Configure and Restart")]
    public void ReconfigAndRestart()
    {
        ConfigureParticleSystems(true);
    }

    [ContextMenu("Start Blackout")]
    public void OnStartBlackout()
    {
        emissionConfig = blackoutEmissionConfig;
        ReconfigAndRestart();
    }

    [ContextMenu("End Blackout")]
    public void OnEndBlackout()
    {
        emissionConfig = defaultEmissionConfig;
        ReconfigAndRestart();
    }
}

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

    [SerializeField]
    private bool shouldPlay = false;

    private void Awake()
    {
        randomSeed = (uint) Random.Range(0, uint.MaxValue);

        tintedParticles = Instantiate(particleSystemPrefab, transform).GetComponent<ParticleSystem>();
        whiteParticles = Instantiate(particleSystemPrefab, transform).GetComponent<ParticleSystem>();

        emissionConfig = defaultEmissionConfig;

        ConfigureParticleSystems();

        if (playOnAwake)
        {
            Play();
        } else
        {
            Stop();
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

            if (ps.randomSeed != randomSeed)
            {
                if (ps.isPlaying)
                {
                    ps.Stop();
                    ps.randomSeed = randomSeed;
                    ps.Play();
                }
                else
                {
                    ps.randomSeed = randomSeed;
                }
            }
        }
    }

    public void ConfigureParticleSystems()
    {
        ConfigureParticleSystem(tintedParticles, tintedConfig);
        ConfigureParticleSystem(whiteParticles, whiteConfig);
    }

    [ContextMenu("Start Blackout")]
    public void OnStartBlackout()
    {
        emissionConfig = blackoutEmissionConfig;
        ConfigureParticleSystems();

        SetPlaying(shouldPlay);
    }

    [ContextMenu("End Blackout")]
    public void OnEndBlackout()
    {
        emissionConfig = defaultEmissionConfig;
        ConfigureParticleSystems();

        SetPlaying(shouldPlay);
    }

    public void Stop()
    {
        shouldPlay = false;
        tintedParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        whiteParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
    }

    public void Play()
    {
        shouldPlay = true;
        if (!tintedParticles.isPlaying)
        {
            tintedParticles.Play();
        }

        if (!whiteParticles.isPlaying)
        {
            whiteParticles.Play();
        }
    }

    public void SetPlaying(bool shouldPlay)
    {
        if (shouldPlay)
        {
            Play();
        } else
        {
            Stop();
        }
    }
}

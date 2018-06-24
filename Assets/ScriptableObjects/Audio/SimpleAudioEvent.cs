using UnityEngine;

[CreateAssetMenu(menuName="Audio Events/Simple")]
class SimpleAudioEvent : AudioEvent
{
    public AudioClip[] clips;

    [Range(0.0f, 1.0f)]
    public float minVolume = 1.0f;
    [Range(0.0f, 1.0f)]
    public float maxVolume = 1.0f;

    [Range(-3.0f, 3.0f)]
    public float minPitch = 1.0f;
    [Range(-3.0f, 3.0f)]
    public float maxPitch = 1.0f;

    public override void Play(AudioSource source) {
        if (clips.Length == 0) return;

        AudioClip clip = clips[Random.Range(0, clips.Length)];
        source.volume = Random.Range(minVolume, maxVolume);
        source.pitch = Random.Range(minPitch, maxPitch);
        source.PlayOneShot(clip);
    }
}
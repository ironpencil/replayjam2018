using UnityEngine;

[CreateAssetMenu(menuName="Audio Events/Simple")]
class SimpleAudioEvent : AudioEvent
{
    public AudioClip[] clips;

    public bool playRandom = true;

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

        source.volume = Random.Range(minVolume, maxVolume);
        source.pitch = Random.Range(minPitch, maxPitch);

        if (playRandom)
        {
            AudioClip clip = clips[Random.Range(0, clips.Length)];
            source.PlayOneShot(clip);
        } else
        {
            foreach (var clip in clips)
            {
                source.PlayOneShot(clip);
            }
        }
    }
}
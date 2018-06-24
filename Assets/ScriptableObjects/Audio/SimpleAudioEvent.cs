using UnityEngine;

[CreateAssetMenu(menuName="Audio Events/Simple")]
class SimpleAudioEvent : AudioEvent
{
    public AudioClip[] clips;

    public override void Play(AudioSource source) {
        if (clips.Length == 0) return;

        source.clip = clips[Random.Range(0, clips.Length)];
        source.PlayOneShot(source.clip);
    }
}
using UnityEngine;

public enum SoundName
{
    HOOK1,
    HOOK2,
    GRUNT1,
    GRUNT2,
    SPLASH,
    SPLASH2
}

/// <summary>
/// Handles audio playback for the game.
/// Listens to play sound events - no singleton access required.
/// </summary>
public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] soundList;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        GameEvents.onPlaySound += HandlePlaySound;
    }

    void OnDestroy()
    {
        GameEvents.onPlaySound -= HandlePlaySound;
    }

    private void HandlePlaySound(SoundName sound, float volume)
    {
        if (audioSource != null && soundList != null)
        {
            if ((int)sound < soundList.Length)
            {
                audioSource.PlayOneShot(soundList[(int)sound], volume);
            }
            else
            {
                Debug.LogWarning($"[SoundManager] Sound index {sound} is out of range!");
            }
        }
        else
        {
            Debug.LogWarning("[SoundManager] AudioSource or soundList not properly initialized!");
        }
    }
}

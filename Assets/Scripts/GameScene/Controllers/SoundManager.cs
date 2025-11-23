using UnityEngine;


public enum SoundName
{
    SUS,
    HUH,
    SPLASH,
    SPLASH2
}
public class SoundManager : SingletonNoPersist<SoundManager>
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] soundList;

    protected override void Awake()
    {
        base.Awake();  // Handle singleton logic
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundName sound, float volume = 1f)
    {
        if (instance != null && instance.audioSource != null && instance.soundList != null)
        {
            if ((int)sound < instance.soundList.Length)
            {
                instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
            }
            else
            {
                Debug.LogWarning($"[SoundManager] Sound index {sound} is out of range!");
            }
        }
        else
        {
            Debug.LogWarning("[SoundManager] Instance or components not properly initialized!");
        }
    }
}

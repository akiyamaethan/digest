using UnityEngine;
using UnityEngine.Rendering;


public enum SoundName
{
    SUS,
    HUH,
    SPLASH,
    SPLASH2
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] soundList;
    void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundName sound, float volume = 1f)
    {
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
    }
}

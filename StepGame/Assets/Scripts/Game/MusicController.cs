using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance;
    private AudioSource audioSource;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
        }
    }
}

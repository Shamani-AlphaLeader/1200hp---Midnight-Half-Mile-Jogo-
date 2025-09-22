using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    [Header("Playlist")]
    public AudioClip[] playlist;

    [Header("Configuração")]
    public float fadeDuration = 1.5f;
    public float volume = 0.7f;

    private int currentTrack = 0;
    private AudioSource audioSource;
    private static MusicManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = false;
        audioSource.volume = 0f;
    }

    void Start()
    {
        if (playlist.Length > 0)
        {
            // 🎲 Define faixa inicial aleatória
            currentTrack = Random.Range(0, playlist.Length);
            StartCoroutine(FadeInTrack(currentTrack));
        }
    }

    void Update()
    {
        if (!audioSource.isPlaying && playlist.Length > 0)
        {
            NextTrack();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            NextTrack();
        }
    }

    public void NextTrack()
    {
        currentTrack++;
        if (currentTrack >= playlist.Length)
            currentTrack = 0;

        StartCoroutine(FadeOutInTrack(currentTrack));
    }

    private IEnumerator FadeInTrack(int index)
    {
        if (index < 0 || index >= playlist.Length) yield break;

        audioSource.clip = playlist[index];
        audioSource.Play();

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0f, volume, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = volume;
    }

    private IEnumerator FadeOutInTrack(int nextIndex)
    {
        float elapsed = 0f;
        float startVolume = audioSource.volume;
        while (elapsed < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = 0f;
        audioSource.Stop();

        yield return FadeInTrack(nextIndex);
    }
}

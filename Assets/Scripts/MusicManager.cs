using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    [Header("Playlist")]
    public AudioClip[] playlist;     // lista de m�sicas
    public int startTrackIndex = 0;  // faixa inicial escolhida

    [Header("Configura��o")]
    public float fadeDuration = 1.5f; // tempo de fade in/out
    public float volume = 0.7f;       // volume padr�o (0 a 1)

    private int currentTrack = 0;
    private AudioSource audioSource;
    private static MusicManager instance;

    void Awake()
    {
        // garante que s� exista um MusicManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // mant�m entre cenas
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
        audioSource.volume = 0f; // come�a silencioso
    }

    void Start()
    {
        if (playlist.Length > 0)
        {
            currentTrack = startTrackIndex;
            StartCoroutine(FadeInTrack(currentTrack));
        }
    }

    void Update()
    {
        // troca autom�tica quando a m�sica acabar
        if (!audioSource.isPlaying && playlist.Length > 0)
        {
            NextTrack();
        }

        // bot�o de pular m�sica (teste no teclado)
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextTrack();
        }
    }

    public void NextTrack()
    {
        currentTrack++;
        if (currentTrack >= playlist.Length)
            currentTrack = 0; // reinicia playlist

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
        // fade out
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

        // fade in pr�xima faixa
        yield return FadeInTrack(nextIndex);
    }
}
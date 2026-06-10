using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] AudioSource musicaSource;
    [SerializeField] AudioSource efectosSource;

    [Header("Musicas")]
    [SerializeField] AudioClip musicaMenu;
    [SerializeField] AudioClip musicaJuego;

    [Header("Volumen")]
    [SerializeField] float volumenMenu = 0.3f;
    [SerializeField] float volumenJuego = 0.4f;
    [SerializeField] float volumenEfectos = 0.7f;

    [Header("Efectos")]
    [SerializeField] AudioClip clipDisparoJugador;
    [SerializeField] AudioClip clipDisparoEnemigo;
    [SerializeField] AudioClip clipMuerteEnemigo;
    [SerializeField] AudioClip clipMuerteJugador;
    [SerializeField] AudioClip clipPowerUp;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnScenaCargada;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnScenaCargada;
    }

    void Start()
    {
        TocarMusicaSegunEscena(SceneManager.GetActiveScene().name);
    }

    void OnScenaCargada(Scene escena, LoadSceneMode mode)
    {
        TocarMusicaSegunEscena(escena.name);
    }

    void TocarMusicaSegunEscena(string nombreEscena)
    {
        if (nombreEscena == "mainMenu" || nombreEscena == "MainMenu")
            CambiarMusica(musicaMenu, volumenMenu);
        else
            CambiarMusica(musicaJuego, volumenJuego);
    }

    void CambiarMusica(AudioClip nuevoClip, float volumen)
    {
        if (nuevoClip == null) return;
        if (musicaSource.clip == nuevoClip) return;

        musicaSource.Stop();
        musicaSource.clip = nuevoClip;
        musicaSource.loop = true;
        musicaSource.volume = volumen;
        musicaSource.Play();
    }

    // ── Efectos ──────────────────────────────────────
    public void PlayDisparoJugador()
    {
        if (clipDisparoJugador == null) return;
        efectosSource.PlayOneShot(clipDisparoJugador, volumenEfectos);
    }

    public void PlayDisparoEnemigo()
    {
        if (clipDisparoEnemigo == null) return;
        efectosSource.PlayOneShot(clipDisparoEnemigo, volumenEfectos);
    }

    public void PlayMuerteEnemigo()
    {
        if (clipMuerteEnemigo == null) return;
        efectosSource.PlayOneShot(clipMuerteEnemigo, volumenEfectos);
    }

    public void PlayMuerteJugador()
    {
        if (clipMuerteJugador == null) return;
        efectosSource.PlayOneShot(clipMuerteJugador, volumenEfectos);
    }

    public void PlayPowerUp()
    {
        if (clipPowerUp == null) return;
        efectosSource.PlayOneShot(clipPowerUp, volumenEfectos);
    }
}
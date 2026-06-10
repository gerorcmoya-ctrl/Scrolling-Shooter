using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("Pausa")]
    [SerializeField] GameObject pausaPanel;
    [Header("HUD")]
    [SerializeField] TextMeshProUGUI vidaTexto;
    [SerializeField] TextMeshProUGUI scoreTexto;

    [Header("Game Over")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TextMeshProUGUI scoreFinalGameOver;

    [Header("Victoria")]
    [SerializeField] GameObject victoriaPanel;
    [SerializeField] TextMeshProUGUI scoreFinalVictoria;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void OnEnable()
    {
        PlayerHealth.OnHealthChanged += ActualizarVida;
        ScoreManager.OnScoreChanged += ActualizarScore;
    }

    void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= ActualizarVida;
        ScoreManager.OnScoreChanged -= ActualizarScore;
    }

    void Start()
    {
        gameOverPanel.SetActive(false);
        victoriaPanel.SetActive(false);
        ActualizarScore(0);
    }

    // ── HUD ──────────────────────────────────────────
    void ActualizarVida(int vida)
    {
        vidaTexto.text = $"VIDAS: {vida}";
    }

    void ActualizarScore(int score)
    {
        scoreTexto.text = $"SCORE: {score}";
    }
    public void ShowPausa(bool mostrar)
    {
        pausaPanel.SetActive(mostrar);
    }

    public void OnReanudar()
    {
        Time.timeScale = 1f;
        pausaPanel.SetActive(false);
    }
    // ── Game Over ─────────────────────────────────────
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);

        // Forzar el score actual en el momento exacto que aparece
        int scoreActual = ScoreManager.Instance != null ? ScoreManager.Instance.Score : 0;
        scoreFinalGameOver.text = $"SCORE FINAL: {scoreActual}";

        Time.timeScale = 0f;
    }

    // ── Victoria ──────────────────────────────────────
    public void ShowVictory()
    {
        StartCoroutine(VictoriaConDelay());
    }

    System.Collections.IEnumerator VictoriaConDelay()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        int score = ScoreManager.Instance != null ? ScoreManager.Instance.Score : 0;
        scoreFinalVictoria.text = $"SCORE FINAL: {score}";
        victoriaPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // ── Botones ───────────────────────────────────────
    public void OnReiniciar()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void OnMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
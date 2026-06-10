using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER");
        UIManager.Instance?.ShowGameOver();
    }

    public void Victory()
    {
        Debug.Log("VICTORIA");
        UIManager.Instance?.ShowVictory();
    }

    bool pausado = false;

    public void TogglePause()
    {
        pausado = !pausado;

        if (pausado)
        {
            Time.timeScale = 0f;
            UIManager.Instance?.ShowPausa(true);
        }
        else
        {
            Time.timeScale = 1f;
            UIManager.Instance?.ShowPausa(false);
        }
    }
    public void ShowBossWarning() => Debug.Log("BOSS WARNING");
}
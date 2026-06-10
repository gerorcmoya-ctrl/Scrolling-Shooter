using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int Score { get; private set; }

    // Evento para avisar a la UI
    public static System.Action<int> OnScoreChanged;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddScore(int cantidad)
    {
        Score += cantidad;
        Debug.Log($"Score actualizado: {Score}");
        OnScoreChanged?.Invoke(Score);
    }
}
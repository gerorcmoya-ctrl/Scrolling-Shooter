using UnityEngine;

public class BossDetector : MonoBehaviour
{
    EnemyHealth enemyHealth;

    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        if (enemyHealth != null)
            enemyHealth.OnDeath += OnBossMuerto;
    }

    void OnBossMuerto()
    {
        Debug.Log("Boss muerto — llamando Victoria directamente");
        // Llamar directo sin esperar — el GameManager maneja el delay
        GameManager.Instance.Victory();
    }
}
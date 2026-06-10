using UnityEngine;
using System;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] int maxHealth = 3;
    [SerializeField] int scoreValue = 100;

    [Header("UI")]
    [SerializeField] EnemyHealthBar healthBar;

    [Header("VFX")]
    [SerializeField] GameObject deathVFX;

    [Header("Drops")]
    [SerializeField] GameObject[] posiblesDrops;
    [SerializeField] float chanceDeDrop = 0.3f;

    [Header("Flash de daño")]
    [SerializeField] Renderer[] renderers;

    int currentHealth;
    bool isDead;

    public Action OnDeath;

    void Awake()
    {
        currentHealth = maxHealth;
        healthBar?.UpdateBar(currentHealth, maxHealth);
    }

    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;
        // Debug.Log($"{gameObject.name} recibió {dmg} — vida: {currentHealth}");

        healthBar?.UpdateBar(currentHealth, maxHealth);

        StartCoroutine(FlashDaño());

        if (currentHealth <= 0)
            Morir();
    }

    IEnumerator FlashDaño()
    {
        // Parpadeo 3 veces rápido
        for (int i = 0; i < 3; i++)
        {
            foreach (var r in renderers)
                r.enabled = false;

            yield return new WaitForSeconds(0.05f);

            foreach (var r in renderers)
                r.enabled = true;

            yield return new WaitForSeconds(0.05f);
        }
    }

    void Morir()
    {
        isDead = true;

        Debug.Log($"Morir() llamado en {gameObject.name} | OnDeath tiene suscriptores: {OnDeath != null}");

        ScoreManager.Instance.AddScore(scoreValue);

        if (deathVFX)
            Instantiate(deathVFX, transform.position, Quaternion.identity);

        if (posiblesDrops.Length > 0 && UnityEngine.Random.value < chanceDeDrop)
        {
            int idx = UnityEngine.Random.Range(0, posiblesDrops.Length);
            Instantiate(posiblesDrops[idx], transform.position, Quaternion.identity);
        }

        OnDeath?.Invoke();
        Debug.Log("OnDeath invocado");
        Destroy(gameObject);
    }
}
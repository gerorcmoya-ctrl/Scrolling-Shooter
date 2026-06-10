using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] int maxHealth = 3;
    [SerializeField] float invincibleTime = 1.5f;   // segundos de invencibilidad tras recibir daño

    [Header("VFX — arrastrá prefabs acá (opcional por ahora)")]
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject deathVFX;
    [SerializeField] GameObject shieldVFX;

    [Header("Renderers para el parpadeo — arrastrá los MeshRenderer de la nave")]
    [SerializeField] Renderer[] shipRenderers;

    int currentHealth;
    bool isInvincible;
    bool shieldActive;

    // Evento que avisa a la UI cuando cambia la vida
    public static System.Action<int> OnHealthChanged;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        // Avisa a la UI al inicio para mostrar la vida inicial
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(int cantidad)
    {
        if (isInvincible || shieldActive) return;

        // Camera Shake
        CameraFollow.Instance?.Shake(6f, 0.3f);

        if (hitVFX)
            Instantiate(hitVFX, transform.position, Quaternion.identity);

        currentHealth -= cantidad;
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
            Morir();
        else
            StartCoroutine(RutinaInvencible());
    }

    IEnumerator RutinaInvencible()
    {
        isInvincible = true;
        float tiempo = 0f;

        while (tiempo < invincibleTime)
        {
            // Parpadeo: apaga y prende los renderers
            foreach (var r in shipRenderers)
                r.enabled = !r.enabled;

            yield return new WaitForSeconds(0.1f);
            tiempo += 0.1f;
        }

        // Asegurarse de que queden visibles al terminar
        foreach (var r in shipRenderers)
            r.enabled = true;

        isInvincible = false;
    }

    void Morir()
    {
        if (deathVFX)
            Instantiate(deathVFX, transform.position, Quaternion.identity);

        // Desactivar controles
        GetComponent<PlayerInputs>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;

        gameObject.SetActive(false);
        GameManager.Instance.GameOver();
    }

    public void Heal(int cantidad)
    {
        currentHealth = Mathf.Min(currentHealth + cantidad, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void ActivateShield(float duracion)
    {
        StartCoroutine(RutinaEscudo(duracion));
    }

    IEnumerator RutinaEscudo(float duracion)
    {
        shieldActive = true;
        if (shieldVFX) shieldVFX.SetActive(true);

        yield return new WaitForSeconds(duracion);

        shieldActive = false;
        if (shieldVFX) shieldVFX.SetActive(false);
    }
}
using UnityEngine;
using System.Collections;

public class BossHealth : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] int maxHealth = 200;
    [SerializeField] int fase2Threshold = 100;

    [Header("Flash de daño")]
    [SerializeField] Renderer[] renderers;

    [Header("VFX")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] GameObject fase2VFX;

    int currentHealth;
    bool isDead;
    bool fase2Activada;

    BossController controller;
    BossHealthBar healthBar;

    public static System.Action OnBossMuerto;

    void Awake()
    {
        currentHealth = maxHealth;
        controller = GetComponent<BossController>();
    }

    void Start()
    {
        // Buscar la barra automáticamente en la escena
        healthBar = FindObjectOfType<BossHealthBar>();

        if (healthBar == null)
            Debug.LogError("No se encontró BossHealthBar en la escena!");
        else
            Debug.Log("BossHealthBar encontrada OK");

        healthBar?.Mostrar();
        healthBar?.UpdateBar(currentHealth, maxHealth);
    }

    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;
        healthBar?.UpdateBar(currentHealth, maxHealth);
        StartCoroutine(FlashDaño());

        if (currentHealth <= fase2Threshold && !fase2Activada)
            ActivarFase2();

        if (currentHealth <= 0)
            Morir();
    }

    void ActivarFase2()
    {
        fase2Activada = true;
        if (fase2VFX)
            Instantiate(fase2VFX, transform.position, Quaternion.identity);
        controller?.ActivarFase2();
    }

    IEnumerator FlashDaño()
    {
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
        if (deathVFX)
            Instantiate(deathVFX, transform.position, Quaternion.identity);
        healthBar?.Ocultar();
        OnBossMuerto?.Invoke();
        StartCoroutine(RutinaVictoria());
    }

    IEnumerator RutinaVictoria()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.Victory();
        Destroy(gameObject);
    }
}
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] Transform barraFill;
    [SerializeField] GameObject barraContainer;  // arrastrá BossBarBackground acá

    void Start()
    {
        // Ocultar al inicio
        if (barraContainer) barraContainer.SetActive(false);
    }

    public void Mostrar()
    {
        Debug.Log("BossHealthBar.Mostrar() llamado");
        if (barraContainer)
        {
            barraContainer.SetActive(true);
            Debug.Log("BossBarBackground activado");
        }
        else
            Debug.LogError("barraContainer es NULL — no está asignado");
    }

    public void Ocultar()
    {
        if (barraContainer) barraContainer.SetActive(false);
    }

    public void UpdateBar(int vidaActual, int vidaMax)
    {
        float porcentaje = (float)vidaActual / vidaMax;
        barraFill.localScale = new Vector3(porcentaje, 1f, 1f);
    }
}
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] Transform barraFill;    // arrastrá el Fill acá
    [SerializeField] Canvas canvas;

    Transform camara;

    void Start()
    {
        camara = Camera.main.transform;
    }

    void LateUpdate()
    {
        // La barra siempre mira a la cámara
        if (camara != null)
            canvas.transform.LookAt(
                canvas.transform.position + camara.forward
            );
    }

    public void UpdateBar(int vidaActual, int vidaMax)
    {
        // Escalar la barra en X según el porcentaje de vida
        float porcentaje = (float)vidaActual / vidaMax;
        barraFill.localScale = new Vector3(porcentaje, 1f, 1f);
    }
}
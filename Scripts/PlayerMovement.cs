using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Velocidad")]
    [SerializeField] float moveSpeed = 12f;

    [Header("Limites del area de juego")]
    [SerializeField] float xLimit = 8f;
    [SerializeField] float zLimit = 5f;

    [Header("Inclinacion visual")]
    [SerializeField] float tiltAngle = 25f;
    [SerializeField] float tiltSpeed = 10f;

    Vector2 input;

    // Update en vez de FixedUpdate para movimiento suave
    void Update()
    {
        MoverNave();
        InclinarNave();
    }

    void MoverNave()
    {
        Vector3 movimiento = new Vector3(input.x, 0f, input.y)
                             * moveSpeed * Time.deltaTime;

        Vector3 nuevaPos = transform.position + movimiento;

        nuevaPos.x = Mathf.Clamp(nuevaPos.x, -xLimit, xLimit);
        nuevaPos.z = Mathf.Clamp(nuevaPos.z, -zLimit, zLimit);
        nuevaPos.y = 0f;

        transform.position = nuevaPos;
    }

    void InclinarNave()
    {
        float inclinacionZ = -input.x * tiltAngle;
        float inclinacionX = input.y * tiltAngle * 0.4f;

        Quaternion rotacionObjetivo = Quaternion.Euler(inclinacionX, 0f, inclinacionZ);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            rotacionObjetivo,
            tiltSpeed * Time.deltaTime
        );
    }

    public void SetInput(Vector2 nuevoInput)
    {
        input = nuevoInput;
    }
}
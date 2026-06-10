using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] PlayerMovement movement;
    [SerializeField] PlayerShooter shooter;

    PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();
        Debug.Log("PlayerInputs: Awake OK");
    }

    void OnEnable()
    {
        controls.Player.Enable();
        Debug.Log("PlayerInputs: Controls habilitados");

        controls.Player.Fire.performed += _ => {
            Debug.Log("FIRE presionado");
            shooter.StartShooting();
        };
        controls.Player.Fire.canceled += _ => shooter.StopShooting();
        controls.Player.Pause.performed += _ => GameManager.Instance.TogglePause();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    void Update()
    {
        Vector2 moveInput = controls.Player.Move.ReadValue<Vector2>();

        // Comentá esta línea
        // if (moveInput != Vector2.zero)
        //     Debug.Log($"Move input: {moveInput}");

        movement.SetInput(moveInput);
    }
}
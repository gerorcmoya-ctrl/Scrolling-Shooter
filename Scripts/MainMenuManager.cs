using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject controlesPanel;

    public void Jugar()
    {
        SceneManager.LoadScene(1);  // carga la escena número 1 del Build Settings
    }

    public void Salir()
    {
        Application.Quit();
        Debug.Log("Saliendo...");
    }

    public void AbrirControles()
    {
        controlesPanel.SetActive(true);
    }

    public void CerrarControles()
    {
        controlesPanel.SetActive(false);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        // Load your main game scene by name or index
        SceneManager.LoadScene("Gladiator Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

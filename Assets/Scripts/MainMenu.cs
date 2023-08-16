using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void QuitGame() {
        Application.Quit();
    }

    public void StartGame() {
        SceneManager.LoadScene("Level 1");
    }

    public void BackToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void HowToPlay() {
        SceneManager.LoadScene("HowToPlay");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartGame() {
        SceneManager.LoadScene("City");
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene("Start Menu");
    }
}

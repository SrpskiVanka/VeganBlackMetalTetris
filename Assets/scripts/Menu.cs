using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject quitButton;
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }
}

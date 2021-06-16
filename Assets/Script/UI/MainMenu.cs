using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play() //play function
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//loads the scene second to the main menu
    }

    public void Quit() //quit function
    {
        Application.Quit();//quits window
    }
}

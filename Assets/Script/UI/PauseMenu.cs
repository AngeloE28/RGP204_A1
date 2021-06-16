using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public bool isPasued;
    
    void Start()
    {
        pauseMenu.SetActive(false); //keeeps panel closed

    }

    public void PauseGame() //function to pause game
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; //stops the game time to avoid any interaction behind the pause menu
        isPasued = true;
    }

    public void ResumeGame()//function to resume game
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPasued = false;

    }
    public void Restart()//function to restart the game
    {
        Time.timeScale = 1f; //sets time back to active
        SceneManager.LoadScene("Game");
    }


    public void QuitTOMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); //loads tthe scene main menu
    }

   

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) //using escape key to pause and unpause
        {
            if(isPasued)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
}

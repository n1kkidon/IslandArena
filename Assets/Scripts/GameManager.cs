using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject gameOverUI;
    public GameObject pauseMenuUI;

    public GameObject player;
    public GameObject playerCam;
    bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !gameOverUI.activeInHierarchy)
        {
            if (isPaused)
                ResumeGame();
            else PauseGame();
            isPaused = !isPaused;          
        }
        if(gameOverUI.activeInHierarchy || pauseMenuUI.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void GameOver()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        playerCam.GetComponent<ThirdPersonCam>().enabled = false;
        gameOverUI.SetActive(true);
    }
    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    public void MainMenu() => SceneManager.LoadScene("MainMenu");
    public void Quit() => Application.Quit();
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

}

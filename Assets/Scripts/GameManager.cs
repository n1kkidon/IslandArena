using Assets.Scripts.Saving;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject pauseMenuUI;
    public GameObject shopUI;

    public GameObject player;
    public GameObject playerCam;
    bool isPaused = false;
    public static bool shopOpen {get; private set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        LoadGame();
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
        if(Input.GetKeyDown(KeyCode.F1) && !gameOverUI.activeInHierarchy)
        {
            if (shopOpen)
                CloseShop();
            else OpenShop();
        }
        if(gameOverUI.activeInHierarchy || pauseMenuUI.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            if(!shopOpen)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void LoadGame()
    {
        var item = DataPersistenceManager.Instance;
        item.LoadGame();
    }
    public void SaveGame()
    {
        var item = DataPersistenceManager.Instance;
        item.SaveGame();
    }
    public void GameOver()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        playerCam.GetComponent<ThirdPersonCam>().enabled = false;
        gameOverUI.SetActive(true);
    }
    public void Restart() 
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SaveSystem.DeleteSaveFile();
    }
    public void RestartFromLastSave() 
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu() 
    {
        SaveGame();
        SceneManager.LoadScene("MainMenu"); 
    }
    public void Quit()
    {
        SaveGame();
        Application.Quit();
    }
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
    public void OpenShop()
    {
        shopOpen=true;
        shopUI.SetActive(true);
        player.GetComponentInChildren<PlayerMovement>().enabled = false;
        GameObject.Find("Camera").GetComponentInChildren<CinemachineBrain>().enabled=false;
    }
    public void CloseShop()
    { 
        shopOpen=false;
        shopUI.SetActive(false);
        player.GetComponentInChildren<PlayerMovement>().enabled = true;
        GameObject.Find("Camera").GetComponentInChildren<CinemachineBrain>().enabled = true;
    }
}

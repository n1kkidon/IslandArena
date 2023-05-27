using Assets.Scripts.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject pauseMenuUI;
    public GameObject skillTreeUI;

    public GameObject player;
    public GameObject playerCam;
    bool isPaused = false;
    bool isInSkillTrees = false;
    // Start is called before the first frame update
    void Start()
    {
        LoadGame();
    }


    void CloseUI()
    {
        if (isPaused)
        {
            ResumeGame();
            isPaused = false;
        }
        if (isInSkillTrees)
        {
            CloseSkillTree();
            isInSkillTrees = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOverUI.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isPaused && !isInSkillTrees)
                {
                    PauseGame();
                    isPaused=true;
                }
                else
                {
                    CloseUI();
                }

            }
            else if (Input.GetKeyDown(KeyCode.Tab) && !isPaused)
            {
                if (isInSkillTrees)
                    CloseSkillTree();
                else OpenSkillTree();
                isInSkillTrees = !isInSkillTrees;
            }
        }
       
        


        if(gameOverUI.activeInHierarchy || pauseMenuUI.activeInHierarchy || skillTreeUI.activeInHierarchy)
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
    public void OpenSkillTree()
    {
        skillTreeUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void CloseSkillTree()
    {
        skillTreeUI.SetActive(false);
        Time.timeScale = 1f;
    }
}

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
    public GameObject skillTreeUI;

    public GameObject player;
    public GameObject playerCam;
    bool isPaused = false;
    public static bool shopOpen {get; private set; } = false;
    bool isInSkillTrees = false;
    // Start is called before the first frame update
    void Start()
    {
        LoadGame();
    }
    void Awake()
    {
        skillTreeUI.SetActive(true);
        skillTreeUI.SetActive(false);
    }

    void CloseUI()
    {
        if (isPaused)
            ResumeGame();
        if (isInSkillTrees)
            CloseSkillTree();
        if (shopOpen)
            CloseShop();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOverUI.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isPaused && !isInSkillTrees && !shopOpen)
                    PauseGame();
                else
                    CloseUI();

            }
            else if (Input.GetKeyDown(KeyCode.Tab) && !isPaused && !shopOpen)
            {
                if (isInSkillTrees)
                    CloseUI();
                else OpenSkillTree();
            }
            else if (Input.GetKeyDown(KeyCode.F1) && !isPaused && !isInSkillTrees)
            {
                if (shopOpen)
                    CloseUI();
                else OpenShop();
            }
        }
        
        if(gameOverUI.activeInHierarchy || pauseMenuUI.activeInHierarchy || skillTreeUI.activeInHierarchy || shopUI.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
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
        isPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
    public void OpenSkillTree()
    {
        isInSkillTrees = true;
        skillTreeUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void CloseSkillTree()
    {
        isInSkillTrees = false;
        skillTreeUI.SetActive(false);
        Time.timeScale = 1f;
    }
}

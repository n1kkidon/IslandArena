using Assets.Scripts.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("TestScene");
        SaveSystem.DeleteSaveFile();
        Time.timeScale = 1f;
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("TestScene");
        Time.timeScale = 1f;
    }
    public void Options() => Debug.Log("there are no options as of yet!");
    public void Quit() => Application.Quit();
}

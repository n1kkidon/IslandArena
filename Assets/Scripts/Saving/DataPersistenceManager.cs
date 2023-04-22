using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Saving
{
    public class DataPersistenceManager : MonoBehaviour
    {
        private GameData gameData;
        private List<IDataPersistence> dataPersistenceList;
        public static DataPersistenceManager Instance { get; private set;}
        private void Awake()
        {
            if(Instance != null)
            {
                Debug.LogError("more than 1 data persistence manager");
            }
            Instance = this;
        }
     
        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            var items = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
            return items.ToList();
        }
        public void NewGame()
        {
            gameData = new GameData();
        }
        public void LoadGame()
        {
            dataPersistenceList ??= FindAllDataPersistenceObjects();
            gameData = SaveSystem.Load();
            if (gameData == null)
            {
                Debug.Log("No saved data found. Starting a new game.");
                NewGame();
                return; //can remove this if new game should set some default vaules
            }
            foreach(var item in dataPersistenceList)
                item.LoadData(gameData);     
        }
        public void SaveGame()
        {
            dataPersistenceList ??= FindAllDataPersistenceObjects();
            gameData ??= new GameData();
            foreach(var item in dataPersistenceList)
                item.SaveData(ref gameData);
            if (gameData.currentHealth <= 0)
                return;
            SaveSystem.Save(gameData);
        }

    }
}

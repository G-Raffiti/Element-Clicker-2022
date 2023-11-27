using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _SaveSystem.Data;
using UI;
using UnityEngine;

namespace _SaveSystem
{
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("File Storage Config")]
        [SerializeField] private string fileName;
        [SerializeField] private bool useEncryption;

        private GameData gameData;
        private List<IDataPersistence> dataPersistenceObjects;
        private FileDataHandler dataHandler;

        public static DataPersistenceManager instance { get; private set; }

        private void Awake() 
        {
            if (instance != null) 
            {
                Debug.LogError("Found more than one Data Persistence Manager in the scene.");
            }
            instance = this;
        }

        private void OnEnable()
        {
            OpeningScreen.eGameLoaded += () => StartCoroutine(AutoSave());
        }

        private void Start()
        {
            this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
            this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        }

        private IEnumerator AutoSave()
        {
            yield return new WaitForSeconds(60);
            SaveGame();
        }

        public void NewGame() 
        {
            this.gameData = new GameData();
            SaveGame();
            LoadGame();
        }

        public void LoadGame()
        {
            // load any saved data from a file using the data handler
            this.gameData = dataHandler.Load();
        
            // if no data can be loaded, initialize to a new game
            if (this.gameData == null) 
            {
                Debug.Log("No data was found. Initializing data to defaults.");
                NewGame();
                return;
            }

            // push the loaded data to all other scripts that need it
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
            {
                dataPersistenceObj.LoadData(gameData);
            }
        }

        public void SaveGame()
        {
            // pass the data to other scripts so they can update it
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
            {
                dataPersistenceObj.SaveData(gameData);
            }

            // save that data to a file using the data handler
            dataHandler.Save(gameData);
        }

        private void OnApplicationQuit() 
        {
            SaveGame();
        }

        private List<IDataPersistence> FindAllDataPersistenceObjects() 
        {
            List<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
                .OfType<IDataPersistence>().ToList();

            return dataPersistenceObjects;
        }

        public void SaveLoadButton()
        {
            SaveGame();
            LoadGame();
        }
    }
}

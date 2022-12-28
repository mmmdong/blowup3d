using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MondayOFF;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    #region Singleton
    public static DataManager instance;
    public PlayerData player;

    [HideInInspector] public string path;
    [HideInInspector] public bool isCalling;
    public bool isDataExist;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        if (File.Exists(Path.Combine(Application.persistentDataPath, "Load.json")))
        {
            isDataExist = true;
            Load_Data();
            if (!player.first)
            {
                EventTracker.ClearStage(0);
                EventTracker.TryStage(1);
                player.first = true;
                Save_Data();
            }
        }
    }

    #endregion

#if UNITY_EDITOR || UNITY_STANDALONE
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Delete();
            SceneManager.LoadSceneAsync(0);
            DestroyImmediate(GameManager.instance);
            DestroyImmediate(this);
        }
    }
#endif


#if UNITY_EDITOR

    private void OnApplicationQuit()
    {
        Save_Data();
    }
#elif !UNITY_EDITOR

 private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            Save_Data();
    }

#endif

    [ContextMenu("Delete")]
    private void Delete()
    {
        path = Path.Combine(Application.persistentDataPath, "Load.json");
        File.Delete(path);
    }

    public void Save_Data()
    {
        string jsonData = JsonUtility.ToJson(player, true);
        path = Path.Combine(Application.persistentDataPath, "Load.json");
        File.WriteAllText(path, jsonData);
    }

    public void Load_Data()
    {
        path = Path.Combine(Application.persistentDataPath, "Load.json");
        string jsondata = File.ReadAllText(path);
        player = JsonUtility.FromJson<PlayerData>(jsondata);

        GameManager.instance._currentCurrency.Value = player.money;
        
        GameManager.instance._bestGrade = player.bestGrade;
    }


    [Serializable]
    public class PlayerData
    {
        public bool first;
        public int level;
        public int bestGrade;
        public int blowCount;
        public long money;
        public int liveBalls;
        public int[] liveBallGrade;

        public Buttons buttons;
    }

    [Serializable]
    public class Buttons
    {
        public ButtonDatas btnAddBallData;
        public ButtonDatas btnBallCountData;
        public ButtonDatas btnLevUpData;
        public ButtonDatas btnMergeBallData;
    }

    [Serializable]
    public class ButtonDatas
    {
        public long buttonLev = 1;
        public long costValue;
    }
}

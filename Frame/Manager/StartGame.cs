using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{

    //public static StartGame instance;
    public bool IsStartGameScene = false;//预留切换开始场景
    GameApp gameapp;
    private void Awake()
    {

        //instance = this;
        gameapp = UnitySingleton<GameApp>.Instance;
        gameapp.InstantGameEnter();

        InitGameData();
        gameapp.ReadFile();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitGameData()
    {
        if (SceneManager.GetActiveScene().name != SetConfig.sceneRole)
        {
            SceneManager.LoadSceneAsync(SetConfig.sceneRole);
            // SceneManager.LoadScene(SetConfig.sceneRole);
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using CWinterTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WinterTools;

namespace Demo
{
    public class StartGame : MonoBehaviour
    {
        //public static StartGame instance;
        public bool IsStartGameScene = false; //预留切换开始场景
        GameApp gameapp;

        private void Awake()
        {

            //instance = this;
            gameapp = UnitySingleton<GameApp>.Instance;
            gameapp.InstantGameEnter();

            InitGameData();
            gameapp.ReadFile();
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
}
using System;
using Main.Application.Managers;
using Main.Presentation;
using Main.Presentation.Initializers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Application
{
    public class GameSceneController: MonoBehaviour
    {
        #region Controllers

        [Header("Controllers")]
        [SerializeField]
        private HandController _handController;
        
        [SerializeField]
        private UIController _uiController;

        #endregion
        
        #region Unity: Awake | Start | OnDestroy

        private void OnEnable()
        {
            ManagerContainer.Instance.Init();
        }

        private void Start()
        {
            ManagerContainer.Instance.Launch();
            
            _handController.Init();
            
            HandInitializer.Instance.CreateHand();
            
            _uiController.Init();
            
            AddEvents();
        }

        private void OnDestroy()
        {
            RemoveEvents();
            
            _uiController.DeInit();
            
            _handController.DeInit();
            
            ManagerContainer.Instance.DeInit();
        }

        #endregion
        
        
        #region App: Enter | Exit

        private void EnterApp()
        {
            ManagerContainer.Instance.ReBegin();
        }

        private void ExitApp(bool isQuit)
        {
            
        }

        #endregion
        

        #region Unity: OnApplicationPause | OnApplicationQuit

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                ExitApp(false);
            }
            else
            {
                EnterApp();
            }
        }

        private void OnApplicationQuit()
        {
            ExitApp(true);
        }

        #endregion

        
        #region Event: OnRestartBtnClicked

        private void OnRestartBtnClicked(object sender, EventArgs e)
        {
            if (!HandInitializer.Instance.DidHandCreated)
            {
                return;
            }
            
            SceneManager.LoadScene("GameScene");
        }

        #endregion

        #region Events: Add | Remove

        private void AddEvents()
        {
            _uiController.RestartBtnClickedEvent += OnRestartBtnClicked;
        }

        private void RemoveEvents()
        {
            _uiController.RestartBtnClickedEvent -= OnRestartBtnClicked;
        }
        
        #endregion
    }
}
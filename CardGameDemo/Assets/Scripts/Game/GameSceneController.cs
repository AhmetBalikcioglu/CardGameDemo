using System;
using Main.Game.Application.Managers;
using Main.Game.Presentation;
using Main.Game.Presentation.Initializers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Game
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
            Init();
        }

        private void Start()
        {
            Launch();
        }

        #endregion

        
        #region Init

        private void Init()
        {
            ManagerContainer.Instance.Init();
        }

        #endregion
        
        #region Launch

        private void Launch()
        {
            ManagerContainer.Instance.Launch();
            
            _handController.Init();
            
            HandInitializer.Instance.CreateHand();
            
            _uiController.Init();
            
            AddEvents();
        }

        #endregion

        #region DeInit

        private void DeInit()
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
            if (isQuit)
            {
                DeInit();
            }
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
            
            _handController.ResetHand();
            ManagerContainer.Instance.Hand.CreateHand();
            HandInitializer.Instance.CreateHand();
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
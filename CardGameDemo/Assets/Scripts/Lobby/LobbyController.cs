using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Main.Lobby
{
    public class LobbyController: MonoBehaviour
    {
        #region Childs

        [Header("Buttons")]
        [SerializeField]
        private Button _btnPlay;

        #endregion

        #region Unity: Start

        private void Start()
        {
            AddEvents();
        }

        #endregion

        #region Unity: OnDestroy

        private void OnDestroy()
        {
            RemoveEvents();
        }

        #endregion

        
        #region LoadGame: StartSequence

        private void StartLoadGameSequence()
        {
            SceneManager.LoadSceneAsync("GameScene");
        }

        #endregion

        
        #region Event: OnBtnPlayClicked

        private void OnBtnPlayClicked()
        {
            StartLoadGameSequence();
        }

        #endregion
        
        #region Events: Add | Remove

        private void AddEvents()
        {
            _btnPlay.onClick.AddListener(OnBtnPlayClicked);
        }

        private void RemoveEvents()
        {
            _btnPlay.onClick.RemoveListener(OnBtnPlayClicked);
        }

        #endregion
    }
}
using System;
using Main.Game.Application.Managers;
using Main.Game.Data;
using Main.Game.Presentation.Initializers;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Game.Presentation
{
    public class UIController : MonoBehaviour
    {
        #region Events

        public event EventHandler RestartBtnClickedEvent;

        #endregion

        #region Childs

        [Header("Buttons")]
        [SerializeField]
        private Button _btnAscending;

        [SerializeField]
        private Button _btnSimilar;
        
        [SerializeField]
        private Button _btnSmart;

        [SerializeField]
        private Button _btnRestart;

        #endregion


        #region Init

        public void Init()
        {
            AddEvents();
        }

        #endregion

        #region DeInit

        public void DeInit()
        {
            RemoveEvents();
        }

        #endregion

        
        #region Event: OnBtnAscendingClicked

        private void OnBtnAscendingClicked()
        {
            if (!HandInitializer.Instance.DidHandCreated)
            {
                return;
            }

            ManagerContainer.Instance.Hand.ChangeHandOrder(HandOrder.Ascending);
        }

        #endregion

        #region Event: OnBtnSimilarClicked

        private void OnBtnSimilarClicked()
        {
            if (!HandInitializer.Instance.DidHandCreated)
            {
                return;
            }

            ManagerContainer.Instance.Hand.ChangeHandOrder(HandOrder.Similar);
        }

        #endregion
        
        #region Event: OnBtnSmartClicked

        private void OnBtnSmartClicked()
        {
            if (!HandInitializer.Instance.DidHandCreated)
            {
                return;
            }

            ManagerContainer.Instance.Hand.ChangeHandOrder(HandOrder.Smart);
        }

        #endregion
        
        #region Event: OnBtnRestartClicked

        private void OnBtnRestartClicked()
        {
            RestartBtnClickedEvent?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Events: Add | Remove

        private void AddEvents()
        {
            _btnAscending.onClick.AddListener(OnBtnAscendingClicked);
            _btnSimilar.onClick.AddListener(OnBtnSimilarClicked);
            _btnSmart.onClick.AddListener(OnBtnSmartClicked);
            _btnRestart.onClick.AddListener(OnBtnRestartClicked);
        }

        private void RemoveEvents()
        {
            _btnAscending.onClick.RemoveListener(OnBtnAscendingClicked);
            _btnSimilar.onClick.RemoveListener(OnBtnSimilarClicked);
            _btnSmart.onClick.RemoveListener(OnBtnSmartClicked);
            _btnRestart.onClick.RemoveListener(OnBtnRestartClicked);
        }

        #endregion
    }
}
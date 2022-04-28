using System;
using UnityEngine;

namespace Lib.Mvc
{
    public abstract class CView<TModel> : MonoBehaviour
        where TModel:CModel
    {
        #region Events

        public event EventHandler OnShowEvent;
        public event EventHandler OnHideEvent;

        #endregion
        
        #region Model

        protected TModel _model;

        #endregion
        
        #region Visible

        protected bool _visible;
        public bool Visible => _visible;

        #endregion
        
        
        #region Model: Set

        public void SetModel(TModel model)
        {
            _model = model;
        }

        #endregion
        
        
        #region Show -> OnShow -> ShowComplete

        internal void Show()
        {
            _visible = true;
            
            gameObject.SetActive(true);

            OnShow();
            ShowComplete();
            
            OnAddEvents();
        }

        protected abstract void OnShow();

        private void ShowComplete()
        {
            OnShowEvent?.Invoke(this, EventArgs.Empty);
        }
        
        #endregion

        #region Refresh -> OnRefresh

        public void Refresh()
        {
            OnRefresh();
        }

        protected virtual void OnRefresh()
        {
            //
        }

        #endregion
        
        #region Hide -> OnHide -> HideComplete

        internal void Hide()
        {
            OnHide();
            HideComplete();
        }

        protected abstract void OnHide();

        private void HideComplete()
        {
            OnRemoveEvents();
            
            _visible = false;
            
            gameObject.SetActive(false);

            OnHideEvent?.Invoke(this, EventArgs.Empty);
            
            _model = null;
        }
        
        #endregion

        
        #region Events: OnAdd | OnRemove

        protected abstract void OnAddEvents();
        protected abstract void OnRemoveEvents();

        #endregion
    }
}
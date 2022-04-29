using System;
using DG.Tweening;
using Lib.Mvc;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace Main.Game.Presentation
{
    public class CardView: CView<CardModel>
    {
        #region Events

        public event EventHandler OnCardSelectedEvent;
        public event EventHandler OnCardReleasedEvent;
        
        #endregion

        #region Childs

        [Header("SpriteRenderers")]
        [SerializeField]
        private SpriteRenderer _srCard;
        
        #endregion

        #region OnShow | OnHide

        protected override void OnShow()
        {
            _srCard.sprite = ResourceProvider.GetCardSprite(_model.Info.Type, _model.Info.Value);
            gameObject.name = $"{_model.Info.Type}_{_model.Info.Value}";
        }

        protected override void OnHide()
        {
            
        }
        
        #endregion

        
        #region Set: PositionAndRotation
        
        public void SetPositionAndRotation(Vector3 position, Vector3 rotation)
        {
            var oldRotation = transform.eulerAngles;
            transform.up = rotation;
            
            var newRotation = transform.eulerAngles;
            transform.eulerAngles = oldRotation;
            
            transform.DOMove(position, 0.5f);
            transform.DORotate(newRotation, 0.5f);
        }
        
        #endregion
        
        
        #region Event: Unity: OnMouseUp
        
        private void OnMouseDown()
        {
            OnCardSelectedEvent?.Invoke(this, EventArgs.Empty);
        }

        #endregion
        
        #region Event: Unity: OnMouseUp

        private void OnMouseUp()
        {
            OnCardReleasedEvent?.Invoke(this, EventArgs.Empty);
        }
        
        #endregion

        #region Events: Add | Remove

        protected override void OnAddEvents()
        {
            
        }

        protected override void OnRemoveEvents()
        {
            
        }

        #endregion
    }
}
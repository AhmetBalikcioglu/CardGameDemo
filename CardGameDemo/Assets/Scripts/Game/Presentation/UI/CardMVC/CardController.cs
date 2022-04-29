using System;
using Lib.Mvc;
using UnityEngine;

namespace Main.Game.Presentation
{
    public class CardController: CController<CardModel, CardView>
    {
        #region Events

        public event EventHandler<CardSelectedEventArgs> OnCardSelectedEvent;
        public event EventHandler<CardReleasedEventArgs> OnCardReleasedEvent;

        #endregion

        #region OnShow | OnHide

        protected override void OnShow()
        {
            
        }

        protected override void OnHide()
        {
            
        }

        #endregion
        

        #region Set: PositionAndRotation

        public void SetPositionAndRotation(Vector3 position, Vector3 rotation)
        { 
            _view.SetPositionAndRotation(position, rotation);   
        }

        #endregion
        

        #region Event: OnCardSelected

        private void OnCardSelected(object sender, EventArgs e)
        {
            OnCardSelectedEvent?.Invoke(this, new CardSelectedEventArgs()
            {
                CardController = this
            });
        }
        
        #endregion

        #region Event: OnCardReleased

        private void OnCardReleased(object sender, EventArgs e)
        {
            OnCardReleasedEvent?.Invoke(this, new CardReleasedEventArgs()
            {
                CardController = this
            });
        }

        #endregion

        #region Events: OnAdd | OnRemove

        protected override void OnAddEvents()
        {
            _view.OnCardSelectedEvent += OnCardSelected;
            _view.OnCardReleasedEvent += OnCardReleased;
        }

        protected override void OnRemoveEvents()
        {
            _view.OnCardSelectedEvent -= OnCardSelected;
            _view.OnCardReleasedEvent -= OnCardReleased;
        }

        #endregion
    }
}
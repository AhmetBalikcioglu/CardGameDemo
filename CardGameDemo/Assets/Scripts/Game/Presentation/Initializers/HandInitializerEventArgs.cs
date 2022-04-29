using System;

namespace Main.Game.Presentation.Initializers
{
    public class CardCreatedEventArgs : EventArgs
    {
        public CardView View { get; set; }
    }

}
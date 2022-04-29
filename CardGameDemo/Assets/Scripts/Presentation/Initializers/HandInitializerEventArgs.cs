using System;

namespace Main.Presentation.Initializers
{
    public class CardCreatedEventArgs : EventArgs
    {
        public CardView View { get; set; }
    }

}
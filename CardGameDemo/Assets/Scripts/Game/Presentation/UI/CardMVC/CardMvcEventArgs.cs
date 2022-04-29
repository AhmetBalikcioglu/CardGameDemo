using System;

namespace Main.Game.Presentation
{
    public class CardSelectedEventArgs : EventArgs
    {
        public CardController CardController { get; set; }
    }
    
    public class CardReleasedEventArgs : EventArgs
    {
        public CardController CardController { get; set; }
    }
}
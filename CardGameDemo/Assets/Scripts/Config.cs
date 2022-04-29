using System.Collections.Generic;
using Main.Data;

namespace Main
{
    public static class Config
    {
        #region Hand

        public const int TotalHandCount = 11;

        #endregion
        
        #region Deck

        public static readonly Dictionary<CardType, List<CardValue>> DeckConfigDictionary = new Dictionary<CardType, List<CardValue>>()
        {
            {
                CardType.Spades, new List<CardValue>()
                {
                    CardValue.Ace,
                    CardValue.Two,
                    CardValue.Three,
                    CardValue.Four,
                    CardValue.Five,
                    CardValue.Six,
                    CardValue.Seven,
                    CardValue.Eight,
                    CardValue.Nine,
                    CardValue.Ten,
                    CardValue.Jack,
                    CardValue.Queen,
                    CardValue.King
                }
            },
            {
                CardType.Diamonds, new List<CardValue>()
                {
                    CardValue.Ace,
                    CardValue.Two,
                    CardValue.Three,
                    CardValue.Four,
                    CardValue.Five,
                    CardValue.Six,
                    CardValue.Seven,
                    CardValue.Eight,
                    CardValue.Nine,
                    CardValue.Ten,
                    CardValue.Jack,
                    CardValue.Queen,
                    CardValue.King
                }
            },
            {
                CardType.Hearts, new List<CardValue>()
                {
                    CardValue.Ace,
                    CardValue.Two,
                    CardValue.Three,
                    CardValue.Four,
                    CardValue.Five,
                    CardValue.Six,
                    CardValue.Seven,
                    CardValue.Eight,
                    CardValue.Nine,
                    CardValue.Ten,
                    CardValue.Jack,
                    CardValue.Queen,
                    CardValue.King
                }
            },
            {
                CardType.Clubs, new List<CardValue>()
                {
                    CardValue.Ace,
                    CardValue.Two,
                    CardValue.Three,
                    CardValue.Four,
                    CardValue.Five,
                    CardValue.Six,
                    CardValue.Seven,
                    CardValue.Eight,
                    CardValue.Nine,
                    CardValue.Ten,
                    CardValue.Jack,
                    CardValue.Queen,
                    CardValue.King
                }
            }
        };

        #endregion
    }
}
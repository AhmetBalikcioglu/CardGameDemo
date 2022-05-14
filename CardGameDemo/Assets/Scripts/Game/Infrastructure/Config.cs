using System.Collections.Generic;
using Main.Game.Data;

namespace Main.Game
{
    public static class Config
    {
        #region Hand

        public const int TotalHandCount = 11;

        public const int CircleRadiusForArc = 20;
        public const float CircleYPositionForArc = -23f;
        public const float DegreeBetweenCardsForArc = 0.05f;
        
        #region Test - Case1

        public static readonly List<CardInfo> HandTestCase1ConfigList = new List<CardInfo>()
        {
            { new CardInfo() { Type = CardType.Hearts, Value = CardValue.Ace } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Two } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Five } },
            { new CardInfo() { Type = CardType.Hearts, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Ace } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Three } },
            { new CardInfo() { Type = CardType.Clubs, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Ace } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Three } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Four } }
        };

        #endregion
        
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
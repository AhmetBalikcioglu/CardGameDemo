using System.Collections.Generic;
using Codice.Client.BaseCommands;
using Main.Game.Data;
using UnityEngine;

namespace Main.Game
{
    public static class Config
    {
        #region Screen

        public static readonly Vector2 REF_RESOLUTION = new Vector2(1920f, 1080f);
        public static readonly Vector2 REF_BOUNDS = REF_RESOLUTION / 100f;

        #endregion

        #region Hand

        public const int TotalHandCount = 11;

        #region UI

        public const int CircleRadiusForArc = 20;
        public const float CircleYPositionForArc = -23f;
        public const float DegreeBetweenCardsForArc = 0.05f;
        
        #endregion

        #region Test

        public static readonly Dictionary<TestCase, List<CardInfo>> HandTestCaseDictionary = new Dictionary<TestCase, List<CardInfo>>()
        {
            {
                TestCase.Case1, HandTestCase1ConfigList
            },
            {
                TestCase.Case2, HandTestCase2ConfigList
            }
        };

        #region Test - Case1

        public static List<CardInfo> HandTestCase1ConfigList => new List<CardInfo>()
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
        
        #region Test - Case2

        public static List<CardInfo> HandTestCase2ConfigList => new List<CardInfo>()
        {
            { new CardInfo() { Type = CardType.Hearts, Value = CardValue.Three } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Two } },
            { new CardInfo() { Type = CardType.Hearts, Value = CardValue.Five } },
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
using System.Collections.Generic;
using Main.Game;
using Main.Game.Data;
using NUnit.Framework;

public class TestCaseTests
{
    [Test]
    public void TestCase1Config()
    {
        var handTestCase1ConfigList = new List<CardInfo>()
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
        
        CollectionAssert.AreEquivalent(handTestCase1ConfigList, Config.HandTestCase1ConfigList);
    }
}
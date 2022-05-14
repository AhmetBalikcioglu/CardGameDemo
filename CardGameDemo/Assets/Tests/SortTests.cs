using System.Collections.Generic;
using Main.Game;
using Main.Game.Application.Managers;
using Main.Game.Data;
using NUnit.Framework;

public class SortTests
{
    #region TestCase1: AscendingOrdered

    [Test]
    public void TestCase1AscendingOrdered()
    {
        var hand = new List<CardInfo>(Config.HandTestCase1ConfigList);

        var handManager = new HandManager();
        
        var properAscendingGroupsList = handManager.GetProperAscendingGroups(hand);

        for (int i = properAscendingGroupsList.Count - 1; i >= 0 ; i--)
        {
            handManager.SetProperGroupsToHand(properAscendingGroupsList[i], hand);
        }
        
        var handTestCase1AscendingOrdered = new List<CardInfo>()
        { 
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Ace } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Two } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Three } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Four } }, 
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Three } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Five } },
            { new CardInfo() { Type = CardType.Hearts, Value = CardValue.Ace } },
            { new CardInfo() { Type = CardType.Hearts, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Clubs, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Ace } }
        };
        
        CollectionAssert.AreEqual(handTestCase1AscendingOrdered, hand);
    }

    #endregion

    #region TestCase1: SimilarOrdered

    [Test]
    public void TestCase1SimilarOrdered()
    {
        var hand = new List<CardInfo>(Config.HandTestCase1ConfigList);

        var handManager = new HandManager();
        
        var properSimilarGroupsList = handManager.GetProperSimilarGroups(hand);

        for (int i = properSimilarGroupsList.Count - 1; i >= 0 ; i--)
        {
            handManager.SetProperGroupsToHand(properSimilarGroupsList[i], hand);
        }
        
        var handTestCase1SimilarOrdered = new List<CardInfo>()
        {
            { new CardInfo() { Type = CardType.Hearts, Value = CardValue.Ace } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Ace } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Ace } },
            { new CardInfo() { Type = CardType.Hearts, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Clubs, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Two } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Five } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Three } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Three } }
        };
        
        CollectionAssert.AreEqual(handTestCase1SimilarOrdered, hand);
    }

    #endregion
}
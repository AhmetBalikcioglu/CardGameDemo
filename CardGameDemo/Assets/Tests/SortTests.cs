using System.Collections.Generic;
using Main.Game.Application.Managers;
using Main.Game.Data;
using NUnit.Framework;

public class SortTests
{
    #region TestCase1: AscendingOrdered

    [Test]
    public void TestCase1AscendingOrdered()
    {
        var handManager = new HandManager();
        
        handManager.CreateTestCaseHand(TestCase.Case1);
        handManager.ChangeHandOrder(HandOrder.Ascending);

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
        
        CollectionAssert.AreEqual(handTestCase1AscendingOrdered, handManager.Hand);
    }

    #endregion

    #region TestCase1: SimilarOrdered

    [Test]
    public void TestCase1SimilarOrdered()
    {
        var handManager = new HandManager();
        
        handManager.CreateTestCaseHand(TestCase.Case1);
        handManager.ChangeHandOrder(HandOrder.Similar);

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
        
        CollectionAssert.AreEqual(handTestCase1SimilarOrdered, handManager.Hand);
    }

    #endregion
    
    #region TestCase1: SmartOrdered

    [Test]
    public void TestCase1SmartOrdered()
    {
        var handManager = new HandManager();
        
        handManager.CreateTestCaseHand(TestCase.Case1);
        handManager.ChangeHandOrder(HandOrder.Smart);

        var handTestCase1SmartOrdered = new List<CardInfo>()
        {
            { new CardInfo() { Type = CardType.Hearts, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Clubs, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Three } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Five } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Ace } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Two } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Three } },
            { new CardInfo() { Type = CardType.Hearts, Value = CardValue.Ace } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Ace } }
        };
        
        CollectionAssert.AreEqual(handTestCase1SmartOrdered, handManager.Hand);
    }

    #endregion
    
    #region TestCase2: AscendingOrdered

    [Test]
    public void TestCase2AscendingOrdered()
    {
        var handManager = new HandManager();
        
        handManager.CreateTestCaseHand(TestCase.Case2);
        handManager.ChangeHandOrder(HandOrder.Ascending);

        var handTestCase2AscendingOrdered = new List<CardInfo>()
        { 
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Ace } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Two } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Three } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Four } }, 
            { new CardInfo() { Type = CardType.Hearts, Value = CardValue.Three } },
            { new CardInfo() { Type = CardType.Hearts, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Hearts, Value = CardValue.Five } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Three } },
            { new CardInfo() { Type = CardType.Clubs, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Ace } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Four } },
        };
        
        CollectionAssert.AreEqual(handTestCase2AscendingOrdered, handManager.Hand);
    }

    #endregion
    
    #region TestCase2: SimilarOrdered

    [Test]
    public void TestCase2SimilarOrdered()
    {
        var handManager = new HandManager();
        
        handManager.CreateTestCaseHand(TestCase.Case2);
        handManager.ChangeHandOrder(HandOrder.Similar);

        var handTestCase2SimilarOrdered = new List<CardInfo>()
        {
            { new CardInfo() { Type = CardType.Hearts, Value = CardValue.Three } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Three } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Three } },
            { new CardInfo() { Type = CardType.Hearts, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Clubs, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Two } },
            { new CardInfo() { Type = CardType.Hearts, Value = CardValue.Five } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Ace } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Ace } }
        };
        
        CollectionAssert.AreEqual(handTestCase2SimilarOrdered, handManager.Hand);
    }

    #endregion
    
    #region TestCase2: SmartOrdered

    [Test]
    public void TestCase2SmartOrdered()
    {
        var handManager = new HandManager();
        
        handManager.CreateTestCaseHand(TestCase.Case2);
        handManager.ChangeHandOrder(HandOrder.Smart);

        var handTestCase2SmartOrdered = new List<CardInfo>()
        {
            { new CardInfo() { Type = CardType.Clubs, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Hearts, Value = CardValue.Three } },
            { new CardInfo() { Type = CardType.Hearts, Value = CardValue.Four } },
            { new CardInfo() { Type = CardType.Hearts, Value = CardValue.Five } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Ace } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Two } },
            { new CardInfo() { Type = CardType.Spades, Value = CardValue.Three } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Three } },
            { new CardInfo() { Type = CardType.Diamonds, Value = CardValue.Ace } }
        };
        
        CollectionAssert.AreEqual(handTestCase2SmartOrdered, handManager.Hand);
    }

    #endregion
}
using System.Linq;
using Main.Game;
using Main.Game.Data;
using NUnit.Framework;

public class DeckTests
{
    [Test]
    public void DeckSize()
    {
        var totalCardCount = Config.DeckConfigDictionary.Values.Sum(cardValueList => cardValueList.Count);
        Assert.AreEqual(52, totalCardCount);
    }

    [Test]
    public void DeckHasSpades()
    {
        Assert.AreEqual(true, Config.DeckConfigDictionary.ContainsKey(CardType.Spades));
    }
    
    [Test]
    public void DeckHasDiamonds()
    {
        Assert.AreEqual(true, Config.DeckConfigDictionary.ContainsKey(CardType.Diamonds));
    }
    
    [Test]
    public void DeckHasHearts()
    {
        Assert.AreEqual(true, Config.DeckConfigDictionary.ContainsKey(CardType.Hearts));
    }
    
    [Test]
    public void DeckHasClubs()
    {
        Assert.AreEqual(true, Config.DeckConfigDictionary.ContainsKey(CardType.Clubs));
    }
}
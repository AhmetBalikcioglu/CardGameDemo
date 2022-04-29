using System;
using System.Collections.Generic;
using System.Linq;
using Lib.Manager;
using Main.Game.Application.Managers.Comparer;
using Main.Game.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main.Game.Application.Managers
{
    public class HandManager: CManager
    {
        #region Events

        public event EventHandler HandOrderUpdatedEvent;

        #endregion

        #region Field
        
        private List<CardInfo> _fullDeck;
        
        private List<CardInfo> _hand;
        public List<CardInfo> Hand => _hand;

        private HandOrder _handOrder;
        public HandOrder HandOrder => _handOrder;

        #endregion

        #region OnBuild | OnBegin | OnReBegin | OnRegister | OnUnRegister | OnEnd

        protected override void OnBuild()
        {
            CreateFullDeck();
        }

        protected override void OnBegin()
        {
            CreateRandomHand();
        }

        protected override void OnReBegin()
        {
            
        }

        protected override void OnRegister()
        {
            
        }

        protected override void OnUnRegister()
        {
            
        }

        protected override void OnEnd()
        {
            
        }
        
        #endregion
        
        
        #region Deck: CreateFull

        private void CreateFullDeck()
        {
            _fullDeck = new List<CardInfo>();
            
            foreach (var deckPair in Config.DeckConfigDictionary)
            {
                foreach (var cardValue in deckPair.Value)
                {
                    var data = new CardInfo()
                    {
                        Type = deckPair.Key,
                        Value = cardValue
                    };
                    _fullDeck.Add(data);
                }
            }
        }

        #endregion

        #region Hard: CreateRandom

        private void CreateRandomHand()
        {
            _handOrder = HandOrder.None;
            
            _hand = new List<CardInfo>();
            for (int i = 0; i < Config.TotalHandCount; i++)
            {
                var randomCard = _fullDeck[Random.Range(0, _fullDeck.Count)];

                while (_hand.Contains(randomCard))
                {
                    randomCard = _fullDeck[Random.Range(0, _fullDeck.Count)];
                }
                _hand.Add(randomCard);
            }
        }

        #endregion

        
        #region Hand: ChangeOrder

        public void ChangeHandOrder(HandOrder order)
        {
            switch (order)
            {
                case HandOrder.Ascending:
                    SetHandToAscendingOrdered();
                    break;
                case HandOrder.Similar:
                    SetHandToSimilarOrdered();
                    break;
            }
            
            HandOrderUpdatedEvent?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        
        #region Hand: SetToAscendingOrdered

        private void SetHandToAscendingOrdered()
        {
            var properAscendingGroupsList = GetProperAscendingGroups();

            foreach (var properAscendingGroup in properAscendingGroupsList)
            {
                SetProperGroupsToHand(properAscendingGroup);
            }

            _handOrder = HandOrder.Ascending;
        }
        
        #endregion

        #region Hand: SetToSimilarOrdered

        private void SetHandToSimilarOrdered()
        {
            var properSimilarGroupsList = GetProperSimilarGroups();
            
            foreach (var properSimilarGroup in properSimilarGroupsList)
            {
                SetProperGroupsToHand(properSimilarGroup);
            }
            
            _handOrder = HandOrder.Similar;
        }

        #endregion

        #region Hand: SetToSmartOrdered
        
        // Just a tryout
        private void SetHandToSmartOrdered()
        {
            var properAscendingGroupsList = GetProperAscendingGroups();
            var properSimilarGroupsList = GetProperSimilarGroups();
        
            var collidedGroupsInfoDictionary = new SortedDictionary<List<CardInfo>, CollidedGroupInfo>(new DescendingGroupComparer());
            foreach (var properAscendingGroup in properAscendingGroupsList)
            {
                var collidedGroups = new List<List<CardInfo>>();
                var collidedCards = new List<CardInfo>();
                foreach (var properSimilarGroup in properSimilarGroupsList)
                {
                    foreach (var card in properAscendingGroup)
                    {
                        if (properSimilarGroup.Contains(card))
                        {
                            collidedCards.Add(card);
                            collidedGroups.Add(properSimilarGroup);
                            break;
                        }
                    }
                }
        
                if (collidedGroups.Any())
                {
                    collidedGroupsInfoDictionary.Add(properAscendingGroup, new CollidedGroupInfo(properAscendingGroup, collidedGroups, collidedCards));
                }
            }
            
            foreach (var properSimilarGroup in properSimilarGroupsList)
            {
                var collidedGroups = new List<List<CardInfo>>();
                var collidedCards = new List<CardInfo>();
                foreach (var properAscendingGroup in properAscendingGroupsList)
                {
                    foreach (var card in properSimilarGroup)
                    {
                        if (properAscendingGroup.Contains(card))
                        {
                            collidedCards.Add(card);
                            collidedGroups.Add(properAscendingGroup);
                            break;
                        }
                    }
                }
        
                if (collidedGroups.Any())
                {
                    collidedGroupsInfoDictionary.Add(properSimilarGroup, new CollidedGroupInfo(properSimilarGroup, collidedGroups, collidedCards));
                }
            }
        
            var resolvedGroupsList = new List<List<CardInfo>>();
        
            foreach (var collidedGroupInfo in collidedGroupsInfoDictionary)
            {
                if (!IsGroupValid(collidedGroupInfo.Key))
                {
                    RemoveCardGroupFromEverywhere(collidedGroupInfo.Key, ref collidedGroupsInfoDictionary);
                    continue;
                }
        
                if (collidedGroupInfo.Value.CollidedCardGroups.Count == 0)
                {
                    resolvedGroupsList.Add(collidedGroupInfo.Key);
                    continue;
                }
                
                if (collidedGroupInfo.Key.Count == 3)
                {
                    foreach (var collidedCardGroup in collidedGroupInfo.Value.CollidedCardGroups)
                    {
                        foreach (var cardInfo in collidedGroupInfo.Value.CollidedCardsList)
                        {
                            collidedCardGroup.Remove(cardInfo);
                            collidedGroupsInfoDictionary[collidedCardGroup].CollidedCardsList.Remove(cardInfo);
                        }
                        collidedGroupsInfoDictionary[collidedCardGroup].CollidedCardGroups.Remove(collidedCardGroup);
        
                        if (!IsGroupValid(collidedCardGroup))
                        {
                            RemoveCardGroupFromEverywhere(collidedCardGroup, ref collidedGroupsInfoDictionary);
                        }
                        else
                        {
                            var newCollideInfo = new CollidedGroupInfo(collidedGroupsInfoDictionary[collidedCardGroup].Group, collidedGroupsInfoDictionary[collidedCardGroup].CollidedCardGroups, collidedGroupsInfoDictionary[collidedCardGroup].CollidedCardsList);
                            var newGroup = new List<CardInfo>(collidedCardGroup);
                            collidedGroupsInfoDictionary.Remove(collidedCardGroup);
                            collidedGroupsInfoDictionary.Add(newGroup, newCollideInfo);
                        }
                    }
                    
                    resolvedGroupsList.Add(collidedGroupInfo.Key);
                    continue;
                }
                
                foreach (var collidedCards in collidedGroupInfo.Value.CollidedCardsList)
                {
                    //1-2-3-4 4-4-4 Scenario                    
                }
            }
        
            var properSmartGroupsList = new List<List<CardInfo>>();
        
            foreach (var properAscendingGroup in properAscendingGroupsList)
            {
                if (!collidedGroupsInfoDictionary.ContainsKey(properAscendingGroup))
                {
                    properSmartGroupsList.Add(properAscendingGroup);
                }
            }
            
            foreach (var properSimilarGroup in properSimilarGroupsList)
            {
                if (!collidedGroupsInfoDictionary.ContainsKey(properSimilarGroup))
                {
                    properSmartGroupsList.Add(properSimilarGroup);
                }
            }
        }
        
        private void RemoveCardGroupFromEverywhere(List<CardInfo> group, ref SortedDictionary<List<CardInfo>, CollidedGroupInfo> dictionary)
        {
            foreach (var collidedCardGroup in dictionary[group].CollidedCardGroups)
            {
                dictionary[collidedCardGroup].CollidedCardGroups.Remove(group);
                foreach (var cardInfo in group)
                {
                    dictionary[collidedCardGroup].CollidedCardsList.Remove(cardInfo);
                }
            }
            dictionary.Remove(group);
        }
        
        private int GetGroupTotalValue(List<CardInfo> cardGroup)
        {
            int totalValue = 0;
            foreach (var card in cardGroup)
            {
                var value = (int)card.Value;
                if (value > 10)
                {
                    value = 10;
                }
                totalValue += value;
            }
        
            return totalValue;
        }
        
        private bool IsGroupValid(List<CardInfo> group)
        {
            if (group.Count < 3)
            {
                return false;
            }
        
            var isValidForAscending = false;
            for (int i = 1; i < group.Count; i++)
            {
                if (group[i].Value != group[i - 1].Value)
                {
                    isValidForAscending = false;
                    break;
                }
                isValidForAscending = true;
            }
            var isValidForSimilar = group.Find(x => x.Type != group[0].Type) != null;
        
            if (!(isValidForAscending || isValidForSimilar))
            {
                return false;
            }
            
            return true;
        }

        #endregion

        
        #region CardCollection: GetTypes

        private Dictionary<CardType, List<CardInfo>> GetCardTypesCollection()
        {
            var spades = _hand.FindAll(x => x.Type == CardType.Spades).OrderBy(x => x.Value).ToList();
            var diamonds = _hand.FindAll(x => x.Type == CardType.Diamonds).OrderBy(x => x.Value).ToList();
            var hearts = _hand.FindAll(x => x.Type == CardType.Hearts).OrderBy(x => x.Value).ToList();
            var clubs = _hand.FindAll(x => x.Type == CardType.Clubs).OrderBy(x => x.Value).ToList();

            var similarTypeDictionary = new Dictionary<CardType, List<CardInfo>>()
            { 
                { CardType.Spades, spades },
                { CardType.Diamonds, diamonds },
                { CardType.Hearts, hearts },
                { CardType.Clubs, clubs }
            };

            return similarTypeDictionary;
        }
        
        #endregion
        
        #region CardCollection: GetValues

        private Dictionary<CardValue, List<CardInfo>> GetCardValuesCollection()
        {
            var aces = _hand.FindAll(x => x.Value == CardValue.Ace).ToList();
            var twos = _hand.FindAll(x => x.Value == CardValue.Two).ToList();
            var threes = _hand.FindAll(x => x.Value == CardValue.Three).ToList();
            var fours = _hand.FindAll(x => x.Value == CardValue.Four).ToList();
            var fives = _hand.FindAll(x => x.Value == CardValue.Five).ToList();
            var sixes = _hand.FindAll(x => x.Value == CardValue.Six).ToList();
            var sevens = _hand.FindAll(x => x.Value == CardValue.Six).ToList();
            var eights = _hand.FindAll(x => x.Value == CardValue.Eight).ToList();
            var nines = _hand.FindAll(x => x.Value == CardValue.Nine).ToList();
            var tens = _hand.FindAll(x => x.Value == CardValue.Ten).ToList();
            var jacks = _hand.FindAll(x => x.Value == CardValue.Jack).ToList();
            var queens = _hand.FindAll(x => x.Value == CardValue.Queen).ToList();
            var kings = _hand.FindAll(x => x.Value == CardValue.King).ToList();

            var similarValueDictionary = new Dictionary<CardValue, List<CardInfo>>()
            { 
                { CardValue.Ace, aces },
                { CardValue.Two, twos },
                { CardValue.Three, threes },
                { CardValue.Four, fours },
                { CardValue.Five, fives },
                { CardValue.Six, sixes },
                { CardValue.Seven, sevens },
                { CardValue.Nine, nines },
                { CardValue.Ten, tens },
                { CardValue.Jack, jacks },
                { CardValue.Queen, queens },
                { CardValue.King, kings }
            };

            return similarValueDictionary;
        }
        
        #endregion
        
        
        #region Ascending: GetProperGroups

        private List<List<CardInfo>> GetProperAscendingGroups()
        {
            var cardTypesCollection = GetCardTypesCollection();

            var properAscendingGroups = new List<List<CardInfo>>();
            
            foreach (var cardTypeCollection in cardTypesCollection.Values)
            {
                if (cardTypeCollection.Count >= 3)
                {
                    var typeGroup = new List<CardInfo>();
                    foreach (var cardInfo in cardTypeCollection)
                    {
                        if (typeGroup.Count == 0)
                        {
                            if (properAscendingGroups.Any())
                            {
                                if (properAscendingGroups.Last().Last().Value + 1 == cardInfo.Value)
                                {
                                    properAscendingGroups.Last().Add(cardInfo);
                                    continue;
                                }
                            }
                            typeGroup.Add(cardInfo);
                            continue;
                        }

                        if (cardInfo.Value != typeGroup.Last().Value + 1)
                        {
                            typeGroup.Clear();
                            typeGroup.Add(cardInfo);
                            continue;
                        }
                    
                        typeGroup.Add(cardInfo);
                        if (typeGroup.Count >= 3)
                        {
                            properAscendingGroups.Add(new List<CardInfo>(typeGroup));
                            typeGroup.Clear();
                        }
                    }
                }
            }

            return properAscendingGroups;
        }

        #endregion

        #region Similar: GetProperGroups

        private List<List<CardInfo>> GetProperSimilarGroups()
        {
            var cardValuesCollection = GetCardValuesCollection();

            var savedGroups = new List<List<CardInfo>>();

            foreach (var similarValuesList in cardValuesCollection.Values)
            {
                if (similarValuesList.Count >= 3)
                {
                    savedGroups.Add(similarValuesList);
                }
            }
            
            return savedGroups;
        }

        #endregion
        
        
        #region Hand: SetProperGroups

        private void SetProperGroupsToHand(List<CardInfo> properGroupList)
        {
            for (int i = 0; i < properGroupList.Count; i++)
            {
                _hand.Remove(properGroupList[i]);
                _hand.Insert(i, properGroupList[i]);
                Debug.Log($"{ properGroupList[i].Type }_{ properGroupList[i].Value }");
            }
        }

        #endregion
    }
}

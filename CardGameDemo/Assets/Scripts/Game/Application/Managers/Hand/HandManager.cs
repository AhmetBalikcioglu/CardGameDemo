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
            CreateTestCaseHand();
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

        #region Hand: CreateRandom

        public void CreateRandomHand()
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
        
        #region Hand: CreateTestCase

        private void CreateTestCaseHand()
        {
            _handOrder = HandOrder.None;
            
            _hand = new List<CardInfo>();
            _hand = Config.HandTestCase1ConfigList;
        }

        #endregion
        
        #region Hand: CardSwapped

        public void CardSwapped(List<CardInfo> newHandList)
        {
            _hand = newHandList;
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
                case HandOrder.Smart:
                    SetHandToSmartOrdered();
                    break;
            }
            
            HandOrderUpdatedEvent?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        
        #region Hand: SetToAscendingOrdered

        private void SetHandToAscendingOrdered()
        {
            var properAscendingGroupsList = GetProperAscendingGroups(_hand);

            for (int i = properAscendingGroupsList.Count - 1; i >= 0 ; i--)
            {
                SetProperGroupsToHand(properAscendingGroupsList[i], _hand);
            }

            _handOrder = HandOrder.Ascending;
        }
        
        #endregion

        #region Hand: SetToSimilarOrdered

        private void SetHandToSimilarOrdered()
        {
            var properSimilarGroupsList = GetProperSimilarGroups(_hand);
            
            for (int i = properSimilarGroupsList.Count - 1; i >= 0 ; i--)
            {
                SetProperGroupsToHand(properSimilarGroupsList[i], _hand);
            }

            _handOrder = HandOrder.Similar;
        }

        #endregion

        #region Hand: SetToSmartOrdered
        
        // Just a tryout
        private void SetHandToSmartOrdered()
        {
            var properAscendingGroupsList = GetProperAscendingGroups(_hand);
            var properSimilarGroupsList = GetProperSimilarGroups(_hand);

            var collidedGroupsInfoDictionary = GetCollidedCardCollection(properAscendingGroupsList, properSimilarGroupsList);
            
            var properSmartGroupsList = new List<List<CardInfo>>();

            foreach (var properAscendingGroup in properAscendingGroupsList)
            {
                if (!collidedGroupsInfoDictionary.Keys.Contains(properAscendingGroup))
                {
                    properSmartGroupsList.Add(properAscendingGroup);
                }
            }

            foreach (var properSimilarGroup in properSimilarGroupsList)
            {
                if (collidedGroupsInfoDictionary[properSimilarGroup] == null)
                {
                    properSmartGroupsList.Add(properSimilarGroup);
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
                        foreach (var cardInfo in collidedGroupInfo.Value.CollidedCardList)
                        {
                            collidedCardGroup.Remove(cardInfo);
                            collidedGroupsInfoDictionary[collidedCardGroup].CollidedCardList.Remove(cardInfo);
                        }
                        collidedGroupsInfoDictionary[collidedCardGroup].CollidedCardGroups.Remove(collidedCardGroup);
        
                        if (!IsGroupValid(collidedCardGroup))
                        {
                            RemoveCardGroupFromEverywhere(collidedCardGroup, ref collidedGroupsInfoDictionary);
                        }
                        else
                        {
                            var newCollideInfo = new CollidedGroupInfo(collidedGroupsInfoDictionary[collidedCardGroup].Group, collidedGroupsInfoDictionary[collidedCardGroup].CollidedCardGroups, collidedGroupsInfoDictionary[collidedCardGroup].CollidedCardList);
                            var newGroup = new List<CardInfo>(collidedCardGroup);
                            collidedGroupsInfoDictionary.Remove(collidedCardGroup);
                            collidedGroupsInfoDictionary.Add(newGroup, newCollideInfo);
                        }
                    }
                    
                    resolvedGroupsList.Add(collidedGroupInfo.Key);
                    continue;
                }
                
                foreach (var collidedCard in collidedGroupInfo.Value.CollidedCardList)
                {
                    //1-2-3-4 4-4-4 Scenario

                    foreach (var collidedCardGroup in collidedGroupInfo.Value.CollidedCardGroups)
                    {
                        if (!collidedCardGroup.Contains(collidedCard))
                        {
                            continue;
                        }
                        
                        var collidedGroupCopy = new List<CardInfo>(collidedGroupInfo.Key);
                        collidedGroupCopy.Remove(collidedCard);
                        if (!IsGroupValid(collidedGroupCopy))
                        {
                            collidedCardGroup.Remove(collidedCard);
                            collidedGroupsInfoDictionary[collidedCardGroup].Group.Remove(collidedCard);
                            collidedGroupsInfoDictionary[collidedCardGroup].CollidedCardList.Remove(collidedCard);
                            collidedGroupsInfoDictionary[collidedCardGroup].CollidedCardGroups.Remove(collidedGroupInfo.Key);
                            
                            collidedGroupInfo.Value.CollidedCardList.Remove(collidedCard);
                            collidedGroupInfo.Value.CollidedCardGroups.Remove(collidedCardGroup);

                            if (!IsGroupValid(collidedCardGroup))
                            {
                                RemoveCardGroupFromEverywhere(collidedCardGroup, ref collidedGroupsInfoDictionary);
                            }
                            
                            continue;
                        }
                        
                        collidedGroupsInfoDictionary[collidedCardGroup].CollidedCardList.Remove(collidedCard);
                        collidedGroupsInfoDictionary[collidedCardGroup].CollidedCardGroups.Remove(collidedGroupInfo.Key);
                            
                        collidedGroupInfo.Key.Remove(collidedCard);
                        collidedGroupInfo.Value.CollidedCardList.Remove(collidedCard);
                        collidedGroupInfo.Value.CollidedCardGroups.Remove(collidedCardGroup);
                        continue;
                    }
                }
                resolvedGroupsList.Add(collidedGroupInfo.Key);
            }
            
            properSmartGroupsList.AddRange(resolvedGroupsList);
            
            for (int i = properSmartGroupsList.Count - 1; i >= 0 ; i--)
            {
                SetProperGroupsToHand(properSmartGroupsList[i], _hand);
            }
        }
        
        private void RemoveCardGroupFromEverywhere(List<CardInfo> group, ref SortedDictionary<List<CardInfo>, CollidedGroupInfo> dictionary)
        {
            foreach (var collidedCardGroup in dictionary[group].CollidedCardGroups)
            {
                dictionary[collidedCardGroup].CollidedCardGroups.Remove(group);
                foreach (var cardInfo in group)
                {
                    dictionary[collidedCardGroup].CollidedCardList.Remove(cardInfo);
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

        private Dictionary<CardType, List<CardInfo>> GetCardTypesCollection(List<CardInfo> hand)
        {
            var spades = hand.FindAll(x => x.Type == CardType.Spades).OrderBy(x => x.Value).ToList();
            var diamonds = hand.FindAll(x => x.Type == CardType.Diamonds).OrderBy(x => x.Value).ToList();
            var hearts = hand.FindAll(x => x.Type == CardType.Hearts).OrderBy(x => x.Value).ToList();
            var clubs = hand.FindAll(x => x.Type == CardType.Clubs).OrderBy(x => x.Value).ToList();

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

        private Dictionary<CardValue, List<CardInfo>> GetCardValuesCollection(List<CardInfo> hand)
        {
            var aces = hand.FindAll(x => x.Value == CardValue.Ace).ToList();
            var twos = hand.FindAll(x => x.Value == CardValue.Two).ToList();
            var threes = hand.FindAll(x => x.Value == CardValue.Three).ToList();
            var fours = hand.FindAll(x => x.Value == CardValue.Four).ToList();
            var fives = hand.FindAll(x => x.Value == CardValue.Five).ToList();
            var sixes = hand.FindAll(x => x.Value == CardValue.Six).ToList();
            var sevens = hand.FindAll(x => x.Value == CardValue.Six).ToList();
            var eights = hand.FindAll(x => x.Value == CardValue.Eight).ToList();
            var nines = hand.FindAll(x => x.Value == CardValue.Nine).ToList();
            var tens = hand.FindAll(x => x.Value == CardValue.Ten).ToList();
            var jacks = hand.FindAll(x => x.Value == CardValue.Jack).ToList();
            var queens = hand.FindAll(x => x.Value == CardValue.Queen).ToList();
            var kings = hand.FindAll(x => x.Value == CardValue.King).ToList();

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

        #region CardCollection: GetCollided

        private SortedDictionary<List<CardInfo>, CollidedGroupInfo> GetCollidedCardCollection(List<List<CardInfo>> properAscendingGroupsList, List<List<CardInfo>> properSimilarGroupsList)
        {
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
                    collidedGroupsInfoDictionary.Add(properSimilarGroup,
                        new CollidedGroupInfo(properSimilarGroup, collidedGroups, collidedCards));
                }
            }

            return collidedGroupsInfoDictionary;
        }

        #endregion
        
        
        #region Ascending: GetProperGroups

        internal List<List<CardInfo>> GetProperAscendingGroups(List<CardInfo> hand)
        {
            var cardTypesCollection = GetCardTypesCollection(hand);

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

        internal List<List<CardInfo>> GetProperSimilarGroups(List<CardInfo> hand)
        {
            var cardValuesCollection = GetCardValuesCollection(hand);

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

        internal void SetProperGroupsToHand(List<CardInfo> properGroupList, List<CardInfo> hand)
        {
            for (int i = 0; i < properGroupList.Count; i++)
            {
                hand.Remove(properGroupList[i]);
                hand.Insert(i, properGroupList[i]);
                Debug.Log($"{ properGroupList[i].Type }_{ properGroupList[i].Value }");
            }
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Lib.Manager;
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
            CreateHand();
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

        #region Hand: Create

        public void CreateHand()
        {
            if (AppController.Instance.AppConfig.TestEnabled)
            {
                CreateTestCaseHand();
            }
            else
            {
                CreateRandomHand();
            }
        }

        #endregion
        
        #region Hand: CreateRandom

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
        
        #region Hand: CreateTestCase

        internal void CreateTestCaseHand()
        {
            _handOrder = HandOrder.None;
            
            _hand = new List<CardInfo>(Config.HandTestCase1ConfigList);
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
                SetProperGroupsToHand(properAscendingGroupsList[i]);
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
                SetProperGroupsToHand(properSimilarGroupsList[i]);
            }

            _handOrder = HandOrder.Similar;
        }

        #endregion

        #region Hand: SetToSmartOrdered
        
        private void SetHandToSmartOrdered()
        {
            var properAscendingGroupsList = GetProperAscendingGroups(_hand);
            var properSimilarGroupsList = GetProperSimilarGroups(_hand);

            var collidedGroupInfoList = GetGroupCollidedCollection(properAscendingGroupsList, properSimilarGroupsList);
            
            var properSmartGroupsList = new List<List<CardInfo>>();

            foreach (var properAscendingGroup in properAscendingGroupsList)
            {
                if (collidedGroupInfoList.Find(x => IsGroupSame(x.Group, properAscendingGroup)) == null)
                {
                    properSmartGroupsList.Add(properAscendingGroup);
                }
            }

            foreach (var properSimilarGroup in properSimilarGroupsList)
            {
                if (collidedGroupInfoList.Find(x => IsGroupSame(x.Group, properSimilarGroup)) == null)
                {
                    properSmartGroupsList.Add(properSimilarGroup);
                }
            }
            
            var resolvedGroupsList = new List<List<CardInfo>>();
        
            foreach (var collidedGroupInfo in collidedGroupInfoList)
            {
                if (!IsGroupValid(collidedGroupInfo.Group))
                {
                    RemoveCardGroupFromEverywhere(collidedGroupInfo.Group, ref collidedGroupInfoList);
                    continue;
                }
        
                if (!collidedGroupInfo.ColliderGroupInfoList.Any())
                {
                    resolvedGroupsList.Add(new List<CardInfo>(collidedGroupInfo.Group));
                    continue;
                }
                
                if (collidedGroupInfo.Group.Count == 3)
                {
                    resolvedGroupsList.Add(new List<CardInfo>(collidedGroupInfo.Group));
                    foreach (var colliderGroupInfo in collidedGroupInfo.ColliderGroupInfoList.ToList())
                    {
                        RemoveCardFromAnywhereElse(collidedGroupInfo.Group, colliderGroupInfo.Card, ref collidedGroupInfoList);
                    }
                    continue;
                }
                
                foreach (var colliderGroupInfo in collidedGroupInfo.ColliderGroupInfoList.ToList())
                {
                    //1-2-3-4 4-4-4 Scenario
                    
                    var collidedGroupCopy = new List<CardInfo>(collidedGroupInfo.Group);
                    collidedGroupCopy.RemoveAll(x => x.Equals(colliderGroupInfo.Card));
                    if (!IsGroupValid(collidedGroupCopy))
                    {
                        RemoveCardFromAnywhereElse(collidedGroupInfo.Group, colliderGroupInfo.Card, ref collidedGroupInfoList);
                        continue;
                    }
                    
                    RemoveCardFromAnywhereElse(colliderGroupInfo.Group, colliderGroupInfo.Card, ref collidedGroupInfoList);
                }
                resolvedGroupsList.Add(new List<CardInfo>(collidedGroupInfo.Group));
            }
            
            properSmartGroupsList.AddRange(resolvedGroupsList);
            
            for (int i = properSmartGroupsList.Count - 1; i >= 0 ; i--)
            {
                SetProperGroupsToHand(properSmartGroupsList[i]);
            }
            
            _handOrder = HandOrder.Smart;
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

        #region GroupCollection: GetCollided

        private List<CollidedGroupInfo> GetGroupCollidedCollection(List<List<CardInfo>> properAscendingGroupsList, List<List<CardInfo>> properSimilarGroupsList)
        {
            var collidedGroupsInfoList = new List<CollidedGroupInfo>();
            foreach (var properAscendingGroup in properAscendingGroupsList)
            {
                var colliderGroupInfoList = new List<ColliderGroupInfo>();
                foreach (var properSimilarGroup in properSimilarGroupsList)
                {
                    foreach (var card in properAscendingGroup)
                    {
                        if (properSimilarGroup.Contains(card))
                        {
                            colliderGroupInfoList.Add(new ColliderGroupInfo(properSimilarGroup, card));
                            break;
                        }
                    }
                }
        
                if (colliderGroupInfoList.Any())
                {
                    collidedGroupsInfoList.Add(new CollidedGroupInfo(properAscendingGroup, colliderGroupInfoList));
                }
            }

            foreach (var properSimilarGroup in properSimilarGroupsList)
            {
                var colliderGroupInfoList = new List<ColliderGroupInfo>();
                foreach (var properAscendingGroup in properAscendingGroupsList)
                {
                    foreach (var card in properSimilarGroup)
                    {
                        if (properAscendingGroup.Contains(card))
                        {
                            colliderGroupInfoList.Add(new ColliderGroupInfo(properAscendingGroup, card));
                            break;
                        }
                    }
                }

                if (colliderGroupInfoList.Any())
                {
                    collidedGroupsInfoList.Add(new CollidedGroupInfo(properSimilarGroup, colliderGroupInfoList));
                }
            }

            collidedGroupsInfoList = SetCollidedGroupInfoListDescending(collidedGroupsInfoList);
            return collidedGroupsInfoList;
        }

        private List<CollidedGroupInfo> SetCollidedGroupInfoListDescending(List<CollidedGroupInfo> collidedGroupInfoList)
        {
            var descendingCollidedGroupInfoList = collidedGroupInfoList.Distinct().OrderByDescending(x => GetGroupTotalValue(x.Group)).ToList();
            foreach (var collidedGroupInfo in descendingCollidedGroupInfoList)
            {
                var descendingColliderGroupInfoList = collidedGroupInfo.ColliderGroupInfoList.Distinct().OrderByDescending(x => GetGroupTotalValue(x.Group)).ToList();
                collidedGroupInfo.ColliderGroupInfoList = descendingColliderGroupInfoList;
            }

            return descendingCollidedGroupInfoList;
        }

        #endregion
        
        
        #region Ascending: GetProperGroups

        private List<List<CardInfo>> GetProperAscendingGroups(List<CardInfo> hand)
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

        private List<List<CardInfo>> GetProperSimilarGroups(List<CardInfo> hand)
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


        #region HelperFunctions: GetCardValue | GetGroupTotalValue | IsGroupValid | IsGroupSame

        private int GetCardValue(CardInfo card)
        {
            var value = (int)card.Value;
            if (value > 10)
            {
                value = 10;
            }

            return value;
        }

        private int GetGroupTotalValue(List<CardInfo> cardGroup)
        {
            int totalValue = 0;
            foreach (var card in cardGroup)
            {
                var value = GetCardValue(card);
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
                if (group[i].Value < group[i - 1].Value)
                {
                    isValidForAscending = false;
                    break;
                }
                isValidForAscending = true;
            }
            var isValidForSimilar = group.Find(x => x.Type != group[0].Type) != null;
        
            return isValidForAscending || isValidForSimilar;
        }

        private bool IsGroupSame(List<CardInfo> group1, List<CardInfo> group2)
        {
            var group1Copy = new List<CardInfo>(group1);
            var group2Copy = new List<CardInfo>(group2);
            
            foreach (var card in group1)
            {
                if (!group2.Contains(card))
                {
                    return false;
                }
                
                group1Copy.Remove(card);
                group2Copy.Remove(card);
            }
            
            return !group2Copy.Any();
        }
        
        #endregion

        #region Remove: CardFromAnywhereElse | CardGroupFromEverywhere

        private void RemoveCardFromAnywhereElse(List<CardInfo> group, CardInfo card, ref List<CollidedGroupInfo> collidedGroupInfoList)
        {
            foreach (var collidedGroupInfo in collidedGroupInfoList)
            {
                if (!IsGroupSame(group, collidedGroupInfo.Group))
                {
                    collidedGroupInfo.Group.RemoveAll(x => x.Equals(card));
                }
                collidedGroupInfo.ColliderGroupInfoList.RemoveAll(x => x.Card.Equals(card));
            }
        }

        private void RemoveCardGroupFromEverywhere(List<CardInfo> group, ref List<CollidedGroupInfo> groupInfoList)
        {
            var groupIndex = groupInfoList.FindIndex(x => IsGroupSame(x.Group, group));
            if (groupIndex < 0)
            {
                return;
            }
            
            foreach (var colliderCardGroupInfo in groupInfoList[groupIndex].ColliderGroupInfoList)
            {
                var colliderInfoIndex = groupInfoList.FindIndex(x => IsGroupSame(x.Group, colliderCardGroupInfo.Group));
                if (colliderInfoIndex >= 0)
                {
                    groupInfoList[colliderInfoIndex].ColliderGroupInfoList.RemoveAll(x => IsGroupSame(x.Group, group));
                }
            }
        }

        #endregion
    }
}
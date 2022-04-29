using System.Collections.Generic;
using System.Linq;
using Lib.Manager;
using Main.Data;
using UnityEngine;

namespace Main.Application.Managers
{
    public class HandManager: CManager
    {
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

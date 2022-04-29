using System;
using System.Collections.Generic;
using Main.Game.Application.Managers;
using Main.Game.Presentation.Initializers;
using UnityEngine;

namespace Main.Game.Presentation
{
    public class HandController : MonoBehaviour
    {
        #region Fields

        private List<CardController> _cardControllerList;

        private CardController _selectedCard;

        #endregion


        #region Init | DeInit

        public void Init()
        {
            _cardControllerList = new List<CardController>();
           
            AddEvents();
        }

        public void DeInit()
        {
            foreach (var cardController in _cardControllerList)
            {
                cardController.DeInit();
            }

            RemoveEvents();
        }
        
        #endregion
        

        #region Unity: Update

        private void Update()
        {
            if (_selectedCard == null)
            {
                return;
            }
            
            var worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldMousePosition.z = 0;
            _selectedCard.View.transform.position = worldMousePosition;
            CheckForCardSwap(_selectedCard);
        }

        #endregion

        
        #region Card: CreateController

        private void CreateCardController(CardView cardView)
        {
            var model = new CardModel()
            {
                Info = ManagerContainer.Instance.Hand.Hand[_cardControllerList.Count]
            };

            var controller = new CardController();
            controller.Init(model, cardView);
            controller.Show();
            
            _cardControllerList.Add(controller);
            
            controller.OnCardSelectedEvent += OnCardSelected;
            controller.OnCardReleasedEvent += OnCardReleased;
        }

        #endregion

        #region Card: Swap

        private void CheckForCardSwap(CardController cardController)
        {
            var cardIndex = _cardControllerList.IndexOf(cardController);
            var rightCardController = cardIndex == _cardControllerList.Count - 1 ? null : _cardControllerList[cardIndex + 1];
            if (rightCardController != null)
            {
                if (cardController.View.transform.position.x > rightCardController.View.transform.position.x)
                {
                    SwapCard(cardController, rightCardController);
        
                    RefreshCardPositions();
                }
            }
        
            var leftCardController = cardIndex == 0 ? null : _cardControllerList[cardIndex - 1];
            if (leftCardController != null)
            {
                if (cardController.View.transform.position.x < leftCardController.View.transform.position.x)
                {
                    SwapCard(leftCardController, cardController);
        
                    RefreshCardPositions();
                }
            }
        }
        
        private void SwapCard(CardController leftCardController, CardController rightCardController)
        {
            var leftCardIndex = _cardControllerList.IndexOf(leftCardController);
            var rightCardIndex = _cardControllerList.IndexOf(rightCardController);

            _cardControllerList.Insert(leftCardIndex, rightCardController);
            _cardControllerList.RemoveAt(rightCardIndex + 1);
        
            if (_selectedCard != leftCardController)
            {
                leftCardController.View.GetComponent<SpriteRenderer>().sortingOrder = rightCardIndex;
            }
        
            if (_selectedCard != rightCardController)
            {
                rightCardController.View.GetComponent<SpriteRenderer>().sortingOrder = leftCardIndex;
            }
        }

        #endregion
        
        #region Card: SetPositionAndRotation

        private void SetCardPositionAndRotation(CardController card, float totalDegree, bool isLeft)
        {
            var cardIndex = _cardControllerList.IndexOf(card);

            if (card != _selectedCard)
            {
                var radius = Config.CircleRadiusForArc;
                var circlePosition = new Vector3(0, Config.CircleYPositionForArc, 0);
                var position = circlePosition + new Vector3(Mathf.Sin(totalDegree) * radius, Mathf.Cos(totalDegree) * radius, 0);
                var rotation = position - circlePosition;
                card.SetPositionAndRotation(position, rotation);
                card.View.GetComponent<SpriteRenderer>().sortingOrder = cardIndex;
            }

            if (isLeft)
            {
                var leftCardController = cardIndex == 0 ? null : _cardControllerList[cardIndex - 1];
                if (leftCardController != null)
                {
                    SetCardPositionAndRotation(leftCardController, totalDegree - Config.DegreeBetweenCardsForArc, isLeft);
                }
            }
            else
            {
                var rightCardController = cardIndex == _cardControllerList.Count - 1 ? null : _cardControllerList[cardIndex + 1];
                if (rightCardController != null)
                {
                    SetCardPositionAndRotation(rightCardController, totalDegree + Config.DegreeBetweenCardsForArc, isLeft);
                }
            }
        }

        #endregion


        #region CardList: Rearrange

        private void RearrangeCardControllerList()
        {
            var newControllerList = new List<CardController>();

            foreach (var cardInfo in ManagerContainer.Instance.Hand.Hand)
            {
                var controller = _cardControllerList.Find(x => x.Model.Info == cardInfo);
                var newModel = new CardModel()
                {
                    Info = cardInfo
                };
                controller.SetModel(newModel);
                newControllerList.Add(controller);
            }

            _cardControllerList = newControllerList;
            RefreshCardPositions();
        }

        #endregion

        #region CardList: RefreshPositionsFor: Odd | Even

        private void RefreshCardPositions()
        {
            if (_cardControllerList.Count % 2 == 1)
            {
                RefreshCardPositionsForOddList();
            }
            else
            {
                RefreshCardPositionsForEvenList();
            }
        }

        private void RefreshCardPositionsForOddList()
        {
            var currentIndex = _cardControllerList.Count / 2;

            var middleCard = _cardControllerList[currentIndex];
            if (middleCard != _selectedCard)
            {
                var radius = Config.CircleRadiusForArc;
                var circlePosition = new Vector3(0, Config.CircleYPositionForArc, 0);
                var position = circlePosition + new Vector3(Mathf.Sin(0) * radius, Mathf.Cos(0) * radius, 0);
                var rotation = position - circlePosition;

                middleCard.SetPositionAndRotation(position, rotation);
                middleCard.View.GetComponent<SpriteRenderer>().sortingOrder = currentIndex;
            }

            var leftCardController = currentIndex == 0 ? null : _cardControllerList[currentIndex - 1];
            var rightCardController = currentIndex == _cardControllerList.Count - 1 ? null : _cardControllerList[currentIndex + 1];
            
            if (leftCardController != null)
            {
                SetCardPositionAndRotation(leftCardController, - Config.DegreeBetweenCardsForArc, true);
            }

            if (rightCardController != null)
            {
                SetCardPositionAndRotation(rightCardController, Config.DegreeBetweenCardsForArc, false);
            }
        }

        private void RefreshCardPositionsForEvenList()
        {
            var rightIndex = _cardControllerList.Count / 2;
            SetCardPositionAndRotation(_cardControllerList[rightIndex - 1], - Config.DegreeBetweenCardsForArc / 2f, true);
            SetCardPositionAndRotation(_cardControllerList[rightIndex], Config.DegreeBetweenCardsForArc / 2f, false);
        }

        #endregion

        
        #region Event: OnHandOrderUpdated

        private void OnHandOrderUpdated(object sender, EventArgs e)
        {
            RearrangeCardControllerList();
        }

        #endregion

        #region Event: OnCardCreated

        private void OnCardCreated(object sender, CardCreatedEventArgs e)
        {
            CreateCardController(e.View);
            RefreshCardPositions();
        }

        #endregion

        #region Event: OnCardSelected

        private void OnCardSelected(object sender, CardSelectedEventArgs e)
        {
            _selectedCard = e.CardController;
            _selectedCard.View.transform.eulerAngles = Vector3.zero;
            _selectedCard.View.transform.GetComponent<SpriteRenderer>().sortingOrder = 50;
        }

        #endregion

        #region Event: OnCardReleased

        private void OnCardReleased(object sender, CardReleasedEventArgs e)
        {
            var cardIndex = _cardControllerList.IndexOf(_selectedCard);
            
            _selectedCard.View.transform.GetComponent<SpriteRenderer>().sortingOrder = cardIndex;
            _selectedCard = null;
            
            RefreshCardPositions();
        }

        #endregion

        #region Events: Add | Remove

        private void AddEvents()
        {
            ManagerContainer.Instance.Hand.HandOrderUpdatedEvent += OnHandOrderUpdated;
            HandInitializer.Instance.CardCreatedEvent += OnCardCreated;
        }

        private void RemoveEvents()
        {
            ManagerContainer.Instance.Hand.HandOrderUpdatedEvent -= OnHandOrderUpdated;
            HandInitializer.Instance.CardCreatedEvent -= OnCardCreated;

            foreach (var cardController in _cardControllerList)
            {
                cardController.OnCardSelectedEvent -= OnCardSelected;
            }

            foreach (var cardController in _cardControllerList)
            {
                cardController.OnCardReleasedEvent -= OnCardReleased;
            }
        }

        #endregion
    }
}
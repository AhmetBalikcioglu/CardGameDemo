using System;
using System.Collections;
using UnityEngine;

namespace Main.Presentation.Initializers
{
    public class HandInitializer : MonoBehaviour
    {
        #region Events

        public event EventHandler<CardCreatedEventArgs> CardCreatedEvent;

        #endregion

        #region Singleton Instance

        private static HandInitializer _instance = null;
        public static HandInitializer Instance => _instance;

        #endregion

        #region Childs

        [Header("Containers")]
        [SerializeField]
        private Transform _deckContainer;

        [SerializeField]
        private Transform _handContainer;

        #endregion

        #region Fields

        private bool _didHandCreated;
        public bool DidHandCreated => _didHandCreated;

        #endregion

        #region Unity: Awake

        private void Awake()
        {
            var objectList = FindObjectsOfType(typeof(HandInitializer));
            if (objectList.Length > 1)
            {
                Destroy(gameObject);
            }

            _instance = this;
        }

        #endregion

        
        #region Hand: Create

        public void CreateHand()
        {
            var prefab = ResourceProvider.GetCardGameObject();
            StartCoroutine(SpawnCard(prefab));
        }

        #endregion

        #region Card: Spawn

        private IEnumerator SpawnCard(GameObject prefab)
        {
            for (int i = 0; i < Config.TotalHandCount; i++)
            {
                var gameObject = Instantiate(prefab, _deckContainer.position, Quaternion.identity, _handContainer);

                CardCreatedEvent?.Invoke(this, new CardCreatedEventArgs()
                {
                    View = gameObject.GetComponent<CardView>()
                });

                yield return new WaitForSeconds(0.5f);
            }

            _didHandCreated = true;
        }

        #endregion
    }
}
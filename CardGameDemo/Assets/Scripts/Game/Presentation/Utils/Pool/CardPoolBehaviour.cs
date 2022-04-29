using Lib.Pool;
using UnityEngine;

namespace Main.Game.Presentation
{
    public class CardPoolBehaviour : ObjectPoolBehaviour
    {
        #region Singleton Instance

        private static CardPoolBehaviour _instance = null;
        public static CardPoolBehaviour Instance => _instance;

        #endregion

        #region Unity: OnEnable

        private void OnEnable()
        {
            var objectList = FindObjectsOfType(typeof(CardPoolBehaviour));
            if (objectList.Length > 1)
            {
                Destroy(gameObject);
            }

            _instance = this;
        }

        #endregion

        
        #region Pool: OnCreatePool

        protected override void OnCreatePool()
        {
            
        }
        
        #endregion

        #region Pool: OnGetObject

        protected override void OnGetPooledObject(GameObject gameObject)
        {
            
        }

        #endregion
    }
}
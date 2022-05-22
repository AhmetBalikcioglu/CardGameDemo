using UnityEngine;

namespace Main
{
    public class AppController: MonoBehaviour
    {
        #region Singleton Instance

        private static AppController _instance = null;
        public static AppController Instance => _instance;

        #endregion

        #region Configuration

        [Header("Configuration")]
        
        [SerializeField] 
        private AppConfig _appConfig;
        public AppConfig AppConfig => _appConfig;

        #endregion

        
        #region Unity: Awake

        private void Awake()
        {
            #region DontDestroyOnLoad

            var objectList = FindObjectsOfType(typeof(AppController));
            if (objectList.Length > 1)
            {
                Destroy(gameObject);
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            #endregion
        }

        #endregion
    }
}
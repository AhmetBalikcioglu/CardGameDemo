using System.Collections;
using UnityEngine;

namespace Main.Application
{
    public class ManagerContainer : MonoBehaviour
    {
        #region Singleton Instance

        private static ManagerContainer _instance = null;
        public static ManagerContainer Instance => _instance;

        #endregion

        #region Fields

        private bool _allManagersReady = false;
        public bool IsAllManagersReady => _allManagersReady;

        #endregion
        
        #region Unity: Awake

        private void Awake()
        {
            var objectList = FindObjectsOfType(typeof(ManagerContainer));
            if (objectList.Length > 1)
            {
                Destroy(gameObject);
            }

            _instance = this;
        }

        #endregion

        
        #region Init | DeInit

        public void Init()
        {
            _allManagersReady = false;
            
            CreateManagers();
            BuildManagers();
        }
        
        public void DeInit()
        {
            UnRegisterManagers();
            EndManagers();
        }

        #endregion

        #region Launch

        public void Launch()
        {
            BeginManagers();
            RegisterManagers();

            _allManagersReady = true;
        }

        #endregion
        

        #region ReBegin

        public void ReBegin()
        {
            StartCoroutine(ReBeginRoutine());
        }

        private IEnumerator ReBeginRoutine()
        {
            yield return new WaitForEndOfFrame();
            
            ReBeginManagers();
        }

        #endregion
        

        #region CManager: Create

        private void CreateManagers()
        {
            
        }

        #endregion

        #region CManager: Build

        private void BuildManagers()
        {
            
        }

        #endregion

        #region CManager: Begin

        private void BeginManagers()
        {
            
        }

        #endregion

        #region CManager: ReBegin

        private void ReBeginManagers()
        {
            
        }

        #endregion

        #region CManager: Register

        private void RegisterManagers()
        {
            
        }

        #endregion
        
        #region CManager: UnRegister

        private void UnRegisterManagers()
        {
            
        }

        #endregion

        #region CManager: End

        private void EndManagers()
        {
            
        }

        #endregion
    }
}

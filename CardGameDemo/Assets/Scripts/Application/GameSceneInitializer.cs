using Main.Application.Managers;
using UnityEngine;

namespace Main.Application
{
    public class GameSceneInitializer: MonoBehaviour
    {
        #region Unity: Awake | Start | OnDestroy

        private void OnEnable()
        {
            ManagerContainer.Instance.Init();
        }

        private void Start()
        {
            ManagerContainer.Instance.Launch();
        }

        private void OnDestroy()
        {
            ManagerContainer.Instance.DeInit();
        }

        #endregion
        
        
        #region App: Enter | Exit

        private void EnterApp()
        {
            ManagerContainer.Instance.ReBegin();
        }

        private void ExitApp(bool isQuit)
        {
            
        }

        #endregion
        

        #region Unity: OnApplicationPause | OnApplicationQuit

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                ExitApp(false);
            }
            else
            {
                EnterApp();
            }
        }

        private void OnApplicationQuit()
        {
            ExitApp(true);
        }

        #endregion
    }
}
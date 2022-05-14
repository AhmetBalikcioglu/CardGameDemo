using System;
using UnityEngine;

namespace Main.Game.Presentation.Utils
{
    public class CameraBehaviour: MonoBehaviour
    {
        #region Field

        private float _scaledOrthographicSize;

        #endregion

        
        #region Unity: Awake

        private void Awake()
        {
            Init();
        }

        #endregion

        
        #region Init

        private void Init()
        {
            ScaleCamera();

            if (AllowManipulateCameraForSafeArea())
            {
                SetCameraScaleForSafeArea();
                SetCameraPositionForSafeArea();
            }
        }

        #endregion
        
        
        #region Camera: Scale

        private void ScaleCamera()
        {
            var screenRatio = (float) Screen.width / (float) Screen.height;
            var targetRatio = Config.REF_BOUNDS.x / Config.REF_BOUNDS.y;
            
            if (screenRatio >= targetRatio)
            {
                _scaledOrthographicSize =  Config.REF_BOUNDS.y / 2;
            }
            else
            {
                var differenceInSize = targetRatio / screenRatio;
                _scaledOrthographicSize =  Config.REF_BOUNDS.y / 2 * differenceInSize;
            }
            
            Camera.main.orthographicSize = _scaledOrthographicSize;
        }

        #endregion

        #region Camera: ManipulateForSafeArea

        private bool AllowManipulateCameraForSafeArea()
        {
            if (Math.Abs(Screen.height - Screen.safeArea.size.y) < Double.Epsilon)
            {
                return false;
            }

            return true;
        }
        
        private void SetCameraScaleForSafeArea()
        {
            var totalHeightDiff = Screen.height - Screen.safeArea.size.y;
            
            _scaledOrthographicSize = (totalHeightDiff / 100f + Config.REF_BOUNDS.y) / 2f;
            Camera.main.orthographicSize = _scaledOrthographicSize;
        }

        private void SetCameraPositionForSafeArea()
        {
            var screenCenter = new Vector2(Screen.width, Screen.height) / 2f;
            var targetPosition = new Vector3(screenCenter.x - Screen.safeArea.center.x, screenCenter.y - Screen.safeArea.center.y) / 100f;
            targetPosition.z = -10f;
            
            transform.position = targetPosition;
        }
        
        #endregion
    }
}
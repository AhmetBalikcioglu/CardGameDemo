using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lib.SafeCanvas
{
    [RequireComponent(typeof(Canvas))]
    public class CSafeCanvas : MonoBehaviour
    {
        #region Fields

        public static UnityEvent onOrientationChange = new UnityEvent();
        public static UnityEvent onResolutionChange = new UnityEvent();

        #endregion

        #region Childs

        [SerializeField]
        private List<RectTransform> safeAreaTransformList;

        #endregion

        #region Fields

        public static bool IsLandscape { get; private set; }

        private static List<CSafeCanvas> _helpers = new List<CSafeCanvas>();

        private static bool _screenChangeVarsInitialized;
        private static ScreenOrientation _lastOrientation = ScreenOrientation.Portrait;
        private static Vector2 _lastResolution = Vector2.zero;
        private static Rect _lastSafeArea = Rect.zero;

        private Canvas _canvas;
        private RectTransform _rectTransform;

        #endregion

        #region Unity: Awake

        private void Awake()
        {
            if (!_helpers.Contains(this))
                _helpers.Add(this);

            _canvas = GetComponent<Canvas>();
            _rectTransform = GetComponent<RectTransform>();

            if (!_screenChangeVarsInitialized)
            {
                _lastOrientation = Screen.orientation;
                _lastResolution.x = Screen.width;
                _lastResolution.y = Screen.height;
                _lastSafeArea = Screen.safeArea;

                _screenChangeVarsInitialized = true;
            }
        }

        #endregion
        
        #region Unity: Start

        private void Start()
        {
            ApplySafeArea();
        }
        
        #endregion

        #region Unity: Update

        private void Update()
        {
            if (_helpers[0] != this)
                return;

            if (Screen.orientation != _lastOrientation)
                OrientationChanged();

            if (Screen.safeArea != _lastSafeArea)
                SafeAreaChanged();

            if (Screen.width != _lastResolution.x || Screen.height != _lastResolution.y)
                ResolutionChanged();
        }
        
        #endregion

        #region Unity: OnDestroy

        void OnDestroy()
        {
            if (_helpers != null && _helpers.Contains(this))
                _helpers.Remove(this);
        }

        #endregion

        
        #region SafeArea: Apply

        void ApplySafeArea()
        {
            if (safeAreaTransformList == null)
                return;

            var safeArea = Screen.safeArea;

            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;
            anchorMin.x /= _canvas.pixelRect.width;
            anchorMin.y /= _canvas.pixelRect.height;
            anchorMax.x /= _canvas.pixelRect.width;
            anchorMax.y /= _canvas.pixelRect.height;

            foreach (var saTransform in safeAreaTransformList)
            {
                saTransform.anchorMin = anchorMin;
                saTransform.anchorMax = anchorMax;
            }
        }

        #endregion

        
        #region Orientation: Changed

        private static void OrientationChanged()
        {
            _lastOrientation = Screen.orientation;
            _lastResolution.x = Screen.width;
            _lastResolution.y = Screen.height;

            IsLandscape = _lastOrientation == ScreenOrientation.LandscapeLeft ||
                          _lastOrientation == ScreenOrientation.LandscapeRight ||
                          _lastOrientation == ScreenOrientation.Landscape;
            onOrientationChange.Invoke();

        }

        #endregion
        
        #region Resolution: Changed

        private static void ResolutionChanged()
        {
            if (_lastResolution.x == Screen.width && _lastResolution.y == Screen.height)
                return;

            _lastResolution.x = Screen.width;
            _lastResolution.y = Screen.height;

            IsLandscape = Screen.width > Screen.height;
            onResolutionChange.Invoke();
        }
        
        #endregion

        #region SafeArea: Changed

        private static void SafeAreaChanged()
        {
            if (_lastSafeArea == Screen.safeArea)
                return;

            _lastSafeArea = Screen.safeArea;

            for (int i = 0; i < _helpers.Count; i++)
            {
                _helpers[i].ApplySafeArea();
            }
        }
        
        #endregion

        
        #region Canvas: GetSize

        public static Vector2 GetCanvasSize()
        {
            return _helpers[0]._rectTransform.sizeDelta;
        }
        
        #endregion

        #region SafeArea: GetSize

        public static Vector2 GetSafeAreaSize()
        {
            for (int i = 0; i < _helpers.Count; i++)
            {
                if (_helpers[i].safeAreaTransformList != null && 
                    _helpers[i].safeAreaTransformList[0] != null)
                {
                    return _helpers[i].safeAreaTransformList[0].sizeDelta;
                }
            }
        
            return GetCanvasSize();
        }

        #endregion
    }
}
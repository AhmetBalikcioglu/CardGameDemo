using UnityEngine;

namespace Main
{
    [CreateAssetMenu(fileName = "AppConfig", menuName = "App/Config", order = 1)]
    public class AppConfig : ScriptableObject
    {
        [Header("Test")]
        public bool TestEnabled;
    }
}
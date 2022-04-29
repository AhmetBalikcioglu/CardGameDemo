using Main.Data;
using UnityEngine;

namespace Main.Presentation
{
    public static class ResourceProvider
    {
        #region Get: CardSprite

        public static Sprite GetCardSprite(CardType type, CardValue value)
        {
            return Resources.Load<Sprite>($"Sprites/Cards/{type.ToString()}_{(int) value}");
        }
        
        #endregion

        #region Get: CardGameObject

        public static GameObject GetCardGameObject()
        {
            return Resources.Load<GameObject>($"Prefabs/UI/Card/CardView");
        }

        #endregion
    }
}
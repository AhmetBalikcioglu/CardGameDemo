using Main.Data;
using UnityEngine;

namespace Main.Presentation
{
    public static class ResourceProvider
    {
        public static Sprite GetCardSprite(CardType type, CardValue value)
        {
            return Resources.Load<Sprite>($"Sprites/Cards/{type.ToString()}_{(int) value}");
        }
    }
}
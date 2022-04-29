using System.Collections.Generic;
using Main.Game.Data;

namespace Main.Game.Application.Managers.Comparer
{
    public class DescendingGroupComparer : IComparer<List<CardInfo>>
    {
        public int Compare(List<CardInfo> x, List<CardInfo> y)
        {
            if (GetGroupTotalValue(y).CompareTo(GetGroupTotalValue(x)) != 0)
            {
                return GetGroupTotalValue(y).CompareTo(GetGroupTotalValue(x));
            }
            else
            {
                var isXAscendingGroup = x[0].Value != x[1].Value;
                return isXAscendingGroup ? 1 : 2;
            }
        }
    
        private int GetGroupTotalValue(List<CardInfo> cardGroup)
        {
            int totalValue = 0;
            foreach (var card in cardGroup)
            {
                var value = (int)card.Value;
                if (value > 10)
                {
                    value = 10;
                }
                totalValue += value;
            }

            return totalValue;
        }
    }
}
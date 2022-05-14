using System.Collections.Generic;
using Main.Game.Data;

namespace Main.Game.Application.Managers.Comparer
{
    public class DescendingGroupComparer : IComparer<List<CardInfo>>, IEqualityComparer<List<CardInfo>>
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

        public bool Equals(List<CardInfo> x, List<CardInfo> y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Capacity == y.Capacity && x.Count == y.Count;
        }

        public int GetHashCode(List<CardInfo> obj)
        {
            unchecked
            {
                return (obj.Capacity * 397) ^ obj.Count;
            }
        }
    }
}
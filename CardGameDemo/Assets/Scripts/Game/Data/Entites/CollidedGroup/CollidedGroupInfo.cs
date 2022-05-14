using System.Collections.Generic;

namespace Main.Game.Data
{
    public class CollidedGroupInfo
    {
        public List<CardInfo> Group { get; set; }
        public List<List<CardInfo>> CollidedCardGroups { get; set; }
        public List<CardInfo> CollidedCardList { get; set; }

        public CollidedGroupInfo(List<CardInfo> group, List<List<CardInfo>> collidedCardGroups, List<CardInfo> collidedCardsList)
        {
            Group = new List<CardInfo>(group);
            CollidedCardGroups = new List<List<CardInfo>>(collidedCardGroups);
            CollidedCardList = new List<CardInfo>(collidedCardsList);
        }
    }
}
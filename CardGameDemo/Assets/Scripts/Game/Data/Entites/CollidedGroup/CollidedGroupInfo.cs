using System.Collections.Generic;

namespace Main.Game.Data
{
    public class CollidedGroupInfo
    {
        public List<CardInfo> Group { get; set; }
        public List<ColliderGroupInfo> ColliderGroupInfoList { get; set; }

        public CollidedGroupInfo(List<CardInfo> group, List<ColliderGroupInfo> colliderGroupInfoList)
        {
            Group = new List<CardInfo>(group);
            ColliderGroupInfoList = new List<ColliderGroupInfo>(colliderGroupInfoList);
        }
    }

    public class ColliderGroupInfo
    {
        public CardInfo Card { get; set; }
        public List<CardInfo> Group { get; set; }
        
        public ColliderGroupInfo(List<CardInfo> group, CardInfo card)
        {
            Card = card;
            Group = new List<CardInfo>(group);
        }
    }
}
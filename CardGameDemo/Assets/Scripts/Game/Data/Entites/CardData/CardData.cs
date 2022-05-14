namespace Main.Game.Data
{
    public class CardInfo
    {
        public CardType Type { get; set; }
        public CardValue Value { get; set; }
        
        
        #region Override: Equals

        public override bool Equals(object obj)
        {
            if (obj is CardInfo another)
            {
                return Type == another.Type && Value == another.Value;
            }

            return base.Equals(obj);
        }

        #endregion
    }
}
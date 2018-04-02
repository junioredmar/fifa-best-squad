namespace FifaBestSquad
{
    public class Ligation
    {
        public bool IsGreen => ((Player1.Club == Player2.Club) || (Player1.Nation == Player2.Nation && Player1.League == Player2.League));

        public Player Player1 { get; set; }

        public PositionEnum PositionPlayer1 { get; set; }

        public Player Player2 { get; set; }

        public PositionEnum PositionPlayer2 { get; set; }
    }
}
namespace GolfGame.Data
{
    public class GolfHole
    {
        public int Par {  get; set; }
        public int Distance { get; set; }

        public GolfHole(int par, int distance)
        {
            Par = par;
            Distance = distance;
        }
    }
}
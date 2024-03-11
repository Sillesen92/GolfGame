namespace GolfGame.Data
{
    public class GolfClub
    {
        public string Name { get; set; }
        public double Distance { get; set; }

        public GolfClub(string name, double distance)
        {
            Name = name;
            Distance = distance;
        }
    }
}
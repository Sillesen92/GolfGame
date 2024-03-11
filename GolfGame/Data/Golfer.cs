using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfGame.Data
{
    public class Golfer
    {
        public string Name { get; set; }
        public List<GolfClub> GolfBag { get; set; }

        public Golfer(string name, List<GolfClub> golfBag)
        {
            Name = name;
            GolfBag = golfBag;
        }
    }
}
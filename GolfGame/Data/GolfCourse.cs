using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfGame.Data
{
    public class GolfCourse
    {
        public string Name { get; set; }
        public List<GolfHole> GolfHoles { get; set; } = new List<GolfHole>();
        public int Par { get; set; } = 0;
        public int Distance { get; set; } = 0;

        public GolfCourse(string name, List<GolfHole> golfHoles)
        {
            Name = name;
            GolfHoles = golfHoles;
            foreach (var hole in golfHoles)
            {
                Par += hole.Par;
                Distance += hole.Distance;
            }
        }
    }
}

// See https://aka.ms/new-console-template for more information
using GolfGame.Data;
using System.Security.Cryptography;

internal class Program
{
    private static GolfCourse? ChosenGolfCourse { get; set; }
    private static List<GolfClub> GolfClubs { get; set; } = [];
    private static Golfer? Golfer { get; set; }
    private static List<GolfHole> GolfHoles { get; set; } = [];
    private static List<GolfCourse> GolfCourses { get; set; } = [];
    private static List<int> ScoreCard { get; set; } = [];
    private static int RelationToPar = 0;
    private static void Main(string[] args)
    {
        LoadData();
        Console.WriteLine("Velkommen til Sillesen Golf Game 2024");
        Console.WriteLine();
        string? inputFromUser = ChooseGolfer();
        if (inputFromUser != null)
        {
            Golfer = new(inputFromUser, GolfClubs);
        }
        Console.WriteLine();
        if (Golfer != null)
        {
            Console.WriteLine($"Velkommen til {Golfer.Name}");
        }
        inputFromUser = ChooseGolfCourse();
        while (ChosenGolfCourse == null)
        {
            var succeeded = int.TryParse(inputFromUser, out int convertedInput);
            if (succeeded)
            {
                ChosenGolfCourse = GolfCourses[convertedInput - 1];
            }
            else
            {
                Console.WriteLine("Du skal vælge tallet ud for den golfbane du vil vælge");
                inputFromUser = ChooseGolfCourse();
            }
        }
        Console.Clear();
        Console.WriteLine($"Runden på {ChosenGolfCourse.Name} er begyndt");
        foreach(GolfHole hole in ChosenGolfCourse.GolfHoles)
        {
            PlayGolfHole(hole, Golfer);
            PrintScoreCard(ScoreCard, hole);
            Console.WriteLine("Tryk enter for næste hul");
            Console.ReadLine();
            Console.Clear();
        }
    }

    private static void PlayGolfHole(GolfHole hole, Golfer? chosenGolfer)
    {
        if (chosenGolfer != null)
        {
            double metersLeft = hole.Distance;
            int numberOfShotsUsed = 0;
            while (metersLeft > 0)
            {
                GolfClub? currentGolfClub = null;
                if(metersLeft < 60)
                {
                    if (metersLeft < 20)
                    {
                        Console.WriteLine($"Du er på green og har et {string.Format("{0:N2}", metersLeft)} meter put");
                        var numbersOfShotsBeforePutting = numberOfShotsUsed;
                        numberOfShotsUsed += CalculateThePutting(metersLeft);
                        Console.WriteLine($"Du har brugt {numberOfShotsUsed - numbersOfShotsBeforePutting} putts og har brugt {numberOfShotsUsed} på dette hul");
                        metersLeft = 0;
                    }
                    else
                    {
                        Console.WriteLine($"Du har {string.Format("{0:N2}", metersLeft)} meter tilbage, og vælger at pitche bolden");
                        metersLeft = CalculateThePitch(metersLeft);
                        numberOfShotsUsed += 1;
                    }
                }
                else
                {
                    Console.WriteLine("Hvilken golfkølle vil du vælge til dit næste slag?");
                    Console.WriteLine($"Du har {string.Format("{0:N2}", metersLeft)} meter til hullet");
                    Console.WriteLine("----------------------------------");
                    string? inputFromUser = ChooseGolfClub(chosenGolfer);
                    while (currentGolfClub == null)
                    {
                        var succeeded = int.TryParse(inputFromUser, out int convertedInput);
                        if (succeeded && convertedInput <= chosenGolfer.GolfBag.Count)
                        {
                            currentGolfClub = chosenGolfer.GolfBag[convertedInput - 1];
                        }
                        else
                        {
                            Console.WriteLine("Du skal vælge tallet ud for den golfkølle du vil vælge");
                            inputFromUser = ChooseGolfClub(chosenGolfer);
                        }
                    }
                    metersLeft -= CalculateTheGolfShot(currentGolfClub);
                    if (metersLeft < 0)
                    {
                        metersLeft = Math.Abs(metersLeft);
                    }
                    numberOfShotsUsed++;
                }                
            }
            ScoreCard.Add(numberOfShotsUsed);
            RelationToPar += numberOfShotsUsed - hole.Par;
        }
    }

    private static void PrintScoreCard(List<int> scoreCard, GolfHole hole)
    {
        Console.WriteLine();
        Console.WriteLine("----------------------------------");
        Console.WriteLine("Scorekort:");
        Console.WriteLine();
        for(int i = 0; i < scoreCard.Count; i++)
        {
            if (scoreCard[i] < GolfHoles[i].Par)
            {
                Console.WriteLine($"Hul {i + 1}: ({scoreCard[i]})");
                Console.WriteLine();
            }
            else if(scoreCard[i] > GolfHoles[i].Par)
            {
                Console.WriteLine($"Hul {i + 1}: |{scoreCard[i]}|");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"Hul {i + 1}:  {scoreCard[i]}");
                Console.WriteLine();
            }
        }
        if (RelationToPar >= 0)
        {
            Console.WriteLine($"Du er +{RelationToPar}");
        }
        else
        {
            Console.WriteLine($"Du er {RelationToPar}");
        }
        
        Console.WriteLine();
    }

    private static double CalculateThePitch(double metersLeft)
    {
        double randomNumber = RandomNumberGenerator.GetInt32(100);
        double maxDistanceOnGreen = 19.99;
        return maxDistanceOnGreen / 100 * randomNumber;
    }

    private static int CalculateThePutting(double metersLeft)
    {
        double randomNumber = RandomNumberGenerator.GetInt32(100);
        if(metersLeft < 3)
        {
            var makePercentage = 66;
            if(randomNumber > 100 - makePercentage)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }
        else if (metersLeft >= 3 && metersLeft < 15)
        {
            var makePercentage = 25;
            var threePutCountry = 10;
            if (randomNumber > 100 - makePercentage)
            {
                return 1;
            }
            else if(randomNumber > threePutCountry)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }
        else
        {
            var makePercentage = 10;
            var threePutCountry = 25;
            if (randomNumber > 100 - makePercentage)
            {
                return 1;
            }
            else if (randomNumber > threePutCountry)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }
    }

    private static double CalculateTheGolfShot(GolfClub currentGolfClub)
    {
        double maxDistance = currentGolfClub.Distance;
        double randomPercentage = RandomNumberGenerator.GetInt32(80, 100);
        double distance = maxDistance * randomPercentage / 100;
        Console.WriteLine($"Du har ramt bolden {randomPercentage}% af dit fulde potentiale");
        Console.WriteLine($"Derfor har du slået {distance} meter, hvor det maximale kunne have været {maxDistance} meter");
        return distance;
    }

    private static string? ChooseGolfClub(Golfer chosenGolfer)
    {
        PrintGolfClubs(chosenGolfer);
        var inputFromUser = Console.ReadLine();
        return inputFromUser;
    }

    private static void PrintGolfClubs(Golfer chosenGolfer)
    {
        for (int i = 0; i < chosenGolfer.GolfBag.Count; i++)
        {
            Console.WriteLine($"{i + 1}: {chosenGolfer.GolfBag[i].Name}");
            Console.WriteLine($"     Længst mulige slag: {chosenGolfer.GolfBag[i].Distance} meter");
        }
    }

    private static string? ChooseGolfCourse()
    {
        Console.WriteLine();
        PrintGolfCourses(GolfCourses);
        var inputFromUser = Console.ReadLine();
        return inputFromUser;
    }

    private static void PrintGolfCourses(List<GolfCourse> golfCourses)
    {
        Console.WriteLine("Hvilken golfbane vil du spille?");
        Console.WriteLine("----------------------------------");
        for (int i = 0; i < golfCourses.Count; i++)
        {
            Console.WriteLine($"{i + 1}: {golfCourses[i].Name}");
            Console.WriteLine($"     Par:    {golfCourses[i].Par}");
            Console.WriteLine($"     Længde: {golfCourses[i].Distance} meter");
        };
    }

    private static string? ChooseGolfer()
    {
        Console.Clear();
        Console.WriteLine("Hvad hedder du?");
        var inputFromUser = Console.ReadLine();
        return inputFromUser;
    }

    private static void LoadData()
    {
        GolfClubs.AddRange([
            new("Lob Wedge", 80),
            new("Sand Wedge", 95),
            new("Gap Wedge", 110),
            new("Pitching Wedge", 125),
            new("9 Iron", 135),
            new("8 Iron", 145),
            new("7 Iron", 155),
            new("6 Iron", 165),
            new("5 Iron", 175),
            new("4 Iron", 185),
            new("2 Iron", 210),
            new("3 Wood", 240),
            new("Driver", 265)
        ]);

        GolfHoles.AddRange([
            new(5, 480),
            new(3, 170),
            new(4, 415),
            new(3, 142),
            new(4, 245),
            new(5, 498),
            new(4, 225),
            new(4, 350),
            new(4, 320),
            new(3, 120),
            new(4, 230),
            new(5, 520),
            new(4, 410),
            new(3, 116),
            new(4, 330),
            new(3, 155),
            new(5, 485),
            new(4, 370)
        ]);

        GolfCourses.Add(
            new("Ikast Golfklub", GolfHoles)
            );
    }
}
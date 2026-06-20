// See https://aka.ms/new-console-template for more information

using IARRv3.Api.Domain;
using IARRv3.Api.Domain.Entities;
using IARRv3.Api.Domain.Services;
using RandomNameGeneratorLibrary;

namespace IARRv3.Api;

// generating network 
public static class Program
{
    public static void Main(string[] args)
    {
        const int p1 = 40, p2 = 30, p3 = 40;
        const int u1 = 25, u2 = 150, u3 = 50;
        // var generated = new TestGenerator(55, 110)
        //     .GenerateMonopolisticTrueRatings(40, 0, 0)
        //     .GenerateLowSpamRatings(50, 5, 20, 20)
        //     .GenerateUnverifiedRatings(40, 30, 25, 70);
        var generated = new TestGenerator(p1 + p2 + p3, u1 + u2 + u3)
            .GenerateMonopolisticTrueRatings(p1, u1, 0, 0)
            .GenerateLowSpamRatings(u2, p2, p1, u1)
            .GenerateUnverifiedRatings(u3, p3, p1 + p2, u1 + u2);
        var network = new Network
        {
            Users = generated.Users,
            Posts = generated.Posts,
            Ratings = generated.Ratings,
        };
        var emptyAggregations = network
            .Users
            .Select(user => user.Id)
            .ToDictionary(id => id,  id => new UserAggregation(id).GenerateNewEmpty());
        var emptyCache = new UserData([], []);
        var result = new HandleNewRatingsCpService(0.0000001, emptyAggregations, network ,emptyCache).UpdateNetworkScores();
        var scores = network.Posts.Select(post => post.EstimatedAccuracyScore ).ToList();
        Console.WriteLine($"\n[Final posts scores] with scores with 0 {scores.Count(score => score == 0)}");
        for (var i = 0; i < scores.Count; i++)
        {
            var score = scores[i];
            Console.Write($"{Math.Round(score, 4)}, ");
            if (i == p1 - 1 || i == p1 + p2 - 1) Console.WriteLine();
        }

        Console.WriteLine("\n[Final users reps]");
        for (var i = 0; i < result.Users.Count; i++)
        {
            var rep = result.Users[i];
            Console.Write($"{Math.Round(rep.ReputationScore, 4)}, ");
            if(i == p1 - 1 || i == p1 + p2 - 1) Console.WriteLine();
        }

        try
        {
            using var writer = new StreamWriter("scores.csv");
            Console.WriteLine("Wrote scores");
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error {e.Message}");
        }
    }
}

internal class TestGenerator
{
    public List<User> Users { get; private set; } = [];
    public List<Rating> Ratings { get; private set; } = [];
    public List<Post> Posts { get; private set; } = [];

    public TestGenerator(int postNbr, int userNbr)
    {
        var nameGenerator = new PersonNameGenerator();
        for (var i = 0; i < userNbr; i++)
        {
            Users.Add(new User(Guid.NewGuid(), nameGenerator.GenerateRandomFirstAndLastName()));
        }

        for (var i = 0; i < postNbr; i++)
        {
            Posts.Add(new Post(Guid.NewGuid(), 0));
        }
    }

    public TestGenerator
        GenerateLowSpamRatings(int userNbr, int postNbr, int postIndex, int userIndex) //(50, 5, 20) and postNbr == userNbr 
    {
        if (userIndex + userNbr > Users.Count) userNbr = Users.Count - postIndex;
        if (postIndex + postNbr > Posts.Count) postNbr = Posts.Count - postIndex;
        var step = 0;
        var hop = userNbr / postNbr;
        var randomRating = new Random();
        var randomRatingNbrGen =  new Random();
        for (var i = postIndex; i < postIndex + postNbr; i++)
        {
            var post = Posts[i];
            var maxValue = userNbr / postNbr;
            var ratingNbr = randomRatingNbrGen.Next((int)(maxValue * .6), maxValue + 1);
            var usableUsrIndexes = Enumerable.Range(userIndex  + step, hop).ToArray();
            Random.Shared.Shuffle(usableUsrIndexes);
            step += hop;
            for (var j = 0; j < ratingNbr; j++)
            {
                try
                {
                    Ratings.Add(item: new Rating(post.Id, Users.ElementAt(usableUsrIndexes[j]).Id, randomRating.Next(0, 3) / 10.0));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Spam Generation: {e.Message}");
                    throw;
                }
            }
        }
        return this;
    }

    public TestGenerator GenerateMonopolisticTrueRatings(int postNbr, int userNbr, int postIndex, int userIndex) 
    {
        if (postNbr > Posts.Count) postNbr = Posts.Count - postIndex;
        if (postNbr > Users.Count) postNbr = Users.Count - postIndex;
        // in monopolistic scenario a user votes for the majority of the posts max
        var randomRatingGen = new Random();
        var randomRatingNbrGen = new Random();
        for (var i = postIndex; i < postIndex + postNbr; i++)
        {
            // if(i == postIndex)  Console.WriteLine($"First user index = {i}");
            var ratingNbr = randomRatingNbrGen.Next((int)(postNbr * .3), (int) (postNbr * .6));
            var post = Posts[i];
            var usableUsrIndexes = Enumerable.Range(userIndex, userNbr).ToArray();
            Random.Shared.Shuffle(usableUsrIndexes);
            for (var j = 0; j < ratingNbr; j++)
            {
                try
                {
                    Ratings.Add(item: new Rating(
                        post.Id,
                        Users.ElementAt(usableUsrIndexes[j]).Id,
                        randomRatingGen.Next(8, 11) / 10.0)
                    );
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    // break;
                    throw;
                }

            }
        }
        return this;
    }

    public TestGenerator GenerateUnverifiedRatings(int userNbr, int postNbr, int postIndex, int userIndex) // 
    {
        if (userNbr > Users.Count) userNbr = Users.Count - postIndex;
        if (postNbr > Posts.Count) postNbr = Posts.Count - postIndex;
        var maxNumberOfRatingsPerUser = postNbr / 2; // revisit the second part of this equation 
        
        var randomRatingNbrGen = new Random(); // random is declared outside to prevent
        // the potential bug of generating the same number for every loop iteration 
        var randomRatingGen =  new Random();
        var usablePostIndexes = Enumerable.Range(postIndex, postNbr).ToArray();

        for (var i = userIndex; i  < userIndex + userNbr; i++)
        {
            var user = Users[i];
            // var span = CollectionsMarshal.AsSpan(usablePostIndexes);
            Random.Shared.Shuffle(usablePostIndexes);
            var ratingNbr = randomRatingNbrGen.Next(1, maxNumberOfRatingsPerUser + 1);
            for (var j = 0; j < ratingNbr; j++)
            {
                var randomRating = randomRatingGen.NextDouble();
                // new Random().NextDouble() may return a constant value across all iterations when inside a loop...
                var usableIndex = usablePostIndexes[j];
                Ratings.Add(
                    item: new Rating(Posts[usableIndex].Id, user.Id, randomRating));
                // handle exception when users indexes empty out
                // ...won't happen as max number of raters for 1 Post is always inferior to total
            }
        }

        return this;
    }
}

// var algorithm = new HandleNewRatingsCpService(0.00000001, );
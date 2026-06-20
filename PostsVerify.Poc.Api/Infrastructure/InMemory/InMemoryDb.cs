using System;
using System.Collections.Generic;
using System.Linq;
using PostsVerify.Poc.Api.Domain;

namespace PostsVerify.Poc.Api.Infrastructure.InMemory;

internal class InMemoryDb
{
    internal List<User> Users { get; private set; }
    internal List<Rating> Ratings { get; private set; }
    internal List<Post> Posts { get; private set; }

    private static InMemoryDb _inMemoryDb;

    internal static InMemoryDb GetInstance()
    {
        _inMemoryDb ??= new InMemoryDb();
        return new InMemoryDb();
    }

    private InMemoryDb()
    {
        GenerateDatabase(30, 10);
    }

    private void GenerateDatabase(int userNbr, int postNbr)
    {
        var nameGenerator = new PersonNameGenerator();
        for (var i = 0; i < userNbr; i++)
        {
            Users.Add(new User(Guid.NewGuid(), nameGenerator.GenerateRandomFirstAndLastName()));
        }

        for (var i = 0; i < postNbr; i++)
        {
            Posts.Add(new Post(Guid.NewGuid(),  0));
        }

        var ratio = (float)userNbr / postNbr;
        var maxNbrRatersPerPost = (int)(ratio * (userNbr * .1)); // revisit the second part of this equation 

        var randomRaterNbr = new Random(); // random is declared outside to prevent
        // the potential bug of generating the same number for every loop iteration 

        foreach (var post in Posts)
        {
            var usableUsrIndexes = Enumerable.Range(0, Users.Count + 1).ToList();
            usableUsrIndexes = Shuffler.Shuffle(usableUsrIndexes);
            var nbrRaters = randomRaterNbr.Next(1, maxNbrRatersPerPost + 1);
            for (var i = 0; i < nbrRaters; i++)
            {
                // new Random().NextDouble() may return a constant value across all iterations when inside a loop...
                Ratings.Add(
                    item: new Rating(post.Id, Users.ElementAt(usableUsrIndexes.First()).Id, new Random().NextDouble()));

                // handle exception when users indexes empty out
                // ...won't happen as max number of raters for 1 Post is always inferior to total
                try
                {
                    // "Pop" the used index from the list of possible user indexes
                    usableUsrIndexes.RemoveAt(0);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}
internal static class Shuffler 
{
    internal static List<int> Shuffle(List<int> usableUsrIndexes)
    {
        // Some shuffling logic or algorithm here
        int n = usableUsrIndexes.Count;
        while (n > 1)
        {
            n--;
            // Random.Shared.Next(n + 1) generates a valid random index
            int k = Random.Shared.Next(n + 1);
            
            // Swap values using a modern C# tuple
            (usableUsrIndexes[k], usableUsrIndexes[n]) = (usableUsrIndexes[n], usableUsrIndexes[k]);
        }
        return usableUsrIndexes;
    }
}
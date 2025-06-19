using PostsVerify.Poc.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace PostsVerify.Poc.Api.Infrastructure.Storage.Relational.EntityFrameworkCore;

public class PostsVerifyDbContext(DbContextOptions<PostsVerifyDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Source> Sources { get; set; }
    public DbSet<Area> Areas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasMany(e => e.CreatedPosts)
                .WithOne(p => p.CreatorUser)
                .HasForeignKey(p => p.CreatorUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.AuthoredPosts)
                .WithOne(p => p.AuthorUser)
                .HasForeignKey(p => p.AuthorUserId)
                .OnDelete(DeleteBehavior.SetNull);

        });
        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("Post");

            entity.HasOne(e => e.AuthorUser)
                .WithMany(u => u.AuthoredPosts)
                .HasForeignKey(e => e.AuthorUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.CreatorUser)
                .WithMany(u => u.CreatedPosts)
                .HasForeignKey(e => e.CreatorUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<Review>().ToTable("Review");
        modelBuilder.Entity<Source>().ToTable("Source");
        modelBuilder.Entity<Area>().ToTable("Area");
    }
}
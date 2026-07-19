using KnowledgeHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeHub.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(
    ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DocumentEmbedding>()
            .HasOne(x => x.DocumentChunk)
            .WithMany()
            .HasForeignKey(x => x.DocumentChunkId);
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Article> Articles => Set<Article>();

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<DocumentChunk> DocumentChunks => Set<DocumentChunk>();
    public DbSet<DocumentEmbedding> DocumentEmbeddings => Set<DocumentEmbedding>();
   

}




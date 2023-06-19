using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Database;

public partial class RtsDbContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Save> Saves { get; set; }
    public DbSet<Log> Logs { get; set; }

    public RtsDbContext()
    {
    }

    public RtsDbContext(DbContextOptions<RtsDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=178.250.159.71;Initial Catalog=RTS_DB;user id=andbear;password=MashaUdachi24;Trust Server Certificate=True; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

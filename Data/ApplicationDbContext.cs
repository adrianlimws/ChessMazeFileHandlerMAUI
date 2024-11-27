using ChessMazeFileHandlerMAUI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMazeFileHandlerMAUI.Data
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<FileOperation> FileOperations { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
			Database.EnsureCreated();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<FileOperation>()
				.HasIndex(f => f.Timestamp);
		}
	}
}

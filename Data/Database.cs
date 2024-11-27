using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ChessMazeFileHandlerMAUI.Data
{
	public class FileOperation
	{
		[Key]
		public int Id { get; set; }
		public string FilePath { get; set; }
		public string OperationType { get; set; } 
		public DateTime Timestamp { get; set; }
		public bool IsSuccessful { get; set; }
		public string? ErrorMessage { get; set; }
	}

	public class FileHandlerContext : DbContext
	{
		public DbSet<FileOperation> FileOperations { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Data Source=FileOperations.db");
		}
	}

	public class FileOperationTracker
	{
		private readonly FileHandlerContext _context;

		public FileOperationTracker()
		{
			_context = new FileHandlerContext();
			_context.Database.EnsureCreated();
		}

		public async Task TrackOperationAsync(string filePath, string operationType, bool isSuccessful, string? errorMessage = null)
		{
			var operation = new FileOperation
			{
				FilePath = filePath,
				OperationType = operationType,
				Timestamp = DateTime.Now,
				IsSuccessful = isSuccessful,
				ErrorMessage = errorMessage
			};

			_context.FileOperations.Add(operation);
			await _context.SaveChangesAsync();
		}

		public async Task<List<FileOperation>> GetRecentOperationsAsync(int count = 10)
		{
			return await _context.FileOperations
				.OrderByDescending(op => op.Timestamp)
				.Take(count)
				.ToListAsync();
		}

		public async Task<FileOperation?> GetMostRecentOperationAsync()
		{
			return await _context.FileOperations
				.OrderByDescending(op => op.Timestamp)
				.FirstOrDefaultAsync();
		}

		public async Task<List<FileOperation>> GetFailedOperationsAsync(int count = 10)
		{
			return await _context.FileOperations
				.Where(op => !op.IsSuccessful)
				.OrderByDescending(op => op.Timestamp)
				.Take(count)
				.ToListAsync();
		}

		public async Task<List<string>> GetMostAccessedFilesAsync(int count = 5)
		{
			return await _context.FileOperations
				.GroupBy(op => op.FilePath)
				.Select(group => new
				{
					FilePath = group.Key,
					AccessCount = group.Count()
				})
				.OrderByDescending(x => x.AccessCount)
				.Take(count)
				.Select(x => x.FilePath)
				.ToListAsync();
		}
	}
}

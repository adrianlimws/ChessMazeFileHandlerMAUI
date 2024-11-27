using ChessMazeFileHandlerMAUI.Data;
using ChessMazeFileHandlerMAUI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMazeFileHandlerMAUI.Services
{
	public class FileHistoryService
	{
		private readonly ApplicationDbContext _context;

		public FileHistoryService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task LogFileOperationAsync(string fileName, string filePath, string operationType)
		{
			var operation = new FileOperation
			{
				FileName = fileName,
				FilePath = filePath,
				OperationType = operationType,
				Timestamp = DateTime.Now
			};

			_context.FileOperations.Add(operation);
			await _context.SaveChangesAsync();
		}

		public async Task<List<FileOperation>> GetRecentOperationsAsync(int count = 10)
		{
			return await _context.FileOperations
				.OrderByDescending(f => f.Timestamp)
				.Take(count)
				.ToListAsync();
		}

		public async Task<IEnumerable<FileOperation>> GetOperationsByTypeAsync(string operationType)
		{
			return await _context.FileOperations
				.Where(f => f.OperationType == operationType)
				.OrderByDescending(f => f.Timestamp)
				.ToListAsync();
		}

		public async Task<IEnumerable<FileOperation>> GetOperationsForFileAsync(string fileName)
		{
			return await _context.FileOperations
				.Where(f => f.FileName == fileName)
				.OrderByDescending(f => f.Timestamp)
				.ToListAsync();
		}
	}
}

/* References
 * https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/
 * https://learn.microsoft.com/en-us/dotnet/api/system.data.entity.infrastructure.dbrawsqlquery.tolistasync?view=entity-framework-6.2.0#system-data-entity-infrastructure-dbrawsqlquery-tolistasync
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMazeFileHandlerMAUI.Data
{
	public class FileOperationViewModel
	{
		public string FilePath { get; set; }
		public string OperationType { get; set; }
		public DateTime Timestamp { get; set; }
		public bool IsSuccessful { get; set; }
	}
}

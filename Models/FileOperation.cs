using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMazeFileHandlerMAUI.Models
{
    public class FileOperation
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string OperationType { get; set; } // "Read" or "Write"
        public DateTime Timestamp { get; set; }
    }
}

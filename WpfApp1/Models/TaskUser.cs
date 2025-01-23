using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class TaskUser
    {
        public int Id { get; set; } // Первичный ключ
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public int AssignedToUserId { get; set; } // Внешний ключ
        public User AssignedToUser { get; set; } // Навигационное свойство
    }
}

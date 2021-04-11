using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleTask.Models
{
    public class TaskModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TaskId { get; set; }
        public string TaskName { get; set; }
        
        public Guid AssignedUserId { get; set; }
        public UserModel AssignedUser { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SimpleTask.Models
{
    public class TaskModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TaskId { get; set; }
        public string TaskName { get; set; }
        
        public Guid AssignedUserId { get; set; }
        
        [JsonIgnore]
        public virtual UserModel AssignedUser { get; set; }
    }
}
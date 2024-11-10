using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrammaGo.Server.Models
{
    public class ExerciseResult
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public long Id { get; set; }
        
        public long UserId { get; set; }

        public int AttemptNumber { get; set; }
        
        public int CorrectCount { get; set; }
        
        public int IncorrectCount { get; set; }
        
        public DateTime Date { get; set; }

        public int Level { get; set; } 
        
    }
}

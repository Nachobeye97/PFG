using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Para usar la anotación [Key]
using System.ComponentModel.DataAnnotations.Schema; // Para usar la anotación [DatabaseGenerated]

namespace GrammaGo.Server.Models
{
    public class Exercise
    {
        [Key] // Indica que esta es la clave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Indica que este campo se autogenera
        public long Id { get; set; }
        
        public required string Question { get; set; }
        
        public required List<string> Options { get; set; } // Lista de opciones  
        
        public required string Answer { get; set; } // Respuesta correcta
        
        public long UserId { get; set; } // ID del usuario que creó el ejercicio

        public int Level { get; set; } // Nivel del ejercicio, 1 para básico, 2 para avanzado
    }
}

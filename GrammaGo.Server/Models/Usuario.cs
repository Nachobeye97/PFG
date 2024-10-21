namespace GrammaGo.Server.Models
{
    public class Usuario
    {
        public long Id { get; set; }                   
        public required string Nombre { get; set; }              
        public required string Apellido { get; set; }           
        public required string CorreoElectronico { get; set; }   
        public required string ContrasenaHash { get; set; }      
        public DateTime FechaCreacion { get; set; }     
    }
}

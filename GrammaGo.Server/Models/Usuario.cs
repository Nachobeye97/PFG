namespace GrammaGo.Server.Models
{
  public class Usuario
{
    public long Id { get; set; }
    public required string Nombre { get; set; }
    public required string Apellido { get; set; }
    public required string CorreoElectronico { get; set; }
    public required string ContrasenaHash { get; set; }
    public string? Direccion { get; set; } 
    public string? Provincia { get; set; } 
    public string? CodigoPostal { get; set; } 
    public DateTime FechaCreacion { get; set; }
}

}

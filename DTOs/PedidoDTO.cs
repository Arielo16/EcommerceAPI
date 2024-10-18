using System.Collections.Generic;

namespace EcommerceAPI.DTOs
{
    public class PedidoDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public List<int> ProductosIds { get; set; } = new List<int>();
    }
}
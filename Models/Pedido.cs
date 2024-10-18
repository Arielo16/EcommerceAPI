using System.Collections.Generic;

namespace EcommerceAPI.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public List<int> ProductosIds { get; set; } = new List<int>();
        public virtual Usuario Usuario { get; set; }
    }
}
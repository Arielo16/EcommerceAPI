using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Models;
using EcommerceAPI.Data;
using EcommerceAPI.DTOs;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PedidosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoDTO>>> GetPedidos()
        {
            return await _context.Pedidos
                .Select(p => new PedidoDTO
                {
                    Id = p.Id,
                    UsuarioId = p.UsuarioId,
                    ProductosIds = p.ProductosIds
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoDTO>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return NotFound();

            return new PedidoDTO
            {
                Id = pedido.Id,
                UsuarioId = pedido.UsuarioId,
                ProductosIds = pedido.ProductosIds
            };
        }

        [HttpPost]
        public async Task<ActionResult<PedidoDTO>> PostPedido(PedidoDTO pedidoDto)
        {
            if (pedidoDto.Id == 0 || _context.Pedidos.Any(p => p.Id == pedidoDto.Id))
            {
                return BadRequest("ID inválido o ya existente.");
            }

            foreach (var productoId in pedidoDto.ProductosIds)
            {
                if (!await _context.Productos.AnyAsync(p => p.Id == productoId))
                {
                    return BadRequest($"Producto con ID {productoId} no existe");
                }
            }

            var pedido = new Pedido
            {
                Id = pedidoDto.Id, 
                UsuarioId = pedidoDto.UsuarioId,
                ProductosIds = pedidoDto.ProductosIds
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, pedidoDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedido(int id, PedidoDTO pedidoDto)
        {
            if (id != pedidoDto.Id) return BadRequest();

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return NotFound();

            foreach (var productoId in pedidoDto.ProductosIds)
            {
                if (!await _context.Productos.AnyAsync(p => p.Id == productoId))
                {
                    return BadRequest($"Producto con ID {productoId} no existe");
                }
            }

            pedido.ProductosIds = pedidoDto.ProductosIds;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return NotFound();

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.Id == id);
        }
    }
}

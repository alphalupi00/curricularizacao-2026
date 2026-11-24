#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; //modificações
using Microsoft.AspNetCore.Identity; //modificações
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DULCIESBOLOS.Data;
using DULCIESBOLOS.Models;

namespace DULCIESBOLOS.Controllers
{
    [Authorize] //Apenas usuários autenticados (Admin ou Cliente) podem acessar
    public class VendasController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<Usuario> _userManager;

        public VendasController(AppDBContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Métodos auxiliares de controle de identidade
        private string? ObterUsuarioIdAtual() => _userManager.GetUserId(User);
        private bool IsAdmin() => User.IsInRole("Admin");

        // Se Admin: lista todas as vendas de todos os clientes.
        // Se Cliente: lista APENAS os seus próprios pedidos.
        
        public async Task<IActionResult> Index()
        {
            var userId = ObterUsuarioIdAtual();
            var isAdmin = IsAdmin();

            var query = _context.Venda
                .Include(v => v.Bolo)
                .Include(v => v.Usuario)
                .Include(v => v.Pagamento)
                .AsQueryable();

            // filtra por usuário se não for Admin
            if (!isAdmin)
            {
                query = query.Where(v => v.UsuarioId == userId);
            }

            var vendas = await query.OrderByDescending(v => v.DataVenda).ToListAsync();
            return View(vendas);
        }

    
        // Proteção IDOR: Cliente só pode ver detalhes das próprias vendas.
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Venda
                .Include(v => v.Bolo)
                .Include(v => v.Usuario)
                .Include(v => v.Pagamento)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (venda == null)
            {
                return NotFound();
            }

            //Impede que um cliente veja a venda de outro pela URL
            var userId = ObterUsuarioIdAtual();
            if (!IsAdmin() && venda.UsuarioId != userId)
            {
                return Forbid();
            }

            return View(venda);
        }

       
        public async Task<IActionResult> Create()
        {
            await CarregarListasDropdownAsync();
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("boloId,Quantidade,formaPagamentoId")] Venda venda, int? AcrescimoId = null)
        {
            // Atribui obrigatoriamente o dono da venda a partir do usuário autenticado
            var usuarioAtual = await _userManager.GetUserAsync(User);
            if (usuarioAtual == null)
            {
                return Challenge();
            }
            venda.UsuarioId = usuarioAtual.Id;

            // 1. Busca e valida acréscimo se selecionado
            Acrescimo? acrescimo = null;
            decimal valorAcrescimo = 0m;
            if (AcrescimoId.HasValue)
            {
                acrescimo = await _context.Acrescimo.FindAsync(AcrescimoId.Value);
                if (acrescimo != null)
                {
                    if (acrescimo.Estoque < 1)
                    {
                        ModelState.AddModelError("AcrescimoId", "O acréscimo selecionado está esgotado no momento.");
                    }
                    else
                    {
                        valorAcrescimo = acrescimo.Valor;
                    }
                }
            }

            // 2. Busca e valida o bolo selecionado (estoque e cálculo de valor)
            Bolo? bolo = null;
            if (venda.boloId.HasValue)
            {
                bolo = await _context.Bolo.FindAsync(venda.boloId.Value);
                if (bolo != null)
                {
                    if (bolo.Estoque < venda.Quantidade)
                    {
                        ModelState.AddModelError("Quantidade", $"Estoque insuficiente! Temos apenas {bolo.Estoque} unidade(s) deste bolo.");
                    }
                    else
                    {
                        venda.Valor = ((decimal)bolo.Valor * venda.Quantidade) + valorAcrescimo;
                    }
                }
            }

            venda.DataVenda = DateTime.Now;
            venda.Status = "Pendente";

            // Remove validações automáticas de navegação e id para garantir integridade
            ModelState.Remove("Usuario");
            ModelState.Remove("Bolo");
            ModelState.Remove("Pagamento");
            ModelState.Remove("UsuarioId");

            if (ModelState.IsValid && bolo != null)
            {
                // 3. BAIXA NO ESTOQUE AUTOMÁTICA
                bolo.Estoque -= venda.Quantidade;
                if (acrescimo != null)
                {
                    acrescimo.Estoque -= 1;
                }

                _context.Add(venda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await CarregarListasDropdownAsync(venda.boloId, venda.formaPagamentoId);
            return View(venda);
        }

    
        // Proteção IDOR: Cliente só pode abrir a tela de edição dos seus próprios pedidos.
       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Venda
                .Include(v => v.Usuario)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venda == null)
            {
                return NotFound();
            }

            // Impede que um cliente edite o pedido de outro pela URL
            var userId = ObterUsuarioIdAtual();
            if (!IsAdmin() && venda.UsuarioId != userId)
            {
                return Forbid();
            }

            await CarregarListasDropdownAsync(venda.boloId, venda.formaPagamentoId, venda.UsuarioId);
            return View(venda);
        }

    
        // Proteção IDOR e Integridade:
        // - Garante que o cliente só edita o próprio pedido.
        // - Mantém o UsuarioId e DataVenda originais protegidos contra manipulação.
        // - Apenas Administrador pode alterar o Status do pedido.
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,boloId,Quantidade,formaPagamentoId,Status")] Venda venda)
        {
            if (id != venda.Id)
            {
                return NotFound();
            }

            // Busca a venda existente original no banco
            var vendaExistente = await _context.Venda.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
            if (vendaExistente == null)
            {
                return NotFound();
            }

            //  Cliente só pode salvar edição das suas próprias vendas
            var userId = ObterUsuarioIdAtual();
            var isAdmin = IsAdmin();
            if (!isAdmin && vendaExistente.UsuarioId != userId)
            {
                return Forbid();
            }

            // Preserva o UsuarioId e a DataVenda originais
            venda.UsuarioId = vendaExistente.UsuarioId;
            venda.DataVenda = vendaExistente.DataVenda;

            // Se o usuário não for Admin, preserva o Status original (impede cliente de forçar 'Entregue')
            if (!isAdmin)
            {
                venda.Status = vendaExistente.Status;
            }

            // Recalcula o total a partir do bolo e quantidade
            if (venda.boloId.HasValue)
            {
                var bolo = await _context.Bolo.FindAsync(venda.boloId.Value);
                if (bolo != null)
                {
                    venda.Valor = (decimal)bolo.Valor * venda.Quantidade;
                }
            }

            ModelState.Remove("Usuario");
            ModelState.Remove("Bolo");
            ModelState.Remove("Pagamento");
            ModelState.Remove("UsuarioId");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendaExists(venda.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            await CarregarListasDropdownAsync(venda.boloId, venda.formaPagamentoId, venda.UsuarioId);
            return View(venda);
        }

      
        // Cliente só pode ver a tela de exclusão do seu próprio pedido.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Venda
                .Include(v => v.Bolo)
                .Include(v => v.Usuario)
                .Include(v => v.Pagamento)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (venda == null)
            {
                return NotFound();
            }

            //  Impede que um cliente exclua o pedido de outro pela URL
            var userId = ObterUsuarioIdAtual();
            if (!IsAdmin() && venda.UsuarioId != userId)
            {
                return Forbid();
            }

            return View(venda);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venda = await _context.Venda.FindAsync(id);
            if (venda == null)
            {
                return NotFound();
            }

            //Impede exclusão não autorizada
            var userId = ObterUsuarioIdAtual();
            if (!IsAdmin() && venda.UsuarioId != userId)
            {
                return Forbid();
            }

            _context.Venda.Remove(venda);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendaExists(int id)
        {
            return _context.Venda.Any(e => e.Id == id);
        }

    
        private async Task CarregarListasDropdownAsync(int? boloId = null, int? formaPagamentoId = null, string? usuarioId = null)
        {
            var bolos = await _context.Bolo
                .OrderBy(b => b.Descricao)
                .ThenBy(b => b.Tamanho)
                .ToListAsync();

            var listaBolos = bolos.Select(b => new
            {
                Id = b.Id,
                TextoExibicao = $"{b.Descricao} - Tamanho {b.Tamanho.ToUpper()} (R$ {b.Valor:N2})"
            }).ToList();

            ViewData["boloId"] = new SelectList(listaBolos, "Id", "TextoExibicao", boloId);

            var pagamentos = await _context.Pagamento
                .OrderBy(p => p.descricao)
                .ToListAsync();

            ViewData["formaPagamentoId"] = new SelectList(pagamentos, "Id", "descricao", formaPagamentoId);

            var acrescimos = await _context.Acrescimo
                .OrderBy(a => a.Descricao)
                .ToListAsync();

            var listaAcrescimos = acrescimos.Select(a => new
            {
                Id = a.Id,
                TextoExibicao = $"{a.Descricao} ({a.Quantidade}) - R$ {a.Valor:N2}"
            }).ToList();

            ViewData["AcrescimoId"] = new SelectList(listaAcrescimos, "Id", "TextoExibicao");

            if (IsAdmin())
            {
                var usuarios = await _context.Users
                    .OrderBy(u => u.NomeCompleto)
                    .ToListAsync();

                ViewData["UsuarioId"] = new SelectList(usuarios, "Id", "NomeCompleto", usuarioId);
            }
        }
    }
}

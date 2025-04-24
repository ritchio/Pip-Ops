using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HelpDesk.Data;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Usuario.Interfaces;
using HelpDesk.Services.Usuario.Services;
using HelpDesk.Models.Entities;
using HelpDesk.Models.Utils;
using System.Text.Json;

namespace HelpDesk.Controllers
{
    public class CatalogoUsuariosController(
        ApplicationDbContext context,
        IUsuarioService usuarioService
            ) : Controller
    {
        #region [ Atributos ]
        private readonly ApplicationDbContext _context = context;
        private readonly IUsuarioService _usuarioService = usuarioService;

        #endregion [ Atributos ]


        // GET: CatalogoUsuarios
        public async Task<IActionResult> Index()
        {
            var usuarios = await _usuarioService.ObtenerUsuarios();
            return View(usuarios);
        }

        // GET: CatalogoUsuarios/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUsersViewModel = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetUsersViewModel == null)
            {
                return NotFound();
            }

            return View(aspNetUsersViewModel);
        }

        // GET: CatalogoUsuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CatalogoUsuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount,PrimerNombre,SegundoNombre,ApellidoPaterno,ApellidoMaterno,Activo,FechaActivacion,FechaInactivacion,IdPlaza,PlazaNombre")] AspNetUsersViewModel aspNetUsersViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aspNetUsersViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aspNetUsersViewModel);
        }

        // GET: CatalogoUsuarios/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _usuarioService.ObtenerUsuarioPorId(id);
            
            return View(response);
        }

        // POST: CatalogoUsuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,PrimerNombre,SegundoNombre,ApellidoMaterno,ApellidoPaterno,CorreoElectronico,Activo,IdPlaza")] AspNetUsersViewModel aspNetUsersViewModel)
        {
            if (id != aspNetUsersViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _usuarioService.Modificar(aspNetUsersViewModel);

                    if (response != null)
                    {
                        TempData["Notificacion"] = JsonSerializer.Serialize(new RespuestaGenerica<string>
                        {
                            Exito = true,
                            Mensaje = "Usuario actualizado con éxito.",
                        });
                    }
                    TempData["Notificacion"] = JsonSerializer.Serialize(new RespuestaGenerica<string>
                    {
                        Exito = true,
                        Mensaje = "Usuario actualizado con éxito.",
                    });
                    return View(response);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspNetUsersViewModelExists(aspNetUsersViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                return View(aspNetUsersViewModel);
            }
        }

        // GET: CatalogoUsuarios/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUsersViewModel = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetUsersViewModel == null)
            {
                return NotFound();
            }

            return View(aspNetUsersViewModel);
        }

        // POST: CatalogoUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var aspNetUsersViewModel = await _context.Users.FindAsync(id);
            if (aspNetUsersViewModel != null)
            {
                _context.Users.Remove(aspNetUsersViewModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AspNetUsersViewModelExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}

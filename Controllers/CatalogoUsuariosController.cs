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
using HelpDesk.Services.Plaza.Interfaces;
using HelpDesk.Services.Area.Interfaces;

namespace HelpDesk.Controllers
{
    public class CatalogoUsuariosController(
        ApplicationDbContext context,
        IUsuarioService usuarioService,
        IPlazaService plazaService,
        IAreaService areaService
            ) : Controller
    {
        #region [ Atributos ]
        private readonly ApplicationDbContext _context = context;
        private readonly IUsuarioService _usuarioService = usuarioService;
        private readonly IPlazaService _plazaService = plazaService;
        private readonly IAreaService _areaService = areaService;
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
        public async Task<IActionResult> Crear()
        {
            var usuariomodel = new AspNetUsersViewModel
            {
                CorreoElectronico = "",
                PrimerNombre = "",
                ApellidoPaterno = "",
                IdAreas = [],
                IdRol = ""
            };

            usuariomodel.Roles = await _context.Roles.ToListAsync();
            usuariomodel.Plazas = await _plazaService.ObtenerPlazasAsync();
            usuariomodel.Areas = await _areaService.ObtenerAreasAsync();

            var respuesta = new RespuestaGenerica<string>
            {
                Datos = this.RenderizarVista("_Crear", usuariomodel, true),
                Exito = true
            };

            return Ok(respuesta);
        }

        // POST: CatalogoUsuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(AspNetUsersViewModel aspNetUsersViewModel)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(aspNetUsersViewModel);
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                var usuarioCreado = await _usuarioService.Crear(aspNetUsersViewModel);
                if(usuarioCreado.Exito)
                {
                    var respuesta = new RespuestaGenerica<string>
                    {
                        Exito = true,
                        Mensaje = "Usuario creado con éxito.",

                    };

                    TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                    return RedirectToAction("Index");
                }
                else
                {
                    var respuesta = new RespuestaGenerica<string>
                    {
                        Exito = false,
                        Mensaje =usuarioCreado.Mensaje,
                        
                    };

                    TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                    return RedirectToAction("Index");

                }
                
            }
            else
            {
                var errores = ModelState
                .Where(ms => ms.Value.Errors.Any())
                .SelectMany(ms => ms.Value.Errors.Select(e => e.ErrorMessage))
                .ToList();

                var respuesta = new RespuestaGenerica<string>
                {
                    Datos = errores.ToString(),
                    Exito = false,
                    Mensaje = errores.ToString()
                };
                return Ok(errores);
            }

                
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

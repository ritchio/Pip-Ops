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
using HelpDesk.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace HelpDesk.Controllers
{
    [Authorize]
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


        [VerificaPermiso("Usuario.Ver")]
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

            var CatUsuarioViewModel = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (CatUsuarioViewModel == null)
            {
                return NotFound();
            }

            return View(CatUsuarioViewModel);
        }

        [VerificaPermiso("Usuario.Crear")]  
        public async Task<IActionResult> Crear()
        {
            var usuariomodel = new CatUsuarioViewModel
            {
                CorreoElectronico = "",
                PrimerNombre = "",
                ApellidoPaterno = "",
                IdAreas = [],
                IdRol = ""
            };

            usuariomodel.Roles = await _context.Roles.ToListAsync();
            var plazas = await _plazaService.ObtenerPlazasAsync();
            usuariomodel.Plazas = plazas.Where(p => p.Activo == true).ToList();
            var areas = await _areaService.ObtenerAreasAsync();
            usuariomodel.Areas = areas.Where(a => a.Activo == true && a.IdPlaza == usuariomodel.Plazas.First().IdPlaza).ToList();

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
        public async Task<IActionResult> Crear(CatUsuarioViewModel CatUsuarioViewModel)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(CatUsuarioViewModel);
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                var usuarioCreado = await _usuarioService.Crear(CatUsuarioViewModel);
                if(usuarioCreado.Exito)
                {
                    var respuesta = new RespuestaGenerica<string>
                    {
                        Exito = true,
                        Mensaje = "Usuario creado con éxito.",

                    };

                    TempData["Notificacion"] = JsonSerializer.Serialize(usuarioCreado);
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

        [VerificaPermiso("Usuario.Editar")]
        public async Task<IActionResult> Editar(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario= await _usuarioService.ObtenerUsuarioPorId(id);
            
            
            if(usuario.Datos == null)
            {
                return BadRequest();
            }
            else
            {
                var plazas = await _plazaService.ObtenerPlazasAsync();
                var areas = await _areaService.ObtenerAreasAsync();
                usuario.Datos.Roles = await _context.Roles.ToListAsync();
                usuario.Datos.Plazas = plazas.Where(p => p.Activo == true).ToList();    
                usuario.Datos.Areas = areas.Where(p => p.Activo == true).ToList();
            }

            var respuesta = new RespuestaGenerica<string>
            {
                Datos = this.RenderizarVista("_Editar", usuario.Datos, true),
                Exito = usuario.Exito,
                Mensaje = usuario.Mensaje
            };

            return Ok(respuesta);
            //return View("_Editar", usuario.Datos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(string id, CatUsuarioViewModel CatUsuarioViewModel)
        {
            if (id != CatUsuarioViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _usuarioService.Modificar(CatUsuarioViewModel);

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
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatUsuarioViewModelExists(CatUsuarioViewModel.Id))
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

        [VerificaPermiso("Usuario.Desactivar")]
        public async Task<IActionResult> CambiarEstado(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cambio = await _usuarioService.CambiarEstado(id);

            return Ok(cambio);
            
        }

        [VerificaPermiso("Usuario.Permisos")]
       
        public async Task<IActionResult> ObtenerPermisosUsuario(string idUsuario)
        {
            if (idUsuario == null)
            {
                return NotFound();
            }
            var permisos = await _usuarioService.ObtenerPermisosDeUsuario(idUsuario);

            var permisosUsuario = new PermisosPorUsuarioViewModel
            {
                IdUsuario = idUsuario,
                Permisos = permisos
            };
            var respuesta = new RespuestaGenerica<string>();

            if(permisos != null)
            {
                respuesta.Exito = true;
                respuesta.Datos = this.RenderizarVista("_PermisosUsuario", permisosUsuario, true);
            }
            else
            {
                respuesta.Exito = false;
                respuesta.Mensaje = "No se encontraron permisos para el usuario.";
            }
            return Ok(respuesta);
        }

        public async  Task<IActionResult> AsignarPermiso(string idUsuario, string permiso, bool asignar)
        {
            if (idUsuario == null)
            {
                return Ok(new RespuestaGenerica<string> { Exito = false, Mensaje = "Usuario no encontrado." });
            }
            else
            {
                try
                {
                    var respuesta = await _usuarioService.ActualizarPermisoUsuario(idUsuario, permiso, asignar);
                    return Ok(respuesta);
                }catch(Exception ex)
                {
                    return Ok(new RespuestaGenerica<string> { Exito = false, Mensaje = $"Woopsie! {ex.Message}" });

                }

            }
                
        }

        #region [ Privadas ]
        private bool CatUsuarioViewModelExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        #endregion [ Privadas ]
    }
}

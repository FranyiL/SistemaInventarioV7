using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Utilidades;
using SistemaInventarioV7.AccesoDatos.Data;

namespace SistemaInventarioV7.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Rol_Admin)]
    public class UsuarioController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly ApplicationDbContext _db;

        public UsuarioController(IUnidadTrabajo unidadTrabajo, ApplicationDbContext db)
        {
            _unidadTrabajo = unidadTrabajo;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos() 
        {
            var usuarioLista = await _unidadTrabajo.UsuarioAplicacion.ObtenerTodos();
            var userRol = await _db.UserRoles.ToListAsync();
            var roles = await _db.Roles.ToListAsync();

            foreach (var usuario in usuarioLista)
            {
                var roleId = userRol.FirstOrDefault(u=>u.UserId == usuario.Id).RoleId;

                usuario.Rol = roles.FirstOrDefault(u=>u.Id == roleId).Name;
            }

            return Json(new { data = usuarioLista });
        }

        [HttpPost]
        public async Task<IActionResult> BloquearDesbloquear([FromBody] string id) 
        {
            var usuario = await _unidadTrabajo.UsuarioAplicacion.ObtenerPrimero(u=>u.Id == id);

            if (usuario == null)
            {
                return Json(new {success = false, message = "Error de Usuario" });
            }

            if (usuario.LockoutEnd != null && usuario.LockoutEnd > DateTime.Now)
            {
                //Usuario bloqueado
                usuario.LockoutEnd = DateTime.Now;
            }
            else
            {
                usuario.LockoutEnd= DateTime.Now.AddYears(1000);
            }
            await _unidadTrabajo.Guardar();

            return Json(new { success = true, message = "Operación exitosa" });
        }

        #endregion
    }
}

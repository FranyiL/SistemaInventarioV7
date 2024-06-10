using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos.ViewModels;
using SistemaInventario.Utilidades;
using SistemaInventario.Modelos;
using System.Security.Claims;

namespace SistemaInventarioV7.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Rol_Admin)]
    public class CompaniaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public CompaniaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public async Task<IActionResult> Upsert() 
        {
            CompaniaVM companiaVM = new CompaniaVM()
            {
                Compania = new Compania(),
                BodegaLista = _unidadTrabajo.Inventario.ObtenerTodosDropdownLista("Bodega")
            };

            companiaVM.Compania = await _unidadTrabajo.Compania.ObtenerPrimero();

            if (companiaVM.Compania == null)
            {
                companiaVM.Compania = new Compania();
            }

            return View(companiaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(CompaniaVM companiaVM) 
        {
            if (ModelState.IsValid)
            {
                TempData[DS.Exitosa] = "Compañía grabada exitosamente";
                var claimIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

                if (companiaVM.Compania.Id == 0) //Crear la compañía
                {
                    companiaVM.Compania.CreadoPorId = claim.Value;
                    companiaVM.Compania.ActualizadoPorId = claim.Value;
                    companiaVM.Compania.FechaCreacion = DateTime.Now;
                    companiaVM.Compania.FechaActualizacion = DateTime.Now;

                    await _unidadTrabajo.Compania.Agregar(companiaVM.Compania);
                }
                else //Actualizar compañía
                {
                    companiaVM.Compania.ActualizadoPorId = claim.Value;
                    companiaVM.Compania.FechaActualizacion =DateTime.Now;

                    _unidadTrabajo.Compania.Actualizar(companiaVM.Compania);
                }

                await _unidadTrabajo.Guardar();

                return RedirectToAction("Index", "Home", new { area = "Inventario"});
            }

            TempData[DS.Error] = "Error al grabar compañía";
            return View(companiaVM);
        }
    }
}

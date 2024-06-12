using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Modelos.ViewModels;
using SistemaInventario.Utilidades;
using System.Security.Claims;

namespace SistemaInventarioV7.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class CarroController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;
        
        [BindProperty]
        public CarroCompraVM carroCompraVM { get; set; }

        public CarroController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            //Capturamos el usuario conectado
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            carroCompraVM = new CarroCompraVM();
            carroCompraVM.Orden = new SistemaInventario.Modelos.Orden();
            carroCompraVM.CarroCompraLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(u=>u.UsuarioAplicacionId == claim.Value,
                                                                                           incluirPropiedades:"Producto");
            carroCompraVM.Orden.TotalOrden = 0;
            carroCompraVM.Orden.UsuarioAplicacionId = claim.Value;

            foreach (var lista in carroCompraVM.CarroCompraLista)
            {
                lista.Precio = lista.Producto.Precio; //Siempre mostrar el precio actual del producto   
                carroCompraVM.Orden.TotalOrden += (lista.Precio * lista.Cantidad);
            }
            return View(carroCompraVM);
        }

        public async Task<IActionResult> mas(int carroId) 
        {
            var carroCompras = await _unidadTrabajo.CarroCompra.ObtenerPrimero(c=>c.Id == carroId);
            carroCompras.Cantidad += 1;
            await _unidadTrabajo.Guardar();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> menos(int carroId)
        {
            var carroCompras = await _unidadTrabajo.CarroCompra.ObtenerPrimero(c => c.Id == carroId);

            if (carroCompras.Cantidad == 1)
            {
                //Remover el registro del carro de compras y actualizamos la sesión
                var carroLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(c=>c.UsuarioAplicacionId == carroCompras.UsuarioAplicacionId);

                var numeroProducto = carroLista.Count();
                _unidadTrabajo.CarroCompra.Remover(carroCompras);
                await _unidadTrabajo.Guardar();
                //Actualizar sesión
                HttpContext.Session.SetInt32(DS.ssCarroCompras,numeroProducto - 1);
            }
            else
            {
                carroCompras.Cantidad -= 1;
                await _unidadTrabajo.Guardar();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> remover(int carroId) 
        {
            var carroCompras = await _unidadTrabajo.CarroCompra.ObtenerPrimero(c => c.Id == carroId);

            //Remover el registro del carro de compras y actualizamos la sesión
            var carroLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(c => c.UsuarioAplicacionId == carroCompras.UsuarioAplicacionId);

            var numeroProducto = carroLista.Count();
            _unidadTrabajo.CarroCompra.Remover(carroCompras);
            await _unidadTrabajo.Guardar();
            //Actualizar sesión
            HttpContext.Session.SetInt32(DS.ssCarroCompras, numeroProducto - 1);
            return RedirectToAction("Index");
        }
    }
}

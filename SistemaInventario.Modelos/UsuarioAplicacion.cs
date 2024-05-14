using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos
{
    public class UsuarioAplicacion : IdentityUser
    {
        [Required(ErrorMessage ="Los nombres son requeridos.")]
        [MaxLength(80)]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Los apellidos son requeridos.")]
        [MaxLength(80)]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "La dirección es requerido.")]
        [MaxLength(200)]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "La ciudad es requerida.")]
        [MaxLength(60)]
        public string Ciudad { get; set; }

        [Required(ErrorMessage = "El país es requerido.")]
        [MaxLength(60)]
        public string Pais { get; set; }

        [NotMapped] //Utilizamos esa función para que no se nos agregue en la tabla.
        public string Rol { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Número de serie es requerido.")]
        [MaxLength(60,ErrorMessage = "El número de serie es de un máximo de 60 carácteres.")]
        public string NumeroSerie { get; set; }

        [Required(ErrorMessage = "Descripción es requerida.")]
        [MaxLength(100,ErrorMessage = "El máximo de la descripción es de 100 carácteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es requerido.")]
        public double Precio { get; set; }

        [Required(ErrorMessage = "El costo es requerido.")]
        public double Costo { get; set; }

        public string ImagenUrl { get; set; }

        [Required(ErrorMessage = "Estado es requerido.")]
        public bool Estado { get; set; }

        [Required(ErrorMessage = "La categoria es requerida.")]
        public int CategoriaId { get; set; }

        //Modelo al que se relaciona la categoría del producto.
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }

        [Required(ErrorMessage = "La marca es requerida")]
        public int MarcaId { get; set; }

        [ForeignKey("MarcaId")]
        public Marca Marca { get; set; }
        
        //El sigo de interrogación delante del tipo de dato nos indica que esa propiedad puede ser nula.
        public int? PadreId { get; set; }

        //Creamos la navegación para la recursividad
        public virtual Producto Padre{ get; set; }
    }
}

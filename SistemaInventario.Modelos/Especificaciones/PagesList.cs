using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos.Especificaciones
{
    public class PagesList<T> : List<T>
    {
        public MetaData MetaData { get; set; }

        public PagesList(List<T> items, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize) //Con esto podemos redondear. 1.5 Lo transforma a dos
            };

            AddRange(items); //Agregar los elementos de la colección al final de la lista
        }

        public static PagesList<T> ToPagesList(IEnumerable<T> entidad, int pageNumber, int pageSize) 
        {
            var count = entidad.Count();
            var items = entidad.Skip((pageNumber-1)*pageSize).Take(pageSize).ToList(); 
            
            return new PagesList<T>(items, count, pageNumber, pageSize);
        }
    }
}

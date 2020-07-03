using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDataAccess;

namespace DataAccess
{
    public class MenuDataAccess : IMenuDataAccess
    {
        public List<string> GetItems()
        {
            List<string> ret = new List<string>
            {
                "Alta de genero (AG)",
                "Borrar genero (BG)",
                "Modificar genero (MG)",
                "Alta de pelicula (AP)",
                "Baja de pelicula (BP)",
                "Modificar pelicula (MP)",
                "Asociar pelicula a genero (AS)",
                "Desasociar pelicula a genero (DS)",
                "Alta de director (AD)",
                "Baja de director (BD)",
                "Asociar director y pelicula (DM)",
                "Desasociar director y pelicula (DD)",
                "Modificar director (MD)",
                "Subir archivos de pelicula (SA)",
                "Salir (FF)"
            };
            return ret;
        }
    }
}

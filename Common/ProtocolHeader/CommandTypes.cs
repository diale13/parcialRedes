using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum CommandType
    {
        AG, //Alta genero
        BG, //Bajar genero
        MG, //Modificar genero
        AP, //Alta pelicula
        BP, //Baja pelicula
        MP, //Modificar pelicula
        AS, //Asociar Pelicula a genero
        DS, //Desasociar pelicula a genero
        AD, //Alta director
        BD, //Baja director
        MD, //Modificar director
        DM, //Asociar director y pelicula
        DD, //Desasociar director y pelicula
        SA, //Subir archivo de pelicula
        FF, //Exit
    }
}

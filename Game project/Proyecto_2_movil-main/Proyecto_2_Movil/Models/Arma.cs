using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_2_Movil.Models
{
    public class Arma
    {
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public int AtaqueBase { get; set; }

        // Propiedades de comportamiento
        public string Tipo { get; set; } = "";
        public bool EsDistancia { get; set; } = false;
        public bool CausaSangrado { get; set; } = false;
        public bool EsMagico { get; set; } = false;
        public string? Elemento { get; set; } = null; // "Fuego","Tierra","Aire","Agua"
        public int BonusDistanciaPercent { get; set; } = 0; // ej 20 => +20% daño a distancia
        public bool PierdeVidaAlAtacar { get; set; } = false; // para puños (bestia)
    }
}




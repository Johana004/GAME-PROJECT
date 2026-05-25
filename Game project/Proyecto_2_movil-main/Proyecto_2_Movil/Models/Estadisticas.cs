namespace Proyecto_2_Movil.Models
{
    public class EstadisticaJugador
    {
        public string Nombre { get; set; } = string.Empty;
        public int Ganadas { get; set; }
        public int Perdidas { get; set; }
        public int Empates { get; set; }

        public int Jugadas => Ganadas + Perdidas + Empates;

        public double PorcentajeVictoria =>
            Jugadas == 0 ? 0 : (double)Ganadas / Jugadas * 100;
    }
}

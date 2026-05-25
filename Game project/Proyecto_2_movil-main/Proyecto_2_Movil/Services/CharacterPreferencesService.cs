using Microsoft.Maui.Storage;
using Proyecto_2_Movil.Models;
using System.Text.Json;

namespace Proyecto_2_Movil.Services
{
    public static class CharacterPreferencesService
    {
        private const string KEY_PERSONAJE = "PERSONAJE_SELECCIONADO";

        public static void Guardar(Personaje personaje)
        {
            if (personaje == null) return;

            var json = JsonSerializer.Serialize(personaje);
            Preferences.Set(KEY_PERSONAJE, json);
        }

        public static Personaje? Obtener()
        {
            if (!Preferences.ContainsKey(KEY_PERSONAJE))
                return null;

            var json = Preferences.Get(KEY_PERSONAJE, string.Empty);
            return JsonSerializer.Deserialize<Personaje>(json);
        }

        public static void Limpiar()
        {
            Preferences.Remove(KEY_PERSONAJE);
        }
    }
}

# GAME-PROJECT
# Warriors EITHARJAVAL – Documentación Técnica

¡Bienvenido al repositorio de **Warriors EITHARJAVAL**! Este es un videojuego de combate por turnos desarrollado bajo una arquitectura robusta, limpia y extensible. A continuación, se detalla el funcionamiento del sistema, sus mecánicas de juego, arquitectura y componentes clave.

 1. Reglas del Juego Implementadas

Warriors EITHARJAVAL es un simulador de combate táctico por turnos entre dos jugadores. La personalización de los personajes (Raza y Arma) altera significativamente las estadísticas y la estrategia en el campo de batalla.

Reglas Generales
* Dinámica por Turnos:** El combate se desarrolla de forma estrictamente alterna. En su turno, cada jugador puede ejecutar una única acción.
* Sistema de Posicionamiento:** El enfrentamiento inicia con una distancia inicial establecida entre ambos contendientes.
* Condición de Victoria:** El juego finaliza de manera inmediata cuando la vida de uno de los personajes se reduce a `0`.

Acciones Disponibles
1. Avanzar: Reduce la distancia respecto al oponente, propiciando el combate cuerpo a cuerpo.
2. Retroceder: Incrementa la distancia respecto al rival, útil para evadir ataques de corto alcance.
3. Atacar: Inflige daño al oponente, sujeto a las restricciones de alcance del arma equipada.
4. Sanar: Restaura puntos de salud según las propiedades curativas de la raza seleccionada.

Mecánica de Distancia y Armas
* Distancia = 0: Habilita el combate cuerpo a cuerpo (*Melee*).
Distancia > 0:Restringe los ataques únicamente a armas de largo alcance (ej. Arcos). Si un arma no cuenta con el alcance válido para la distancia actual, el ataque fallará automáticamente.

---

 2. Lógica de Turnos

La gestión del flujo y la secuenciación del combate se encuentra centralizada en el `CombateViewModel`.

### Máquina de Estados de Turnos
El estado del turno se controla a través de la enumeración `TurnoJugador`:
* `Jugador1`
* `Jugador2`

1. Inicialización: El combate siempre arranca asignando el primer movimiento al **Jugador 1**.
2. Ciclo de Turno: Al ejecutar con éxito cualquier acción válida, el control pasa de forma automática al rival y la interfaz de usuario actualiza dinámicamente el indicador de turno.
3. Disparadores de Cambio: Ejecución de un ataque válido.
   * Modificación de posición (Avanzar / Retroceder).
   * Uso de habilidad de curación.
4. Bloqueo Final: Si la salud de algún jugador llega a cero, el estado del juego cambia a "Finalizado" y se congelan las acciones, impidiendo cambios de turno posteriores.

---

3. Diagrama de Arquitectura y Flujo

Arquitectura General (MVVM)
El proyecto se rige estrictamente por el patrón de arquitectura **MVVM (Model-View-ViewModel)**, garantizando el desacoplamiento de la interfaz gráfica respecto a la lógica de negocio.

```
┌─────────────────────────────────┐
│              Views              │ <─── Interfaz Gráfica (XAML)
│          (XAML Pages)           │
└────────────────┬────────────────┘
                 │ Data Binding / Notificaciones de Propiedades
                 ▼
┌─────────────────────────────────┐
│           ViewModels            │ <─── Lógica de UI y Estado de Vista
│          (Lógica UI)            │
└────────────────┬────────────────┘
                 │ Consume / Modifica
                 ▼
┌─────────────────────────────────┐
│             Models              │ <─── Entidades del Dominio (Raza, Arma, Jugador)
│            (Dominio)            │
└────────────────┬────────────────┘
                 │ Hace uso de
                 ▼
┌─────────────────────────────────┐
│            Services             │ <─── Persistencia y Capas de Soporte
│          (Persistencia)         │
└─────────────────────────────────┘
```

Flujo del Juego
El ciclo de vida de una partida sigue la siguiente secuencia lineal:

```
Inicio ──> Registro de Jugadores ──> Selección de Raza ──> Selección de Arma ──> Combate ──> Resumen de Partida
```

---

 4. Lógica de Combate

El motor de combate calcula los resultados en tiempo real evaluando de forma combinada la salud actual, la distancia, el modificador del arma y los rasgos inherentes de la raza.

 Sistema de Daño
* Rango Aleatorio: El daño base está determinado por rangos mínimos y máximos ponderados.
* Modificadores de Armas: Ciertas armas integran lógicas probabilísticas para la activación de:
  * Probabilidad de fallo (*Miss*).
  * Golpes críticos (Multiplicador de daño).
  * Golpes dobles (Ataques sucesivos en el mismo turno).
* Modificadores de Raza: Las razas pueden alterar el daño final neto o inyectar efectos pasivos a la ofensiva.

Sistema de Curación
La capacidad de regeneración de salud es asimétrica y responde estrictamente a la naturaleza biológica o mágica de la raza:

| Raza | Capacidad / Mecánica de Curación |
| :--- | :--- |
| Humano| Recuperación estándar (~45%) |
| Elfo | Recuperación alta (~65%) |
| Elfo (Agua) | Sintonía elemental superior (Restaura entre 75% y 90%) |
| Orco | Requiere el uso/consumo de una poción |
| Bestia| Recuperación pasiva basada en mecánicas de sueño |

*Nota: Ninguna acción de sanación puede sobrepasar el límite de vida máxima establecido para el personaje.*

---

5. Mecanismo de Persistencia

La gestión y almacenamiento de datos se centraliza en el componente `PersistenciaService`.

Elementos Persistidos
* **Perfil de Jugador:** Nombre, Raza seleccionada, Arma seleccionada y Vida actual.
* **Historial Estadístico:** Registro acumulativo de partidas Ganadas, Perdidas y Empatadas.

Estructura de Datos Actual
Para efectos del entorno académico actual, se emplean colecciones en memoria volatiles (`List<Jugador>`) donde cada instancia mantiene referencias estructuradas a sus respectivos objetos `Raza` y `Arma`.

Justificación de Diseño y Escalabilidad
Esta arquitectura fue seleccionada por su simplicidad, claridad y su inmediata compatibilidad con la capa de ViewModels a través de enlaces de datos. Su diseño modular permite migrar el almacenamiento de manera transparente hacia:
* Archivos planos estructurados (JSON / XML)
* Bases de datos relacionales locales (SQLite)
* APIs o servicios en la nube (Persistencia externa)

---

 6. Manejo de Estados Especiales

El motor evalúa al inicio y cierre de cada turno las interacciones complejas entre razas y armas que dan origen a estados alterados:
* Sangrado (Bleed): Inflige daño degenerativo acumulacional constante por cada turno activo.
* Golpes Críticos: Incrementos exponenciales de daño basados en suerte o perks.
* Evasión (Dodge): Porcentaje probabilístico de esquivar por completo un ataque entrante.

---

7. Componentes Visuales y Recursos (Imágenes)

La interfaz se adapta visualmente de forma dinámica. En lugar de cablear imágenes estáticas, el sistema utiliza un **`ValueConverter`** personalizado en la capa de la vista.

Este convertidor toma la propiedad `Raza` del modelo del jugador y la traduce dinámicamente en la ruta del archivo de imagen correspondiente:
* `Humano` ──> `Assets/Images/humano.png`
* `Elfo` ──> `Assets/Images/elfo.png`
* `Elfo (Agua)` ──> `Assets/Images/elfo_agua.png`
* `Orco` ──> `Assets/Images/orco.png`
* `Bestia` ──> `Assets/Images/bestia.png`

---

8. Conclusión Técnica y Roadmap

El núcleo del proyecto Warriors EITHARJAVAL demuestra la correcta implementación de:
* Arquitectura limpia mediante el patrón MVVM.
* Separación estricta de responsabilidades (*SoC*).
* Sistema determinista de turnos y mitigación de errores de estado.
* Sólida comunicación e integración entre la UI y la capa lógica.

Próximos Pasos (Roadmap de Desarrollo)
1. Multijugador: Implementación de juego en red local o mediante WebSockets.
2. Persistencia Avanzada: Sustitución de las listas en memoria por un motor SQLite local.
3. Mejoras Estéticas: Incorporación de animaciones de transición, efectos de partículas para estados especiales y alertas visuales de daño.

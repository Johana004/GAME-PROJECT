# Documentación Técnica – Warriors EITHARJAVAL

## 1. Reglas del Juego Implementadas

El juego **Warriors EITHARJAVAL** es un combate por turnos⁠ entre dos jugadores.
Cada jugador controla un personaje definido por una **raza** y un **arma**, lo cual influye directamente en el desarrollo del combate.

### Reglas generales
- El combate se desarrolla por **turnos⁠ alterons**.
- En cada turno, el⁠ jugador puede ejecutar **una acción**.
- El combate inicia con una **distancia inicial** entre los jugadores.
- El juego termina cuando⁠ la vida de uno de los jugadores llega a **0**.

### Acciones disponibles
- **Avanzar:** Reduce la distancia entre los jugdaores.
- **Retroceder:** Aumenta la distancia.
- **Atacar:** Inflige daño al oponente (si la distancia lo permite).
- **Sanar:** Recupera vida según la raza del jugador.

### Reglas según distancia
- **Distancia 0:** Combate cuerpo a cuerpo.
-⁠ **Distancia > 0:** Solo algunas armas pueden atacar (ej. arco).
- Si la distancia no es váliad para el arma, el ataque falla.

---

## 2. Lógica de Turnos

La lógica de turnos está implementada en el **CombateViewModel**.

### Funcionamiento
- Se utiliza una enumeración `TurnoJugador`:
- `Jugador1`
- `Jugador2`
- El turno inicia siempre con el **Jugador 1**.
- Al finalizar una acción válida:
- El turno cambia automáticamente al otro jugador.
- El texto de turno se actualiza dinámicamente para reflejar quién juega.

### Cambio de turno
El turno cambia cuando:
- Se⁠ ejecuta un ataque válido.
- Se avanza o retrocede.
- Se realiza una curación.

Si el combate ha terminado, **no se permite cambiar el turno**.

---

## 3. Diagrama Explicativo Simple

### Arquitectura General (MVVM)
┌───────────────┐
│ Views │
│ (XAML Pages) │
└───────▲───────┘
│ Binding
┌───────┴───────┐
│ ViewModels │
│ (Lógica UI) │
└───────▲───────┘
│ Uso
┌───────┴───────┐
│ Models │
│ (Dominio) │
└───────▲───────┘
│
┌───────┴───────┐
│ Services │
│ Persistencia │
└───────────────┘

### Flujo⁠ del Juego

Inicio
↓
Registro de Jugadores
↓
Selección de Raza
↓
Selección de Arma
↓
Combate
↓

Resumen de Partida


---

## 4. Lógica de Combate

La lógica del combate se basa en:
- Vida actual de cada jugador.
- Distancia entre jugadores.
- Tipo de⁠ arma seleccionada.
- Raza del personaje.

###⁠ Daño
- El daño es *a*leatorio dentro de rangos definidos**.
- Algunas armas tienen:
- Probabilidad de fallo.
- Golpes⁠ críticos.
- Golpes dobles.
- Las razas pueden modificar⁠ el daño o aplicar efectos especiales.

### Curación
- La curación depende de la raza:
- Humano: ~45%
- Elfo: ~65%
- Elfo (Agua): 75–90%
 - Orco: Curación con poción
- Bestia: Curación al dormir
- No⁠ se puede superar la vida⁠ máxima permitida.

---

## 5. Mecanismo de Persistencia

La persistencia se implementa mediante un **servicio local**, encapsulado en la clase `PersistenciaService`.

### Qué se persiste
- Lista de jugadores.
-⁠ Nombre del jugador.
- Raza seleccionada.
- Arma seleccionada.
- Vida actual.
- Estadísticas de partidas:
 - Ganadas
- Perdidas
 - Empates

### Estructura⁠ de datos
- Se utilizan **colecciones en memoria** (`List<Jugador>`).
- Cada jugador contiene referencias a:
- `Raza`
- `Arma`

### Motivo de la elección
- Simplicidad y claridad para un proyecto académico.
- Fácil⁠ integración con ViewModels.
- Permite extenderse fácilmente a:
- Archivos JSON
 - Base de datos local
- Persistencia externa

---

## 6. Manejo de Estados Especiales

Algunas combinaciones de raza y arma pueden generar:
- **Sangrado:** Daoñ adicional por turnos.
- **Golpes críticos:** Incremento de daño.
- **Evasión:** Probabilidad de evitar daño.

Estos⁠ estados son evaluados y actualizados en⁠ cada turno.

---

## 7. Imágeens de Personajes

Las imágenes de los⁠ personajes se asignan dinámicamente según la raza:
- Humano
- Elfo
- Elfo (Agua)
- Orco
- Bestia

La asignación se realiza mediante un **ValueConverter**, el cual traduce la raza en el nombre del archivo de imagen correspondiente.

---

## 8. Conclusión Técnica

El proyecto implementa correctamente:
-⁠ Patrón MVVM.
- Separación de responsabilidades.
- Lógica⁠ de combate por turnos.
- Persistencia de datos.
-⁠ Integración completa entre UI y lógica.

El sistema está preparado para futuras mejoras del mismo modo que:
- Combate multijugador.
- Persistencia⁠ avanzada.
- Animaciones y efectos visuales.


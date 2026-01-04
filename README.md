# animal-memory-maui
App educativa en .NET MAUI para niños. Combina juego de memoria con aprendizaje interactivo: 
empareja cartas, escucha nombres en inglés y responde preguntas sobre animales. 
Diseñado como modelo de ejemplo escalable para añadir niveles, preguntas y especies.  
Sirve para que los niños aprendan sobre la naturaleza de forma divertida y visual.

---

# Animal Memory

## Descripción del proyecto
Animal Memory es una aplicación educativa desarrollada en .NET MAUI y pensada
principalmente para niños pequeños.  

El objetivo del proyecto es que los niños aprendan sobre los animales de una forma
divertida, visual e interactiva, combinando un juego de memoria con preguntas sencillas
sobre las características de los animales.

Mientras juegan, los niños emparejan cartas de animales, escuchan su nombre en inglés
y responden preguntas relacionadas con los animales que van encontrando.

Este proyecto se ha desarrollado como un modelo de ejemplo, que podría ampliarse
fácilmente añadiendo más animales, más preguntas y nuevos niveles.

---

## Tecnologías utilizadas
- .NET MAUI
- C#
- XAML
- Arquitectura MVVM
- CommunityToolkit.MVVM
- Plugin.Maui.Audio
- Text-to-Speech (Microsoft.Maui.Media)
- Gestos (Tap, Drag & Drop)
- ObservableCollection
- Commands (ICommand)

---

## Interfaz natural implementada
La aplicación utiliza elementos de interfaz natural adaptados a niños pequeños, 
con el objetivo de hacer el aprendizaje más intuitivo y accesible.

Por un lado, se utiliza síntesis de voz (Text-to-Speech) como apoyo al aprendizaje.
Cada vez que el jugador gira una carta, la aplicación pronuncia en voz alta el nombre 
del animal en inglés, ayudando al niño a familiarizarse con vocabulario básico mientras juega.


Por otro lado, la interacción principal se basa en gestos naturales, como:
- Tocar las cartas para girarlas y así encontrar las parejas.
- Arrastrar y soltar al animal en la opcion correcta para responder a las preguntas.


Además, se emplean sonidos y animaciones como refuerzo visual y auditivo:
- Sonidos de acierto y error.
- Animaciones de sacudida cuando se falla una pareja.
- Animaciones de victoria al completar un nivel.

Estos elementos ayudan al niño a comprender el resultado de sus acciones de forma clara y natural, sin necesidad de textos complejos.

La combinación de interacción táctil, audio y preguntas sencillas permite que el juego 
sea fácil de entender sin necesidad de instrucciones complejas mientras el niño se divierte.

---

## Accesibilidad
El proyecto incluye un ejemplo de accesibilidad aplicado únicamente en el apartado de las
preguntas, para mostrar cómo se podría adaptar la aplicación a distintos usuarios.

En el apartado de preguntas se pueden modificar:
- El tamaño del texto.
- El modo de alto contraste.

Estos ajustes afectan solo a esa parte concreta de la aplicación y sirven como
demostración de cómo se podría aplicar la accesibilidad al resto del proyecto.

---

## Funcionamiento del juego
1. El jugador comienza una partida desde la pantalla principal pulsando al botón de jugar.
2. Se muestran las cartas boca abajo.
3. Al tocar una carta:
   - La carta se gira.
   - Se escucha el nombre del animal en inglés.
4. Cuando se encuentra una pareja:
   - Aparece una pregunta sobre ese animal.
   - El jugador responde arrastrando al animal hacia la opción que crea correcta.
   - Se suma un punto cuando encuentra la pareja y acierta la pregunta.
5. Al completar todas las parejas:
   - Se pasa al siguiente nivel.
6. El juego finaliza al completar todos los niveles, en este caso, hasta el nivel 7.

---

## Instrucciones para ejecutar la aplicación
1. Abrir el proyecto en Visual Studio.
2. Seleccionar la plataforma Windows (o Android si se desea).
3. Ejecutar la aplicación.
4. En la pantalla principal, pulsar el botón Jugar.
5. Emparejar las cartas y responder a las preguntas que aparecen.
6. Ajustar el tamaño de texto o el contraste desde la pantalla de ajustes si se desea.

---

## Posibles ampliaciones
Este proyecto es un modelo inicial que podría ampliarse fácilmente:
- Añadiendo más animales.
- Añadiendo más tipos de preguntas.
- Incluyendo más idiomas.
- Aplicando accesibilidad a toda la aplicación.
- Guardando el progreso del jugador.

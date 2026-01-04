using AnimalMemory.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalMemory.MVVM.Servicio
{
    public class AnimalServicio
    {

        public static List<Animal> GetAnimales()
        {

            return new List<Animal>
            {
                new Animal {
                    Nombre = "León",
                    NombreIngles = "Lion",
                    Imagen = "leon.png",
                    Habitat = "Tierra",
                    Dieta = "Carne",
                    Tamaño = "Grande",
                    Horario = "Día"
                },
                new Animal {
                    Nombre = "Delfín",
                    NombreIngles = "Dolphin",
                    Imagen = "delfin.png",
                    Habitat = "Agua",
                    Dieta = "Carne",
                    Tamaño = "Grande",
                    Horario = "Día"
                },
                new Animal {
                    Nombre = "Búho",
                    NombreIngles = "Owl",
                    Imagen = "buho.png",
                    Habitat = "Aire",
                    Dieta = "Carne",
                    Tamaño = "Pequeño",
                    Horario = "Noche"
                },
                new Animal {
                    Nombre = "Elefante",
                    NombreIngles = "Elephant",
                    Imagen = "elefante.png",
                    Habitat = "Tierra",
                    Dieta = "Vegetales",
                    Tamaño = "Grande",
                    Horario = "Día"
                },
                new Animal {
                    Nombre = "Murciélago",
                    NombreIngles = "Bat",
                    Imagen = "murcielago.png",
                    Habitat = "Aire",
                    Dieta = "Vegetales",
                    Tamaño = "Pequeño",
                    Horario = "Noche"
                },
                new Animal {
                    Nombre = "Tiburón",
                    NombreIngles = "Shark",
                    Imagen = "tiburon.png",
                    Habitat = "Agua",
                    Dieta = "Carne",
                    Tamaño = "Grande",
                    Horario = "Día"
                },
                new Animal {
                    Nombre = "Mono",
                    NombreIngles = "Monkey",
                    Imagen = "mono.png",
                    Habitat = "Tierra",
                    Dieta = "Vegetales",
                    Tamaño = "Pequeño",
                    Horario = "Día"
                },
                new Animal {
                    Nombre = "Águila",
                    NombreIngles = "Eagle",
                    Imagen = "aguila.png",
                    Habitat = "Aire",
                    Dieta = "Carne",
                    Tamaño = "Grande",
                    Horario = "Día"
                },
                new Animal {
                    Nombre = "Tortuga",
                    NombreIngles = "Turtle",
                    Imagen = "tortuga.png",
                    Habitat = "Agua",
                    Dieta = "Vegetales",
                    Tamaño = "Pequeño",
                    Horario = "Día"
                },
                new Animal {
                    Nombre = "Gato",
                    NombreIngles = "Cat",
                    Imagen = "gato.png",
                    Habitat = "Tierra",
                    Dieta = "Carne",
                    Tamaño = "Pequeño",
                    Horario = "Día"
                }
            };

        }
    }
}

using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalMemory.MVVM.Model
{
    [AddINotifyPropertyChangedInterface]
    public class Animal
    {
        public string Nombre { get; set; }
        public string NombreIngles { get; set; }
        public string Imagen { get; set; }      
        public string Habitat { get; set; }
        public string Dieta { get; set; }
        public string Tamaño { get; set; }
        public string Horario { get; set; }


        // Variables de control del juego
        public bool EstaGirada { get; set; }
        public bool EsPareja { get; set; }

        // Si estaGirada es true muestra la imagen, si no, el reverso
        public string ImagenActual => EstaGirada ? Imagen : "reverso_carta.png";



    }
}

using AnimalMemory.MVVM.Model;
using AnimalMemory.MVVM.Servicio;
using Plugin.Maui.Audio;
using Microsoft.Maui.Media;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;


namespace AnimalMemory.MVVM.ViewModel
{

    [AddINotifyPropertyChangedInterface]
    public partial class JuegoViewModel
    {

        // Lista de cartas que se mostrará en la pantalla
        public ObservableCollection<Animal> Baraja { get; set; }

        // La puntuación del nene
        public int Puntuacion { get; set; }
        public string NivelTexto { get; set; } = null!;

        private Animal primeraCarta;
        private Animal segundaCarta;

        private int parejasEncontradas;
        private int nivelActual = 1;
        private Random azar = new Random();
        private const int NIVEL_MAXIMO = 5;

        // Comprobar si hay una partida en curso
        public bool PartidaEnCurso => nivelActual > 1 || parejasEncontradas > 0;


        //Número de columnas dinámico según el nivel
        public int Columnas { get; set; } = 2;


        // Comando para girar
        public ICommand GirarTarjetaCommand { get; set; }

        //Comando para el drop exitoso
        public ICommand AlSoltarAnimalCommand { get; set; }

        // Gestor de audio
        private readonly IAudioManager audioManager;


        // Propiedades para la interfaz natural
        public bool MostrandoPregunta { get; set; }
        public string TextoPregunta { get; set; } = "";
        public string TextoOpcionIzquierda { get; set; } = "";
        public string TextoOpcionDerecha { get; set; } = "";
        public string TextoRespuestaCorrecta { get; set; } = "";


        public string ImagenOpcionIzquierda { get; set; } = "";
        public string ImagenOpcionDerecha { get; set; } = "";


        // Para no repetir la última pregunta
        private int ultimaPregunta = -1;

        // Flag para que cuando se esté verificando una pareja no nos deje seleccionar una tercera carta
        private bool verificandoPareja = false;

        // Mensajes de acierto o error
        public string MensajePregunta { get; set; } = "";
        public bool MostrandoMensajePregunta { get; set; } = false;


        // Propiedades para accesibilidad:
        // Tamaño base del texto
        private double tamanoTexto = 100;

        public double TamanoTexto
        {
            get => tamanoTexto;
            set
            {
                tamanoTexto = value;
                PropertyChanged?.Invoke(this, new(nameof(EscalaTexto)));
                PropertyChanged?.Invoke(this, new(nameof(TamanoTextoPregunta)));
                PropertyChanged?.Invoke(this, new(nameof(TamanoMensajePregunta)));
                PropertyChanged?.Invoke(this, new(nameof(TamanoTextoOpciones)));
            }
        }

        public double EscalaTexto => TamanoTexto / 100.0;
        public double TamanoTextoPregunta => 30 * EscalaTexto;
        public double TamanoMensajePregunta => 25 * EscalaTexto;
        public double TamanoTextoOpciones => 18 * EscalaTexto;
        public double TamanoTextoInstruccion => 17 * EscalaTexto;


        // Tamaño del texto en la vista previa
        public double TamanoTextoPreview { get; set; } = 100;
        public double TamanoPreviewPregunta => 30 * (TamanoTextoPreview / 100.0);
        public double TamanoPreviewMensaje => 25 * (TamanoTextoPreview / 100.0);
        public double TamanoPreviewInstruccion => 25 * (TamanoTextoInstruccion / 100.0);

        // Colores derivados del modo contraste
        public bool ModoAltoContraste { get; set; } = false;
        public Color ColorFondoPregunta =>
            ModoAltoContraste ? Color.FromArgb("#2E1A47") : Color.FromArgb("#DDEAE6FF");

        public Color ColorTextoPregunta =>
            ModoAltoContraste ? Colors.White : Color.FromArgb("#9933CC"); 

        public Color ColorTextoMensaje =>
            ModoAltoContraste ? Color.FromArgb("#FFE6FF")  : Color.FromArgb("#4B0082");

        // Constantes para restablecer configuración
        public const double TAMANO_TEXTO_POR_DEFECTO = 100;
        public const bool ALTO_CONTRASTE_POR_DEFECTO = false;

        public Animal AnimalActualPregunta { get; set; } = null!;


        public JuegoViewModel(IAudioManager audioManager)
        {
            this.audioManager = audioManager;
            Baraja = new ObservableCollection<Animal>();

            GirarTarjetaCommand = new Command<Animal>(async (a) => await GirarTarjeta(a));
            AlSoltarAnimalCommand = new Command<string>(async (ladoSeleccionado) => await ValidarRespuesta(ladoSeleccionado));

            CargarCartas();
        }



        public async Task IniciarJuego()
        {
            if (PartidaEnCurso)
            {
                var window = Application.Current?.Windows.FirstOrDefault();
                var page = window?.Page;

                if (page != null)
                {
                    bool continuar = await page.DisplayAlertAsync(
                        $"Tienes una partida en curso. (Nivel: {nivelActual})",
                        "¿Quieres continuar donde lo dejaste o empezar de nuevo?",
                        "Continuar",
                        "Empezar de nuevo"
                    );

                    if (!continuar)
                    {
                        ReiniciarJuego();
                    }
                }
            }
            else
            {
                ReiniciarJuego();
            }
        }

        private void ReiniciarJuego()
        {
            nivelActual = 1;
            Puntuacion = 0;
            parejasEncontradas = 0;

            primeraCarta = null;
            segundaCarta = null;
            MostrandoPregunta = false;

            CargarCartas();
        }


        private void CargarCartas()
        {
           
            // Obtenemos los animales utilizando el servicio
            var todosLosAnimales = AnimalServicio.GetAnimales();

            // Calcular cuántas parejas necesitamos, cogemos solo los necesarios según el nivel (ej: nivel 1 = 3 parejas)
            int numParejas = Math.Min(nivelActual + 2, 7);

            // Para las columnnas, calculamos el total de cartas
            int totalCartas = numParejas * 2;

            // Se selecciona aleatoriamente los animales
            var seleccionados = todosLosAnimales.OrderBy(x => azar.Next()).Take(numParejas).ToList();

            // Duplicamos para crear parejas
            var cartasTemporales = new List<Animal>();
            foreach (var a in seleccionados)
            {
                // Usamos el clonar animal para pasar toda la información sin usar la misma referencia
                cartasTemporales.Add(ClonarAnimal(a));
                cartasTemporales.Add(ClonarAnimal(a));
            }


            // Mezclamos aleatoriamente
            var listaMezclada = cartasTemporales.OrderBy(x => azar.Next()).ToList();

            foreach (var carta in listaMezclada)
                carta.EstaGirada = false;


            // Calculamos el número de columnas según el total de cartas
            Columnas = totalCartas switch
            {
                <= 6  => 3,  
                <= 8  => 4,  
                <= 10 => 5,  
                _ => 4
            };


            // Reemplazamos la colección
            Baraja = new ObservableCollection<Animal>(listaMezclada);

                NivelTexto = $"Nivel {nivelActual}";
                parejasEncontradas = 0;
                MostrandoPregunta = false;
                primeraCarta = null;
                segundaCarta = null;

        }

        // Uso un método auxiliar para no usar la misma instancia de memoria en las parejas
        private Animal ClonarAnimal(Animal a) => new Animal
        {
            Nombre = a.Nombre,
            NombreIngles = a.NombreIngles,
            Imagen = a.Imagen,
            Habitat = a.Habitat,
            Dieta = a.Dieta,
            Tamaño = a.Tamaño,
            Horario = a.Horario
        };

           
            

        private void PrepararPreguntaAleatoria(Animal animal)
        {
            AnimalActualPregunta = animal;
            int tipo;

            do
            {
                // Elegimos un número al azar entre 0 y 3 para decidir qué preguntar
                tipo = azar.Next(0, 4);

            } while (tipo == ultimaPregunta);

            ultimaPregunta = tipo;


            switch (tipo)
            {
                case 0: 
                    TextoPregunta = $"¿Por dónde se desplaza el {animal.Nombre}?";
                    switch (animal.Habitat)
                    {
                        case "Tierra":
                            TextoOpcionIzquierda = "Tierra";
                            TextoOpcionDerecha = "Aire";
                            TextoRespuestaCorrecta = "Tierra";
                            ImagenOpcionIzquierda = "icono_tierra.png";
                            ImagenOpcionDerecha = "icono_aire.png";
                            break;
                        case "Agua":
                            TextoOpcionIzquierda = "Tierra";
                            TextoOpcionDerecha = "Agua";
                            TextoRespuestaCorrecta = "Agua";
                            ImagenOpcionIzquierda = "icono_tierra.png";
                            ImagenOpcionDerecha = "icono_agua.png";
                            break;
                        case "Aire":
                            TextoOpcionIzquierda = "Aire";
                            TextoOpcionDerecha = "Tierra";
                            TextoRespuestaCorrecta = "Aire";
                            ImagenOpcionIzquierda = "icono_aire.png";
                            ImagenOpcionDerecha = "icono_tierra.png";
                            break;
                    }
                    break;

                case 1: 
                    TextoPregunta = $"¿Qué come el {animal.Nombre}?";
                    TextoOpcionIzquierda = "Carne";
                    TextoOpcionDerecha = "Vegetales";
                    TextoRespuestaCorrecta = (animal.Dieta == "Carne") ? "Carne" : "Vegetales";
                    ImagenOpcionIzquierda = "icono_carne.png";
                    ImagenOpcionDerecha = "icono_vegetal.png";
                    break;

                case 2: 
                    TextoPregunta = $"¿El {animal.Nombre} es grande o pequeño?";
                    TextoOpcionIzquierda = "Grande";
                    TextoOpcionDerecha = "Pequeño";
                    TextoRespuestaCorrecta = (animal.Tamaño == "Grande") ? "Grande" : "Pequeño";
                    ImagenOpcionIzquierda = "icono_grande.png";
                    ImagenOpcionDerecha = "icono_pequeno.png";
                    break;

                case 3: 
                    TextoPregunta = $"¿Cuándo está despierto el {animal.Nombre}?";
                    TextoOpcionIzquierda = "Día";
                    TextoOpcionDerecha = "Noche";
                    TextoRespuestaCorrecta = (animal.Horario == "Día") ? "Día" : "Noche";
                    ImagenOpcionIzquierda = "icono_sol.png";
                    ImagenOpcionDerecha = "icono_luna.png";
                    break;
            }

        }


        private async Task GirarTarjeta(Animal animalSeleccionado)
        {
            // Si ya está girada o ya se encontró la pareja, no hacemos nada
            if (animalSeleccionado == null || animalSeleccionado.EstaGirada || verificandoPareja)
                return;

            // Bloqueamos para que no se pueda tocar otra carta mientras se gestiona una
            verificandoPareja = true;

            animalSeleccionado.EstaGirada = true;

            //Reproducir nombre en Inglés
            await DecirNombreEnIngles(animalSeleccionado.NombreIngles);

            if (primeraCarta == null)
            {
                primeraCarta = animalSeleccionado;

                // Desbloqueamos para elegir la segunda carta
                verificandoPareja = false;
            }
            else
            {
                segundaCarta = animalSeleccionado;

                await VerificarPareja();

                // Una vez verificado, desbloqueamos
                verificandoPareja = false;
            }
        }


        private async Task VerificarPareja()
        {
            // Bloqueamos para no seleccionar otras cartas
            verificandoPareja = true;

            if (primeraCarta.Nombre == segundaCarta.Nombre)
            {
                // Reproducir sonido de acierto
                _ = ReproducirSonido("pareja_ok.mp3");

                // Marcar las cartas como encontradas
                primeraCarta.EsPareja = true;
                segundaCarta.EsPareja = true;
                parejasEncontradas++;

                // Preparar la pregunta de interfaz natural
                AnimalActualPregunta = primeraCarta;
                PrepararPreguntaAleatoria(AnimalActualPregunta);

                // Mostrar panel de pregunta
                MostrandoPregunta = true;
            }
            else
            {
                // Reproducir sonido de error
                _ = ReproducirSonido("error.mp3");

                // Animación de sacudida
                WeakReferenceMessenger.Default.Send("ErrorPareja");

                // Esperar un momento antes de tapar las cartas
                await Task.Delay(1000);
                primeraCarta.EstaGirada = false;
                segundaCarta.EstaGirada = false;

                // Limpiar referencias
                primeraCarta = null;
                segundaCarta = null;
                // Desbloqueamos en el final
                verificandoPareja = false;
            }
        }





        private async Task DecirNombreEnIngles(string nombreIngles)
        {
            if (string.IsNullOrEmpty(nombreIngles)) return;

            try
            {
                // Obtener todos los idiomas disponibles en el dispositivo
                var locales = await TextToSpeech.Default.GetLocalesAsync();

                // Buscamos una voz en inglés
                var ingles = locales.FirstOrDefault(l => l.Language.StartsWith("en", StringComparison.OrdinalIgnoreCase));

                await TextToSpeech.Default.SpeakAsync(nombreIngles, new SpeechOptions
                {
                    Locale = ingles,
                    Pitch = 1.2f // Un tono ligeramente alto
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error TTS: {ex.Message}");
            }
        }


        private async Task ValidarRespuesta(string ladoSeleccionado)
        {
            if (ladoSeleccionado == TextoRespuestaCorrecta)
            {
                // Correcto
                await ReproducirSonido("pregunta_ok.mp3");
                Puntuacion += 1;

                // Mostrar mensaje de acierto
                await MostrarMensajePregunta("¡Correcto!", 3000);

            }
            else
            {
                // Incorrecto
                await ReproducirSonido("error.mp3");
                WeakReferenceMessenger.Default.Send("ErrorPareja");

                // Mostrar mensaje de fallo con la respuesta correcta
                await MostrarMensajePregunta($"La respuesta correcta era: {TextoRespuestaCorrecta}", 3500);


            }

            // Pausa extra para poder leer bien el mensaje
            await Task.Delay(500);

            // Limpiamos las cartas actuales
            MostrandoPregunta = false;
            primeraCarta = null;
            segundaCarta = null;

            // Verificamos si el nivel ha terminado
            if (parejasEncontradas == Baraja.Count / 2)
            {

                // Si es el último nivel, mostramos mensaje de victoria final.
                if (nivelActual >= NIVEL_MAXIMO)
                {
                    await ReproducirSonido("victoria_total.mp3");

                    // Obtenemos la ventana actual
                    var window = Application.Current?.Windows[0];

                    if (window?.Page != null)
                    {
                        // Mostramos el alert usando DisplayAlertAsync
                        await window.Page.DisplayAlertAsync(
                            "¡Felicidades!",
                            "Has completado todos los niveles del juego",
                            "Volver al inicio"
                        );
                    }

                    nivelActual = 1;
                    Puntuacion = 0;
                    parejasEncontradas = 0;
                    CargarCartas();
                    return;
                }

                // Si no es el último nivel, avanzamos al siguiente
                // Reproducir sonido de victoria de nivel
                await ReproducirSonido("victoria_ronda.mp3");

                // Avisamos a la vista para que haga la animación de victoria
                WeakReferenceMessenger.Default.Send("VictoriaNivel");

                // Esperamos a que termine la validación
                await Task.Delay(1200);

                // Obtenemos la ventana actual
                var window2 = Application.Current?.Windows.FirstOrDefault();
                var page = window2?.Page;

                if (page != null)
                {
                    await page.DisplayAlertAsync(
                        "¡Increíble!",
                        "Has encontrado todos los animales",
                        "Siguiente Nivel"
                    );
                }


                // Pausa de seguridad para que el Alert desaparezca de la memoria de Windows
                await Task.Delay(500);

                // Usamos MainThread para resetear el juego de forma segura y utilizando el hilo principal
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    nivelActual++;
                    CargarCartas();
                });
            }
        }

        private async Task MostrarMensajePregunta(string mensaje, int duracionMs = 1500)
        {
            MensajePregunta = mensaje;
            MostrandoMensajePregunta = true;

            await Task.Delay(duracionMs);

            MostrandoMensajePregunta = false;
            MensajePregunta = "";
        }


        private async Task ReproducirSonido(string nombreArchivo)
        {
            try
            {
                var jugador = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(nombreArchivo));
                jugador.Play();
            }
            catch { 
                //Si falta el archivo, el juego no se rompe 
            }
        }


        public void RestablecerAccesibilidad()
        {
            TamanoTextoPreview = TAMANO_TEXTO_POR_DEFECTO;
            ModoAltoContraste = ALTO_CONTRASTE_POR_DEFECTO;
        }




    }
}


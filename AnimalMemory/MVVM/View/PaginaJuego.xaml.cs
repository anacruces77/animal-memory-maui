using AnimalMemory.MVVM.ViewModel;
using CommunityToolkit.Mvvm.Messaging;

namespace AnimalMemory.MVVM.View;

public partial class PaginaJuego : ContentPage
{
	public PaginaJuego(JuegoViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

        // Escuchamos el mensaje de error
        WeakReferenceMessenger.Default.Register<string>(this, async (r, m) =>
        {
            if (m == "ErrorPareja")
            {
                await EfectoSacudida();
            }
            else if (m == "VictoriaNivel")
            {
                await AnimacionVictoria();
            }
        });


    }

    private async void btnSalir_Clicked(object sender, EventArgs e)
    {
        
        await Navigation.PopAsync();
    }

    private async void AbrirAjustes_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is JuegoViewModel vm)
        {
            await Navigation.PushAsync(new PaginaAjustes(vm));
        }
    }


    private void OnDropIzquierda(object sender, DropEventArgs e)
    {
        if (BindingContext is JuegoViewModel vm)
            vm.AlSoltarAnimalCommand.Execute(vm.TextoOpcionIzquierda);
    }

    private void OnDropDerecha(object sender, DropEventArgs e)
    {
        if (BindingContext is JuegoViewModel vm)
            vm.AlSoltarAnimalCommand.Execute(vm.TextoOpcionDerecha);
    }




    private async Task EfectoSacudida()
    {
        // Una animación rápida de izquierda a derecha
        uint duration = 50; // milisegundos
        await TableroCartas.TranslateToAsync(-15, 0, duration);
        await TableroCartas.TranslateToAsync(15, 0, duration);
        await TableroCartas.TranslateToAsync(-10, 0, duration);
        await TableroCartas.TranslateToAsync(10, 0, duration);
        await TableroCartas.TranslateToAsync(-5, 0, duration);
        await TableroCartas.TranslateToAsync(5, 0, duration);
        TableroCartas.TranslationX = 0; // Lo dejamos centrado
    }


    private async Task AnimacionVictoria()
    {
        await TableroCartas.ScaleToAsync(1.1, 200); //Hace la carta un poco más grande, y la devuelve al tamaño original
        await TableroCartas.TranslateToAsync(0, -50, 200, Easing.CubicOut); // Sube
        await TableroCartas.TranslateToAsync(0, 0, 200, Easing.BounceOut);  // Cae con rebote
        await TableroCartas.ScaleToAsync(1.0, 200);
    }




    // Importante: Limpiar el mensaje cuando cerramos la página
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Esto limpia todas las suscripciones de golpe
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    // Personalizar la barra de navegación
    protected override void OnAppearing()
    {

        // Se ejecuta cada vez que la página se muestra
        base.OnAppearing();

        var window = Application.Current?.Windows[0];
        if (window?.Page is NavigationPage navPage)
        {
            navPage.BarTextColor = Colors.Purple;
            navPage.BarBackgroundColor = Colors.White;
        }

    }



}
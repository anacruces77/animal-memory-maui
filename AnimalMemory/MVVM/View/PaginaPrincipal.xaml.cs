using AnimalMemory.MVVM.ViewModel;
using Plugin.Maui.Audio;

namespace AnimalMemory.MVVM.View;

public partial class PaginaPrincipal : ContentPage
  
{

    private JuegoViewModel vm;

    public PaginaPrincipal()
	{
		InitializeComponent();
        vm = new JuegoViewModel(AudioManager.Current);
        BindingContext = vm;
}

    private async void btnJugar_Clicked(object sender, EventArgs e)
    {
        await vm.IniciarJuego();
        // Navegamos a la página del juego usando NavigationPage
        await Navigation.PushAsync(new PaginaJuego(vm));
    }

    private async void btnAjustes_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PaginaAjustes(vm));
    }

    private async void btnComoJugar_Clicked(object sender, EventArgs e)
    {
        await DisplayAlertAsync(
        "¿Cómo jugar?",
        "1- Toca dos cartas para buscar animales iguales.\n" +
        "2- Si aciertas, aparecerá una pregunta sobre el animal encontrado.\n" +
        "3- Arrastra el animal a la respuesta correcta.\n" +
        "4- Gana puntos y pasa de nivel encontrando todas las parejas\n\n" +
        "¡Diviértete aprendiendo!",
        "¡Vamos!"
    );
    }



}
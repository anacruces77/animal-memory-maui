using AnimalMemory.MVVM.ViewModel;

namespace AnimalMemory.MVVM.View;

public partial class PaginaAjustes : ContentPage
{
    private JuegoViewModel vm;

    public PaginaAjustes(JuegoViewModel viewModel)
    {
        InitializeComponent();
        vm = viewModel;
        BindingContext = vm; 
    }

    private async void Guardar_Clicked(object sender, EventArgs e)
    {
        // Aplicamos los cambios reales
        vm.TamanoTexto = vm.TamanoTextoPreview;

        await Navigation.PopAsync();
    }

    private void Restablecer_Clicked(object sender, EventArgs e)
    {
        vm.RestablecerAccesibilidad();
    }



}
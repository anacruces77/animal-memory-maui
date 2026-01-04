using AnimalMemory.MVVM.View;
using Microsoft.Extensions.DependencyInjection;

namespace AnimalMemory
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new NavigationPage(new PaginaPrincipal()));

        }
    }
}
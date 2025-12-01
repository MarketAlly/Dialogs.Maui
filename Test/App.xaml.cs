using Test.Pages;

namespace Test
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();
		}

		protected override Window CreateWindow(IActivationState? activationState)
		{
			return new Window(new NavigationPage(new DialogDemoPage()));
		}
	}
}
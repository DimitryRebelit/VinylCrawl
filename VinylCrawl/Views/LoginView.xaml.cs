using VinylCrawl.ViewModels;

namespace VinylCrawl.Views;

public partial class LoginView : ContentPage
{
	private readonly LoginViewModel _viewModel;

	public LoginView(LoginViewModel viewModel)
	{
		_viewModel = viewModel;
		BindingContext = _viewModel;
		InitializeComponent();
	}

    private async void LinkDiscogsButton_Clicked(object sender, EventArgs e)
    {
		await _viewModel.LoginButtonPressed(sender, e);
    }
}
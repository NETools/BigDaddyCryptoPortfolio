using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Contracts.ViewModels.Auth;
using BigDaddyCryptoPortfolio.Models;
using BigDaddyCryptoPortfolio.Ui.Auth;

namespace BigDaddyCryptoPortfolio.Views;

public partial class AuthenticationView : ContentPage
{
    private AuthViews _currentView = AuthViews.Login;
    private Dictionary<AuthViews, View> _views = [];
    public AuthenticationView(IRegisterViewModel registerViewModel, ILoginViewModel loginViewModel, IConfirmViewModel confirmViewModel)
	{
		InitializeComponent();

        _views.Add(AuthViews.Login, new LoginView(loginViewModel));
        _views.Add(AuthViews.Register, new RegisterView(registerViewModel));
        _views.Add(AuthViews.Confirmation, new ConfirmView(confirmViewModel));

        foreach (IViewChange<AuthViews> view in _views.Values)
        {
			view.ViewChange += OnViewChange;
        }

        Content = _views[_currentView];
	}

	private async void OnViewChange(AuthViews view)
	{
        var currentView = _views[_currentView];
        await currentView.FadeTo(0);
        Content = _views[view];
        currentView.Opacity = 1;
        _currentView = view;
	}
}
namespace BigDaddyCryptoPortfolio.Views.SecondOrder;

public partial class AboutView : ContentPage
{
	public string AboutText { get; } = @"Die App wurde von Enes Hergül für BigDaddyCrypto entwickelt.
Bei Fragen, Anregungen und Anfragen wenden Sie sich bitte an den Entwickler; entweder über das Kontaktformular, oder über E-Mail unter enes.hergul215@gmail.com.

Lieben Dank!";
	public AboutView()
	{
		InitializeComponent();

		BindingContext = this;
	}
}
using VinylCrawl.Discogs.Agents;
using VinylCrawl.Discogs.Agents.Interfaces;

namespace VinylCrawl;

public partial class App : Application
{
	public App()
	{
		DependencyService.Register<IAuthAgent, AuthAgent>();
		DependencyService.Register<ICollectionAgent, CollectionAgent>();

		InitializeComponent();

		MainPage = new AppShell();
	}
}

namespace PhotoDiary;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		// Entry page outside the Shell structure: global route.
		Routing.RegisterRoute("entry", typeof(EntryPage));
	}
}

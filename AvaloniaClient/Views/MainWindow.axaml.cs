using System;
using Avalonia.Controls;

namespace AvaloniaClient.Views;

public partial class MainWindow : Window
{
	private readonly IServiceProvider _services;

    public MainWindow(IServiceProvider services)
    {
	    _services = services;
	    InitializeComponent();

        // Pop up a new instance of login window
        UnlockIdFileWindow unlockWindow = new(_services);

        unlockWindow.Show();
    }
}
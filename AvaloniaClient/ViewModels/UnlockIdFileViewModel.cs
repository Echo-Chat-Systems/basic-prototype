using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaClient.ViewModels;

public partial class UnlockIdFileViewModel : ViewModelBase
{
	[ObservableProperty] public partial string Password { get; set; } = "Encryption Password";
}
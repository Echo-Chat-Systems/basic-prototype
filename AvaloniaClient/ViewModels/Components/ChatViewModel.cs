using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaClient.ViewModels.Components;

public class ChatViewModel : ObservableObject
{

}

public partial class ChatMessageModel : ObservableObject
{
	[ObservableProperty] public partial string Text { get; set; } = "Invalid message";
}
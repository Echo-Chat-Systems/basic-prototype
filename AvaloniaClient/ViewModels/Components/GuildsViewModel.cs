using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EchoLib.Models.Data.Guild;

namespace AvaloniaClient.ViewModels.Components;

[TransientModel]
public partial class GuildsViewModel : ViewModelBase
{
	[ObservableProperty] public partial ObservableCollection<JGuild> Guilds { get; set; }

	[RelayCommand]
	public async Task NewGuild()
	{

	}
}
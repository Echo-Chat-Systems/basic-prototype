using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace TuiClient.Components;

public class StackView : View
{
	public enum Direction
	{
		TopToBottom,
		BottomToTop,
		LeftToRight,
		RightToLeft
	}

	private Direction _direction;

	public StackView(Direction direction)
	{
		_direction = direction;
		CanFocus = true;

		Width = Dim.Fill();
	}

	public void AddControl(params View[] views)
	{
		foreach (View view in views)
		{
			AddControl(view);
		}

	}

	public void AddControl(View view)
	{
		switch (_direction)
		{
			case Direction.TopToBottom:
				view.X = 0;
				view.Y = SubViews.Count == 0 ? 0 : Pos.Bottom(SubViews.ElementAt(SubViews.Count - 1)) + 1;
				view.Width = Dim.Fill();

				view.Height = view is Button ? 2 : 1;

				if (view is TextField or Button)
				{
					view.TabStop = TabBehavior.TabStop;
				}

				break;
			case Direction.BottomToTop:
				throw new NotImplementedException();
				break;
			case Direction.LeftToRight:
				throw new NotImplementedException();
			case Direction.RightToLeft:
				throw new NotImplementedException();
				break;
			default:
				throw new ArgumentOutOfRangeException();
				break;
		}
		Add(view);
	}
}
using Terminal.Gui.ViewBase;

namespace TuiClient.Views;

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

		if (_direction is Direction.LeftToRight or Direction.RightToLeft)
		{
			Height = Dim.Fill();
		}
		else
		{
			Width = Dim.Fill();
		}
	}

	public void AddControl(View view)
	{
		switch (_direction)
		{
			case Direction.TopToBottom:
				view.X = 0;
				view.Y = SubViews.Count == 0 ? 0 : Pos.Bottom(SubViews.ElementAt(SubViews.Count - 1));
				view.Width = Dim.Fill();
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
using System.ComponentModel;

namespace Game2048
{
	public class Tile : INotifyPropertyChanged
	{
		private int value;
		public int Value
		{
			get => value;
			set
			{
				if (this.value != value)
				{
					this.value = value;
					OnPropertyChanged(nameof(Value));
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged(string propertyName) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}

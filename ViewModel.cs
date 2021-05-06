using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Game2048
{
	public class ViewModel : INotifyPropertyChanged
	{
		private const int length = 4, nStartTiles = 2, defaultValue = 0;
		private Random random = new Random();
		private int score, highScore;

		public Tile[][] Tiles { get; private set; }
		public Command Left { get; private set; }
		public Command Up { get; private set; }
		public Command Right { get; private set; }
		public Command Down { get; private set; }
		public Command Restart { get; private set; }
		public int Score
		{
			get => score;
			set
			{
				if (score != value)
				{
					score = value;
					if (score > HighScore)
						HighScore = score;
					OnPropertyChanged(nameof(Score));
				}
			}
		}
		public int HighScore
		{
			get => highScore;
			set
			{
				if (highScore != value)
				{
					highScore = value;
					OnPropertyChanged(nameof(HighScore));
				}
			}
		}

		public ViewModel()
		{
			Tiles = new Tile[length][];
			for (int i = 0; i < length; i++)
			{
				Tiles[i] = new Tile[length];
				for (int j = 0; j < length; j++)
					Tiles[i][j] = new Tile();
			}

			Left = new Command(o => Move(true, false));
			Up = new Command(o => Move(false, false));
			Right = new Command(o => Move(true, true));
			Down = new Command(o => Move(false, true));
			Restart = new Command(o =>
			{
				Score = 0;
				for (int i = 0; i < length; i++)
					for (int j = 0; j < length; j++)
						Tiles[i][j].Value = defaultValue;
				TryAddTiles(nStartTiles);
			});
			TryAddTiles(nStartTiles);
		}
		private int CreateValue() => random.NextDouble() > 0.9 ? 4 : 2;
		/// <summary>
		/// Creates values for random tiles where they are not shown.
		/// </summary>
		/// <param name="count">number of tiles to add</param>
		/// <returns>true if there was free space and all tiles were addded, otherwise false</returns>
		private bool TryAddTiles(int count)
		{
			List<int[]> indeces = new List<int[]>();
			for (int i = 0; i < length; ++i)
				for (int j = 0; j < length; ++j)
					if (Tiles[i][j].Value == defaultValue)
						indeces.Add(new int[2] { i, j });
			if (indeces.Count < count)
				return false;

			for (int i = 0; i < count; ++i)
			{
				int[] index = indeces[random.Next(indeces.Count)];
				Tiles[index[0]][index[1]].Value = CreateValue();
				indeces.Remove(index);
			}
			return true;
		}
		/// <summary>
		/// Moves all tiles in the specified direction and adds a new tile if there was a movement.
		/// </summary>
		/// <param name="hor">true - horizontal, false - vertical</param>
		/// <param name="inc">true - right or down, false - left or up</param>
		private void Move(bool hor, bool inc)
		{
			bool lastTileSticked, moved = false;
			for (int i = 0; i < length; ++i)
			{
				lastTileSticked = false;
				for (int j = inc ? length - 1 : 0; j != (inc ? -1 : length); j += inc ? -1 : 1)
					if (Tiles[hor ? i : j][hor ? j : i].Value != defaultValue)
					{
						for (int k = j; k != (inc ? length - 1 : 0); k += inc ? 1 : -1)
						{
							Tile current = Tiles[hor ? i : k][hor ? k : i], next = Tiles[hor ? i : (k + (inc ? 1 : -1))][hor ? (k + (inc ? 1 : -1)) : i];
							if (next.Value == defaultValue)
							{
								int temp = current.Value;
								current.Value = next.Value;
								next.Value = temp;
								moved = true;
							}
							else if (next.Value == current.Value && !lastTileSticked)
							{
								next.Value *= 2;
								Score += next.Value;
								current.Value = defaultValue;
								lastTileSticked = true;
								moved = true;
								break;
							}
							else
							{
								lastTileSticked = false;
								break;
							}
						}
					}
			}
			if (moved)
			{
				TryAddTiles(1);
				if (!Pick(false, false) && !Pick(false, true) && !Pick(true, false) && !Pick(true, true))
				{
					MessageBox.Show("Game over!");
					Restart.Execute(null);
				}
			}
		}
		/// <summary>
		/// Checks is it possible to move the tiles in the specified direction but does not affect the data.
		/// </summary>
		/// <param name="hor">true - horizontal, false - vertical</param>
		/// <param name="inc">true - right or down, false - left or up</param>
		/// <returns>true if you can make a move, otherwise false</returns>
		private bool Pick(bool hor, bool inc)
		{
			for (int i = 0; i < length; ++i)
				for (int j = inc ? length - 1 : 0; j != (inc ? -1 : length); j += inc ? -1 : 1)
					if (Tiles[hor ? i : j][hor ? j : i].Value != defaultValue && j != (inc ? length - 1 : 0))
					{
						Tile current = Tiles[hor ? i : j][hor ? j : i], next = Tiles[hor ? i : (j + (inc ? 1 : -1))][hor ? (j + (inc ? 1 : -1)) : i];
						if (next.Value == defaultValue || next.Value == current.Value)
							return true;
					}
			return false;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged(string propertyName) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}

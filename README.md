# 2048
Single-player sliding tile puzzle.

## Framework
WPF using MVVM pattern.

## Gameplay
2048 is played on a plain 4×4 grid, with numbered tiles that slide when a player moves them using the four arrow keys.

Every turn, a new tile randomly appears in an empty spot on the board with a value of either 2 or 4.
Tiles slide as far as possible in the chosen direction until they are stopped by either another tile or the edge of the grid.
If two tiles of the same number collide while moving, they will merge into a tile with the total value of the two tiles that collided.
The resulting tile cannot merge with another tile again in the same move.

A scoreboard keeps track of the user's score.
The user's score starts at zero, and is increased whenever two tiles combine, by the value of the new tile
When the player has no legal moves, the game ends.

## Features
- adaptive window size
- keeps track of the score
- keeps track of the high score
- tile color is based on its value
- after each move checks if the game is not lost

## Screenshot
![image](https://user-images.githubusercontent.com/64147945/179096457-60b01125-2944-4916-b8bb-85a3d6b3af08.png)

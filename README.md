# Maze Generator

Maze generator is an C# application for generating random mazes with a treasure hidden in them.

*It is made for my mobile game (coming soon)* 

**Assumptions:**
- The entrance of the maze is located somewhere on the bottom of the maze
- The exit of the maze is located somewhere on the top of the maze
- There is only one way to get from the entrance to the exit
- The treasure is hidden on the farther tile from the main path
- Maze is a square

**Application is working up to 9x9 size.** (Treasure location has to be saved in different way for bigger mazes)

# Usage

Static class **MazeGenerator** contains two methods: **Generate** and **GenerateMany**.

- **Generate(int size)**

  Takes one argument - size of the maze.
  
  Generates one random maze of provided size.
  
- **GenerateMany(int size, int count, int overload = 1)**

  Arguments:
  - size     - size of the maze
  - count    - number of mazes to be generated
  - overload - variable indicating how many times more mazes will be generated. The most differentiating of them will be returned.
  
Mazes are returned as members of class **Maze**

# Example

Example of generated maze (after .ToString() is called):

size: 4

```
1010
2320
0222
0000
03
30
```

Each of number of first 4 (size) rows stand for tile. 

If (number == 3) then the walls are located on bottom and right side of tile

If (number == 2) then the wall is located on bottom of tile

If (number == 1) then the wall is located on the right side of tile

If (number == 0) then there are no walls on bottom and right side of tile

Next row stands for the location of the exit and entrance of the maze.
First number - index of end (at the top of maze). Second number - index of the entrance (at the bottom of maze)

Last row stands for the treasure location. Numbers indicate X and Y of the tile containing the treasure.

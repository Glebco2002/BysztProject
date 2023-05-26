using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

class DungeonGen
{
    private int width;
    private int height;
    private bool[,] maze;
    private Random random;

    public DungeonGen(int width, int height)
    {
        this.width = width;
        this.height = height;
        this.maze = new bool[width, height];
        this.random = new Random();
    }


    public void Start()
    {
        // Размер ячейки в пикселях
        int cellSize = 1; 

        DungeonGen generator = new DungeonGen(width, height);
        generator.GenerateMaze();
        generator.SaveMazeAsImage("dungeonmap.png", cellSize);
    }

    public void GenerateMaze()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = true;
            }
        }

        GenerateMazeCell(1, 1);

        // Установка входа и выхода
        maze[1, 0] = false;
        maze[width - 2, height - 1] = false;
    }

    private void GenerateMazeCell(int x, int y)
    {
        maze[x, y] = false;

        int[] directions = GetRandomDirections();

        foreach (int direction in directions)
        {
            int nextX = x;
            int nextY = y;

            if (direction == 0)
            {
                nextX -= 2;
            }
            else if (direction == 1)
            {
                nextX += 2;
            }
            else if (direction == 2)
            {
                nextY -= 2;
            }
            else if (direction == 3)
            {
                nextY += 2;
            }

            if (IsValidCell(nextX, nextY))
            {
                if (maze[nextX, nextY])
                {
                    maze[(x + nextX) / 2, (y + nextY) / 2] = false;
                    GenerateMazeCell(nextX, nextY);
                }
            }
        }
    }

    private int[] GetRandomDirections()
    {
        int[] directions = { 0, 1, 2, 3 };

        // Случайное перемешивание массива
        for (int i = directions.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            int temp = directions[i];
            directions[i] = directions[j];
            directions[j] = temp;
        }

        return directions;
    }

    private bool IsValidCell(int x, int y)
    {
        return x > 0 && x < width && y > 0 && y < height && maze[x, y];
    }

    public void SaveMazeAsImage(string filename, int cellSize)
    {
        int imageWidth = width * cellSize;
        int imageHeight = height * cellSize;

        using (Image<Rgba32> image = new Image<Rgba32>(imageWidth, imageHeight))
        {
            // Заливаем картинку чёрным цветом
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    image[x, y] = Color.Black;
                }
            }

            // Рисуем стены
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int startX = x * cellSize;
                    int startY = y * cellSize;

                    if (maze[x, y] == true) //если клетка - стена
                    {
                        for (int i = startX; i < startX + cellSize; i++)
                        {
                            for (int j = startY; j < startY + cellSize; j++)
                            {
                                image[i, j] = Color.White;
                            }
                        }
                    }
                }
            }

            image.Save(filename);
        }
    }
    public static void InvertImageColors(Image<Rgba32> image)
    {
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                Rgba32 pixel = image[x, y];
                Rgba32 invertedPixel = new Rgba32(
                    (byte)(255 - pixel.R),
                    (byte)(255 - pixel.G),
                    (byte)(255 - pixel.B),
                    pixel.A);

                image[x, y] = invertedPixel;
            }
        }
    }
}
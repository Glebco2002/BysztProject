using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace WorldGen
{
    internal class GlobalCityMapGen
    {
        public void Start()
        {
            Console.Clear();
            Console.WriteLine("Генерация карты городов...");

            // Путь к входному изображению
            string inputImagePath = "physmap.png";

            // Загрузка изображения
            using (Image<Rgba32> image = Image.Load<Rgba32>(inputImagePath))
            {
                // Генерация случайных отметок
                GenerateRandomCities(image);

                // Сохранение результирующего изображения с отметками
                string outputImagePath = "physmap_with_cities.png";
                image.Save(outputImagePath);

                Console.Clear();
                Console.WriteLine("Сгенерированная карта городов сохранена в файл " + outputImagePath);
                Console.WriteLine();
                Console.WriteLine("Нажмите любую кнопку чтобы продолжить");
                Console.ReadKey();
            }
        }

        // Генерация случайных городов
        void GenerateRandomCities(Image<Rgba32> image)
        {
            // Размеры изображения
            int width = image.Width;
            int height = image.Height;

            // Максимальное количество городов в зависимости от размера изображения
            int maxCities = width;

            // Случайный генератор
            Random random = new Random();

            // Цвета городов
            Rgba32[] cityColors = new Rgba32[]
            {
                new Rgba32(255, 0, 0),    // Красный
                new Rgba32(0, 255, 0),    // Зеленый
                new Rgba32(0, 0, 255),    // Синий
                new Rgba32(255, 255, 0),  // Желтый
                new Rgba32(255, 0, 255),  // Пурпурный
                new Rgba32(0, 255, 255)   // Бирюзовый
            };

            // Генерация городов
            for (int i = 0; i < maxCities; i++)
            {
                // Случайные координаты для города
                int cityX = random.Next(0, width);
                int cityY = random.Next(0, height);

                Rgba32 pixel = image[cityX, cityY];

                // Проверяем, что пиксель не является водным и не является заснеженной вершиной
                if (!IsWaterPixel(pixel) && !IsSnowyPeakPixel(pixel))
                {
                    // Случайный выбор цвета города
                    Rgba32 cityColor = cityColors[random.Next(0, cityColors.Length)];

                    // Рисуем город (один пиксель)
                    image[cityX, cityY] = cityColor;
                }
            }
        }


        // Проверяет, является ли пиксель водным
        bool IsWaterPixel(Rgba32 pixel)
        {
            // Здесь вы можете определить вашу логику для проверки,
            // является ли пиксель водным
            // Например, если пиксель имеет синий компонент больше определенного порога
            return pixel.B >= 128;
        }

        // Проверяет, является ли пиксель заснеженной вершиной
        bool IsSnowyPeakPixel(Rgba32 pixel)
        {
            // Здесь вы можете определить вашу логику для проверки, является ли пиксель заснеженной вершиной
            // Например, если пиксель имеет все компоненты (R, G, B) выше определенного порога
            return pixel.R >= 255 && pixel.G >= 255 && pixel.B >= 255;
        }
    }
}
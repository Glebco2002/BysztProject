using System;
using System.IO;
using SharpNoise;
using SharpNoise.Builders;
using SharpNoise.Modules;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace WorldGen
{
    internal class PhysMapGen
    {
        public void Start()
        {
            // Задаём разрешение физической карты
            Console.Clear();
            Console.WriteLine("Рекомендуется генерировать квадратные карты, во избежание ошибок генерации");
            Console.WriteLine();
            Console.WriteLine("Задайте разрешение вашей карты в высоту");
            int custHeight = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            Console.WriteLine("Задайте разрешение вашей карты в ширину");
            int custWidth = Convert.ToInt32(Console.ReadLine());

            // Генерация карты
            Console.Clear();
            Console.WriteLine("Генерация карты...");
            var noiseModule = CreateNoiseModule();
            var noiseMap = GenerateNoiseMap(noiseModule, custWidth, custHeight);

            // Раскраска карты и сохранение её в png файл
            Console.WriteLine("Сохраняем физическую карту в png");
            SaveNoiseMapToPng(noiseMap, "physmap.png");
            Console.WriteLine("Сгенерированная физическая карта сохранена в файл physmap.png");

            // Сохранение сырой карты высот в png файл
            Console.WriteLine("Сохраняем карту высот в png");
            SaveHeightMapToPng(noiseMap, "heightmap.png");
            Console.WriteLine("Сгенерированная карта высот сохранена в файл heightmap.png");

            // Отчёт о проделанной работе
            Console.Clear();
            Console.WriteLine("Сгенерированная физическая карта сохранена в файл physmap.png");
            Console.WriteLine("Сгенерированная карта высот сохранена в файл heightmap.png");
            Console.WriteLine();
            Console.WriteLine("Нажмите любую кнопку чтобы выйти...");
            Console.ReadKey();
        }

        Module CreateNoiseModule()
        {
            // Создает основной модуль шума типа Perlin
            var baseModule = new Perlin();

            // Устанавливает случайное семя на основе текущего времени
            baseModule.Seed = (int)DateTime.Now.Ticks;

            // Создает дополнительный модуль шума типа Perlin
            var noiseModule = new Perlin();

            // Устанавливает случайное семя на основе текущего времени
            noiseModule.Seed = (int)DateTime.Now.Ticks;

            // Устанавливает параметры частоты, Lacunarity (коэффициент зубцов) и OctaveCount (количество октав)
            noiseModule.Frequency = 1.0;
            noiseModule.Lacunarity = 2.0;
            noiseModule.Persistence = 0.7;
            noiseModule.OctaveCount = 32; // Количество октав

            // Возвращает дополнительный модуль шума
            return noiseModule;
        }

        // Создает экземпляр NoiseMap заданного размера
        NoiseMap GenerateNoiseMap(Module module, int width, int height)
        {
            var noiseMap = new NoiseMap();
            noiseMap.SetSize(width, height);

            // Создает NoiseMapBuilder для создания карты шума
            var noiseMapBuilder = new PlaneNoiseMapBuilder();

            // Устанавливает источниковый модуль и карту шума
            noiseMapBuilder.SourceModule = module;
            noiseMapBuilder.DestNoiseMap = noiseMap;

            // Устанавливает размеры карты и границы
            noiseMapBuilder.SetDestSize(width, height);
            noiseMapBuilder.SetBounds(0, 1, 0, 1);

            // Строит карту шума
            noiseMapBuilder.Build();

            // Возвращает сгенерированную карту шума
            return noiseMap;
        }

        // Сохраняет карту шума в файл PNG
        void SaveNoiseMapToPng(NoiseMap noiseMap, string filePath)
        {
            int width = noiseMap.Width;
            int height = noiseMap.Height;

            // Определяет уровни шума и соответствующие цвета
            const double darkestseaLevel = -0.9;
            const double darkerseaLevel = -0.6;
            const double darkseaLevel = -0.4;
            const double seaLevel = -0.2;
            const double lightseaLevel = -0.05;
            const double shallowseaLevel = -0.02;
            const double coastseaLevel = 0.0;
            const double sandLevel = 0.02;
            const double lightgrassLevel = 0.1;
            const double grassLevel = 0.2;
            const double forestLevel = 0.4;
            const double lighthillLevel = 0.50;
            const double hillLevel = 0.6;
            const double darkhillLevel = 0.75;
            const double lightermountainLevel = 0.80;
            const double lightmountainLevel = 0.85;
            const double mountainLevel = 0.9;

            // Создает изображение и устанавливает цвета пикселей в соответствии с уровнями шума
            var darkestseaColor = new Rgba32(0, 0, 60);
            var darkerseaColor = new Rgba32(0, 0, 80);
            var darkseaColor = new Rgba32(0, 0, 100);
            var seaColor = new Rgba32(0, 0, 128);
            var lightseaColor = new Rgba32(0, 0, 164);
            var shallowseaColor = new Rgba32(0, 0, 180);
            var coastseaColor = new Rgba32(0, 0, 200);
            var sandColor = new Rgba32(200, 164, 96);
            var lightgrassColor = new Rgba32(50, 160, 50);
            var grassColor = new Rgba32(34, 139, 34);
            var forestColor = new Rgba32(30, 90, 30);
            var lighthillColor = new Rgba32(200, 160, 110);
            var hillColor = new Rgba32(180, 140, 90);
            var darkhillColor = new Rgba32(140, 100, 60);
            var lightermountainColor = new Rgba32(110, 117, 117);
            var lightmountainColor = new Rgba32(120, 127, 127);
            var mountainColor = new Rgba32(139, 137, 137);
            var snowColor = new Rgba32(255, 255, 255);

            using (var image = new Image<Rgba32>(width, height))
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        double noiseValue = noiseMap.GetValue(x, y);

                        Rgba32 color;
                        if (noiseValue < darkestseaLevel)
                            color = darkestseaColor;
                        else if(noiseValue < darkerseaLevel)
                            color = darkerseaColor;
                        else if(noiseValue < darkseaLevel)
                            color = darkseaColor;
                        else if(noiseValue < seaLevel)
                            color = seaColor;
                        else if(noiseValue < lightseaLevel)
                            color = lightseaColor;
                        else if (noiseValue < shallowseaLevel)
                            color = shallowseaColor;
                        else if (noiseValue < coastseaLevel)
                            color = coastseaColor;
                        else if (noiseValue < sandLevel)
                            color = sandColor;
                        else if (noiseValue < lightgrassLevel)
                            color = lightgrassColor;
                        else if (noiseValue < grassLevel)
                            color = grassColor;
                        else if (noiseValue < forestLevel)
                            color = forestColor;
                        else if (noiseValue < lighthillLevel)
                            color = lighthillColor;
                        else if (noiseValue < hillLevel)
                            color = hillColor;
                        else if (noiseValue < darkhillLevel)
                            color = darkhillColor;
                        else if (noiseValue < lightermountainLevel)
                            color = lightermountainColor;
                        else if (noiseValue < lightmountainLevel)
                            color = lightmountainColor;
                        else if (noiseValue < mountainLevel)
                            color = mountainColor;
                        else
                            color = snowColor;

                        image[x, y] = color;
                    }
                }

                // Сохраняет изображение в файл по указанному пути
                image.Save(filePath);
            }
        }

        // Сохраняет карту высот в файл PNG
        void SaveHeightMapToPng(NoiseMap heightMap, string filePath)
        {
            int width = heightMap.Width;
            int height = heightMap.Height;

            // Определяет минимальное и максимальное значение высоты в карте
            double minHeight = double.MaxValue;
            double maxHeight = double.MinValue;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double heightValue = heightMap.GetValue(x, y);
                    if (heightValue < minHeight)
                        minHeight = heightValue;
                    if (heightValue > maxHeight)
                        maxHeight = heightValue;
                }
            }

            // Создает изображение и устанавливает оттенки серого пикселей в соответствии с высотой
            using (var image = new Image<L8>(width, height))
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        double heightValue = heightMap.GetValue(x, y);

                        // Масштабирует высоту от 0 до 255
                        byte grayscaleValue = (byte)((heightValue - minHeight) / (maxHeight - minHeight) * 255);

                        // Устанавливает оттенок серого для пикселя
                        image[x, y] = new L8(grayscaleValue);
                    }
                }

                // Сохраняет изображение в файл по указанному пути
                image.Save(filePath);
            }
        }

    }
}

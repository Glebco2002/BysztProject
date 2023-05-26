using System;
using System.IO;
using SharpNoise;
using SharpNoise.Builders;
using SharpNoise.Modules;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using WorldGen;

internal class Program
{
    static void Main()
    {
        while(true)
        {
            Console.Clear();
            PhysMapGen physmapgen = new PhysMapGen();
            GlobalCityMapGen globalcitymapgen = new GlobalCityMapGen();
            DungeonGen dungeongen = new DungeonGen(333,333); //нечётные числа
            Console.WriteLine("Генератор мира от Глеба Котикова, версия alpha 0.6");
            Console.WriteLine();
            Console.WriteLine("Команды:");
            Console.WriteLine("     0 - О программе");
            Console.WriteLine("     1 - Генератор физической карты");
            Console.WriteLine("     2 - Генератор городов на физической карте");
            Console.WriteLine("     3 - Генератор подземелий");
            Console.WriteLine("     Еsc - Выйти");

            // Проверяем ввод пользователя
            ConsoleKey key = Console.ReadKey().Key;

            if (key == ConsoleKey.D0) About();
            else if (key == ConsoleKey.D1) physmapgen.Start();
            else if (key == ConsoleKey.D2) globalcitymapgen.Start();
            else if (key == ConsoleKey.D3) dungeongen.Start();
            else if (key == ConsoleKey.Escape) break;
        }
    }

    static void About()
    {
        Console.Clear();
        Console.WriteLine("История версий:");
        Console.WriteLine("     до 0.1 - несчётное количество сборок, различных тестов. В общем версия 0.1 уже представляет из себя законченный продукт по сути, но я решил ещё её развить.");
        Console.WriteLine("        0.1 - самая первая версия. В отличие от сборок до 0.1, отсюда была убрана генерация карты в текстовый файл и вывод карты прямо в консоль, так как эти фичи работали криво и тормозили работу программмы.");
        Console.WriteLine("        0.2 - была слегка изменена работа генератора карты, было улучшено отображение рельефа");
        Console.WriteLine("              необработанная карта высот стала сохраняться в png файл");
        Console.WriteLine("              также был добавлен этот раздел меню");
        Console.WriteLine("        0.3 - была исправлена надпись версии в главном меню");
        Console.WriteLine("              было улучшено управление в главном меню");
        Console.WriteLine("              добавлена кнопка выхода");
        Console.WriteLine("              консоль теперь очищается");
        Console.WriteLine("        0.4 - quality-of-life изменения");
        Console.WriteLine("              изменена работа генератора карты, улучшен рельеф");
        Console.WriteLine("        0.5 - появился генератор городов на физической карте");
        Console.WriteLine("              улучшена генерация физической карты");
        Console.WriteLine("              разрешение физической карты увеличено втрое");
        Console.WriteLine("        0.6 - слегка улучшена генерация физической карты");
        Console.WriteLine("              разрешение физической карты теперь можно вводить самому. Рекомендуются разрешения от 18000х18000, если компьютер позволяет");
        Console.WriteLine("              quality-of-life изменения");
        Console.WriteLine("              добавлен генератор подземелий");
        Console.WriteLine("              ");
        Console.WriteLine("Нажмите любую кнопку чтобы выйти...");
        Console.ReadKey();
    }
}
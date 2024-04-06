using System;

class Program
{
    static void Main()
    {
        string[] lines =
        {
            "Saman\n",
            "Narimai\n",
            "Developer"
        };

        int startR = 0;
        int endR = 2; // اندازهٔ آرایه برای شماره‌گذاری مقادیر صحیح است
        int startC = 0;
        int endC = 3; // اندازهٔ اولین خط برای شماره‌گذاری مقادیر صحیح است

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                // اگر i و j در محدودهٔ اعداد مورد نظر باشند، با رنگ قرمز نمایش داده شوند
                if (i >= startR && i <= endR && j >= startC && j <= endC)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.Write(lines[i][j]);
            }
        }

        Console.ReadKey();
    }
}

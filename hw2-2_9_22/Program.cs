using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace hw2_2__9_22 {
    internal class Program {
        class Block {
            public Block() {
                isBomb = false; clicked = false;
                num = 0;
            }
            public bool isBomb, clicked;
            public int num;
        }

        static void PrintMap(Block[,] m) {
            Console.Write("  ");
            for (int i = 0; i < m.GetLength(1); i++) Console.Write($" {Convert.ToChar('A' + i)}");
            Console.WriteLine();
            for (int i = 0; i < m.GetLength(0); i++) {
                Console.Write(i < 10 ? $"{i} " : i);
                for (int j = 0; j < m.GetLength(1); j++)
                    Console.Write($" {(!m[i, j].clicked ? "-" : m[i, j].isBomb ? "X" : m[i, j].num)}");
                Console.WriteLine();
            }
        }

        static void Main(string[] args) {
            Random r = new Random();
            Console.Write("設定遊戲參數\n輸入空間的大小: ");
            string? s;
            s = Console.ReadLine();
            int h = Convert.ToInt32(s.Split(',')[0]);
            int w = Convert.ToInt32(s.Split(",")[1]);
            Console.Write("輸入鬼的數量: ");
            int num = Convert.ToInt32(Console.ReadLine());
            if (h * w <= num) {
                Console.WriteLine("遊戲參數錯誤!");
                Environment.Exit(1);
            }

            Block[,] maps = new Block[h, w];
            for (int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                    maps[i, j] = new Block();

            int count = num;
            bool first = true, lose = false, win = false;
            do {
                Console.Clear(); PrintMap(maps);

                Tuple<int, int> pos;
                do {
                    Console.Write("輸入要查看的位置: ");
                    s = Console.ReadLine();
                    pos = new(Convert.ToInt32(s.Split(',')[0]), Convert.ToChar(s.Split(',')[1].ToUpper()) - 'A');
                    if (pos.Item1 < 0 || pos.Item1 >= h || pos.Item2 < 0 || pos.Item2 >= w || maps[pos.Item1, pos.Item2].clicked)
                        Console.WriteLine("無效的輸入，請再試一次");
                    else break;
                } while (true);

                if (first) {
                    for (; num > 0; num--) {
                        Tuple<int, int> rPos = new(r.Next(h), r.Next(w));
                        if (maps[rPos.Item1, rPos.Item2].isBomb || rPos.Equals(pos)) {
                            num++; continue;
                        }
                        maps[rPos.Item1, rPos.Item2].isBomb = true;
                        for (int i = -1; i <= 1; i++) {
                            for (int j = -1; j <= 1; j++) {
                                if (rPos.Item1 + i < 0 || rPos.Item1 + i >= h || rPos.Item2 + j < 0 || rPos.Item2 + j >= w) continue;
                                ++maps[rPos.Item1 + i, rPos.Item2 + j].num;
                            }
                        }
                    }
                    first = false;
                }

                maps[pos.Item1, pos.Item2].clicked = true;
                if (maps[pos.Item1, pos.Item2].isBomb) lose = true;
                if (++count == w * h && !lose) win = true;
            } while (!lose && !win);

            for (int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                    maps[i, j].clicked = true;

            Console.Clear(); PrintMap(maps);
            Console.WriteLine(win ? "遊戲結束! 你成功躲避所有的鬼了" : "遊戲結束! 你被鬼抓到了");
        }
    }
}
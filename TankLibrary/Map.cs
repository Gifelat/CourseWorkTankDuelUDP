using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TankLibrary
{
    /// <summary>
    /// Класс, представляющий карту с различными элементами.
    /// </summary>
    public class Map
    {
        private byte[] _pxColor;

        /// <summary>
        /// Получает карту в виде двумерного массива.
        /// </summary>
        /// <returns>Двумерный массив, представляющий карту.</returns>
        public int[,] GetMap()
        {
            Random rd = new Random();
            string[] files = Directory.GetFiles(@"..\..\..\img\map\");
            FileInfo file = new FileInfo(files[rd.Next(files.Length)]);

            Uri pathUri = new Uri(file.FullName);

            BitmapImage bm = new BitmapImage(pathUri);
            int height = bm.PixelHeight;
            int width = bm.PixelWidth;

            _pxColor = new byte[height * width * 4];
            bm.CopyPixels(_pxColor, width * 4, 0);
            StringBuilder sb = new StringBuilder();

            for (int x = 0; x < _pxColor.Length; x++)
            {
                if (x % 4 != 0 || x == 0)
                    sb.Append(_pxColor[x]);
                else
                    sb.Append(" " + _pxColor[x]);
            }

            string[] parts = sb.ToString().Split(' ');

            int[,] walls = new int[16, 29]; // 16, 29 - 1920X1080 // 22 35 16:10

            sb.Clear();
            for (int i = 0; i < parts.Length; i++)
            {
                switch (parts[i])
                {
                    case "255255255255":
                        parts[i] = "0";
                        break;
                    case "164164164255":
                        parts[i] = "1";
                        break;
                    case "818181255":
                        parts[i] = "2";
                        break;
                    case "100255100255":
                        parts[i] = "3";
                        break;
                    case "100160200255":
                        parts[i] = "4";
                        break;
                    default:
                        parts[i] = "4";
                        break;
                }
            }

            int a = 0;
            for (int y = 0; y < walls.GetLength(0); y++)
            {
                for (int x = 0; x < walls.GetLength(1); x++)
                {
                    walls[y, x] = int.Parse(parts[a++]);
                }
            }

            return walls;
        }
    }
}

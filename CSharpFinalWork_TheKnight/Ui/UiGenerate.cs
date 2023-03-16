using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Console2048
{
    internal static class UiGenerate
    {
        const int heightLimit = 37;
        const int widthLimit = 140;

        const int plotX = 0;
        const int plotY = 0;
        const int menuX = 0;
        const int menuY = 26;
        const int stateX = 91;
        const int stateY = 0;
        const int noteX = 91;
        const int noteY = 26;


        public enum WindowSelect
        {
            Plot,
            Menu,
            State,
            Note
        }
        /// <summary>
        /// 畫出內容。 回傳：client 的選擇(or -1) min is 0。
        /// </summary>
        public static int RenderOut(bool clearWindow, WindowSelect window, params string[] lines)
        {
            int result = -1;
            int y = -1, x = -1;
            int maxX = -1, maxY = -1;
            if (clearWindow)
            {
                Clear();
                DefaultPattern();
            }
            switch (window)
            {
                case WindowSelect.Plot:
                    y = plotY;
                    x = plotX;
                    maxX = noteX - 1;
                    maxY = noteY - 1;
                    break;
                case WindowSelect.Menu: //注意
                    y = menuY;
                    x = menuX;
                    maxX = noteX - 1;
                    maxY = heightLimit - 1;
                    break;
                case WindowSelect.State:
                    y = stateY;
                    x = stateX;
                    maxX = widthLimit - 1;
                    maxY = noteY - 1;
                    break;
                case WindowSelect.Note:
                    y = noteY;
                    x = noteX;
                    maxX = widthLimit - 1;
                    maxY = heightLimit - 1;
                    break;
            }
            List<string> linesFixed = new List<string>();
            for (int i = 0; i < lines.Length; i++)
            {
                string a = "";
                string b = "";
                int byteLength = Encoding.Default.GetByteCount(lines[i]);
                if (byteLength > maxX)
                {
                    a = SubStringByByte(lines[i], 0, maxX);
                    b = SubStringByByte(lines[i], maxX, byteLength - maxX);
                    linesFixed.Add(a);
                    linesFixed.Add(b);
                }
                else
                {
                    a = lines[i];
                    linesFixed.Add(a);
                }
            }
            if (linesFixed.Count > maxY)
            {
                throw new Exception("超出顯示限制");
            }

            if (window == WindowSelect.Menu)
            {
                result = ConsoleUiHelper.SetMenu(menuY, menuX, linesFixed.ToArray());
            }
            else
            {
                foreach (var item in linesFixed)
                {
                    SetCursorPosition(x, y);
                    WriteLine(item);
                    y++;
                }
            }
            return result;
        }
        public static int RenderOutEnumMenu<TEnum>() where TEnum : Enum
        {
            int result = RenderOut(false, WindowSelect.Menu, Enum.GetNames(typeof(TEnum)));
            return result;
        }
        public static string RenderOutMenuReadLine(string hint)
        {
            SetCursorPosition(menuX, menuY);
            WriteLine(hint);
            return ReadLine();
        }
        public static void PressAnyKeyToContinue()
        {
            SetCursorPosition(0, 26);
            WriteLine("按下任意鍵繼續");
            ReadKey(true);
        }
        /// <summary>
        /// 畫出本遊戲的框線
        /// </summary>
        private static void DefaultPattern()
        {
            ConsoleUiHelper.SetConsoleWindowSize(ConsoleUiHelper.WindowSize.median);
            ConsoleUiHelper.DrawVerticalLine(0, 90, heightLimit);
            ConsoleUiHelper.DrawHorizontalLine(0, 25, widthLimit);
        }

        private static string SubStringByByte(string str, int start, int length)
        {
            int len = length;
            int byteCount = System.Text.Encoding.Default.GetByteCount(str);
            //修改最大長度，防止溢位
            if (len > byteCount)
            {
                len = byteCount;
            }
            var strBytes = System.Text.Encoding.Default.GetBytes(str);
            string substr = System.Text.Encoding.Default.GetString(strBytes, start, len);
            //對於半個中文字元的特殊處理
            if (substr.EndsWith("?"))
            {
                //判斷原字串是否包含問號
                var c = str.Substring(substr.Length - 1, 1);
                if (!c.Equals("?"))
                {
                    substr = substr.Substring(0, substr.Length - 1);
                }
            }
            return substr;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Console2048
{
    /// <summary>
    /// 小黑窗的UI輔助類
    /// </summary>
    public static class ConsoleUiHelper
    {
        /// <summary>
        /// 畫直。
        /// startYPosition：起始高度(min:0)。
        /// xPosition：橫向位置(min:0)。
        /// length： 長度 。
        /// word： 畫的字元(預設:|) 。
        /// </summary>
        public static void DrawVerticalLine(int startYPosition, int xPosition, int length, string word = "|")
        {
            for (int i = 0; i < length; i++)
            {
                SetCursorPosition(xPosition, startYPosition + i);
                WriteLine(word);
            }
        }
        /// <summary>
        /// 畫橫
        /// startYPosition：起始高度(min:0)。
        /// xPosition：橫向位置(min:0)。
        /// length： 長度 。
        /// word： 畫的字元(預設:-) 。
        /// </summary>
        /// <param name="startXPosition"></param>
        /// <param name="yPosition"></param>
        /// <param name="length"></param>
        /// <param name="word"></param>
        public static void DrawHorizontalLine(int startXPosition, int yPosition, int length, string word = "-")
        {
            SetCursorPosition(startXPosition, yPosition);
            for (int i = 0; i < length; i++)
            {
                Write(word);
            }
        }
        /// <summary>
        /// 小黑窗尺寸
        /// </summary>
        public enum WindowSize
        {
            /// <summary>
            /// 全螢幕
            /// </summary>
            FullSize,
            /// <summary>
            /// 190X50
            /// </summary>
            large,
            /// <summary>
            /// 140X37
            /// </summary>
            median,
            /// <summary>
            /// 90X34
            /// </summary>
            small
        }
        /// <summary>
        /// 設定黑窗大小(預設=small)
        /// </summary>
        public static void SetConsoleWindowSize(WindowSize ws = WindowSize.small)
        {
            switch (ws)
            {
                case WindowSize.FullSize:
                    SetWindowSize(240, 63);
                    break;
                case WindowSize.large:
                    SetWindowSize(190, 50);
                    break;
                case WindowSize.median:
                    SetWindowSize(140, 37);
                    break;
                case WindowSize.small:
                    SetWindowSize(90, 34);
                    break;
            }
        }
        static int oriX=-1;
        static int oriY=-1;
        /// <summary>
        /// 已有初始值
        /// </summary>
        static bool HasOriginValue
        {
            get
            {
                return (oriX >= 0 && oriY >= 0); 
            }
        }
        static void LeaveOperation()
        {
            if (HasOriginValue)
            {
                SetCursorPosition(oriX, oriY);
                Write("　");
            }
        }

        /// <summary>
        /// 可用上下按鈕的選單效果
        /// </summary>
        public static int SetMenu(int y, int x, params string[] operations)
        {
            int row = operations.Length;
            //write menu
            SetCursorPosition(x, y);
            foreach (string item in operations)
            {
                WriteLine($"　　{item.ToString()}");//預留給光標圖示的空白
            }

            //set cursor
            SetCursorPosition(x, y);
            oriX = x;
            oriY = y;
            Write("》");

            //read client operate
            int oldtop = y;
            bool tag = true;
            do
            {
                ConsoleKeyInfo info = ReadKey(true); //bool: true to not display the pressed key; otherwise, false.
                switch (info.Key)
                {
                    case ConsoleKey.Enter:
                        tag = false;
                        break;
                    case ConsoleKey.UpArrow:
                        if (y > oldtop && y <= oldtop + row - 1)
                        {
                            LeaveOperation();
                            y -= 1;
                            Console.SetCursorPosition(x, y);
                            Console.Write("》");
                        }
                        else
                        {
                            Console.SetCursorPosition(x, y);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (y >= oldtop && y < oldtop + row - 1)
                        {
                            LeaveOperation();
                            y += 1;
                            Console.SetCursorPosition(x, y);
                            Console.Write("》");
                        }
                        else
                        {
                            Console.SetCursorPosition(x, y);
                        }
                        break;
                    default:
                        Console.SetCursorPosition(x, y);
                        break;
                }
                oriX = x;
                oriY = y;
            } while (tag);

            int selectedRowIndex = y - oldtop;
            return selectedRowIndex;
        }
    }
}

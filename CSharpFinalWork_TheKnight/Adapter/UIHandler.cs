using Console2048;
using System;
using System.Reflection;

namespace CSharpFinalWork_TheKnight
{
    public interface UIHandler
    {
        int ReadChoice(params string[] choices);
        int ReadChoice<TEnum>() where TEnum : Enum;
        string ReadKeyIn( string question);
        void ShowDialogue(string who = "", params string[] sentances);
        void ShowState(params string[] states);
        void ShowEffect(params string[] states);
    }

    public class ConsoleUI : UIHandler
    {
        public int ReadChoice(params string[] choices)
        {
            if (choices.Length == 0)
            {
                UiGenerate.PressAnyKeyToContinue();
                return -1;
            }
            return UiGenerate.RenderOut(false, UiGenerate.WindowSelect.Menu, choices);
        }

        public string ReadKeyIn(string question)
        {
            return UiGenerate.RenderOutMenuReadLine(question);
        }

        public void ShowDialogue(string who = "??：", params string[] sentances)
        {
            UiGenerate.RenderOut(true, UiGenerate.WindowSelect.Plot, sentances);
        }

        public void ShowState(params string[] states)
        {
            UiGenerate.RenderOut(false, UiGenerate.WindowSelect.State, states);
        }

        int UIHandler.ReadChoice<TEnum>()
        {
           return UiGenerate.RenderOutEnumMenu<TEnum>();
        }
    }
}
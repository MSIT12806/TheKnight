using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFinalWork_TheKnight.Domain
{
    public class Plot
    {
        public IEnumerable<string> FirstPlot_Intro()
        {
            var r = new List<string>();
            r.Add("??：");
            r.Add("勇者，你費盡千辛萬苦終於到了這個地方，");
            r.Add("眼前矗立著最後的難關──一個巨大的魔像，");
            r.Add("打敗他以後，你將成功的拯救公主，並與她共度美好的餘生。");

            return r;
        }
        public IEnumerable<string> FirstPlot_IntroPoint()
        {
            var r = new List<string>();
            r.Add("??：");
            r.Add("你一路走來累積的經歷和收穫，在此刻變化成 100個分配點數，好好分配它，你將有機會打敗眼前的魔像，往你嚮往的未來前進。");

            return r;
        }
    }
}

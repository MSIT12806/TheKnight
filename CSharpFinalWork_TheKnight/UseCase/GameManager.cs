using Console2048;
using CSharpFinalWork_TheKnight.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CSharpFinalWork_TheKnight
{

    //Use Case:
    //實體→說故事  //Narration
    //實體→顯示英雄屬性 //Hero
    //實體→顯示英雄狀態 //Hero : FightCharacter
    //實體→顯示敵人狀態 //Enemy : FightCharacter
    //實體→傳送選擇 //Hero
    //實體→戰鬥歷程
    //玩家→進行選擇

    /// <summary>
    /// 生成遊戲用的物件、管理遊戲流程等。
    /// </summary>
    public class GameManager
    {
        Player Player { get; set; }
        Giant Giant { get; set; }
        public int Round { get; set; }
        bool win = false;
        List<string> fightProcess = new List<string>();

        public GameManager(UIHandler ui, Dialogue dialogue)
        {
            _ui = ui;
            _dialogue = dialogue;
        }
        Dialogue _dialogue;
        UIHandler _ui;
        public void Game()
        {
            while (!win)
            {
                //點評：這個有點像是 UseCase？ 去呼叫 顯示劇情、設定戰鬥屬性、進行戰鬥、結束劇情等等。
                ShowPlot();
                //選擇難易度？
                Giant = new Giant();
                SetPlayer();
                Fight();
                if (End())
                {
                    UiGenerate.RenderOut(true, UiGenerate.WindowSelect.Plot, "遊戲結束");
                    UiGenerate.PressAnyKeyToContinue();
                    return;
                }
                Round++;
                fightProcess.Clear();
            }
        }


        public void ShowPlot()
        {
            //點評：直接跟 Console 耦合了，文字敘述跟介面應該分開，這個也不應該叫做 Start
            _ui.ShowDialogue("", "是否觀看劇情？");
            bool isReadPlot= _ui.ReadChoice("是", "否") == 0;
            if (isReadPlot)
            {
                _ui.ShowDialogue(sentances: _dialogue.StoryIntro().ToArray());
                _ui.ReadChoice();
                _ui.ShowDialogue(sentances: _dialogue.PointIntro().ToArray());
                _ui.ReadChoice();
            }
        }
        public void SetPlayer()
        {
            //產生一個Player 物件
            var player = new Player(_ui);
            //讓 Client 分配點數
            player.DistributeProperty();
            //set Player 物件
            Player = player;
        }

        public void Fight()
        {
            //前置作業

            //1. 綁定雙方戰鬥物件
            Player.SetOpponent(Giant);
            Giant.SetOpponent(Player);
            //2. 生成戰鬥屬性
            //依照點數產生相對應的裝備 & 技能 & 從gm注入要挑戰的對象
            Player.SetFightProperty();
            Player.SetState(Round);
            Giant.SetState(Round);
            //3. 排定戰鬥順序
            Func<FightCharacter, FightCharacter, FightCharacter> findNowFighter = (a, b) =>
            {
                return a.FightRoundPoint >= b.FightRoundPoint ? b : a;
            };
            //3. 戰鬥迴圈
            while (Player.Stamina >= 0 && Player.Hp >= 0 && Giant.Hp >= 0)
            {

                UiGenerate.RenderOut(true, UiGenerate.WindowSelect.Plot, "");
                UiGenerate.RenderOut(false, UiGenerate.WindowSelect.Plot, fightProcess.ToArray());
                Giant.ShowState();
                Player.ShowState();
                FightCharacter nowFighter = RonToolsSt20.Compare.Best<FightCharacter>(findNowFighter, Player, Giant);
                nowFighter.FightRoundPoint += nowFighter.FightRoundUnit;
                ////輪到魔像的基本AI
                ////輪到玩家的攻擊選擇
                Thread.Sleep(2000);
                nowFighter.ResetState();
                nowFighter.SetBuff();

                if (nowFighter == Player)
                    Player.SelectSkill();
                else
                    Giant.RandomSkill();

                fightProcess.AddRange(nowFighter.NowFightContext);
                nowFighter.NowFightContext.Clear();
            }
            if (Player.Stamina <= 0 || Player.Hp <= 0)
                win = false;
            else if (Giant.Hp <= 0)
                win = true;

        }

        private bool End()
        {
            if (win)
            {
                UiGenerate.RenderOut(true, UiGenerate.WindowSelect.Plot, "您成功擊敗魔像");
                UiGenerate.PressAnyKeyToContinue();
                UiGenerate.RenderOut(true, UiGenerate.WindowSelect.Plot, "但是沒有公主");
                UiGenerate.PressAnyKeyToContinue();
                UiGenerate.RenderOut(true, UiGenerate.WindowSelect.Plot, "當然也沒有從此幸福快樂的生活");
                UiGenerate.PressAnyKeyToContinue();
                UiGenerate.RenderOut(true, UiGenerate.WindowSelect.Plot, "明天還要上班上學");
                UiGenerate.PressAnyKeyToContinue();
                UiGenerate.RenderOut(true, UiGenerate.WindowSelect.Plot, "早點睡吧，你人生的鬥爭還長著呢。");
                UiGenerate.PressAnyKeyToContinue();
                return true;
            }
            else
            {
                UiGenerate.RenderOut(true, UiGenerate.WindowSelect.Plot, "您失敗了", "是否重新嘗試？");
                bool end = UiGenerate.RenderOut(false, UiGenerate.WindowSelect.Menu, "是", "否") == 1;
                return end;
            }
        }

    }


}
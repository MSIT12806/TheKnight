using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console2048
{
    public class Giant : FightCharacter
    {
        public Giant()
        {
            Skills.Add("普攻", p =>
            {
                return ((int)(p.Attack * AttackBuff), 0);
            });
            Skills.Add("衝撞", p =>
            {
                return ((int)(p.Attack * AttackBuff), 0);
            });

            Name = "魔像";

        }

        public override int FightRoundUnit { get; protected set; }
        public override int FightRoundPoint { get; set; }
        public override FightCharacter opponent { get; protected set; }

        public override Dictionary<string, ReleaseSkill> Skills { get; } = new Dictionary<string, ReleaseSkill>();
        public override Sword Sword { get; protected set; }
        public override Shield Shield { get; protected set; }
        public override int Hp { get; set; }
        public override float Stamina { get; set; }
        public override int Attack { get; set; }
        public override float Speed { get; set; }
        public override float Avoid { get; set; }
        public override float Stun { get; set; }
        public override float Collide { get; set; }

        internal void RandomSkill()
        {
            Random random = new Random();
            int skillIdx = random.Next(0, Skills.Count - 1);
            var skill = Skills[Skills.Keys.ToArray()[skillIdx]];


            NowFightContext.Add($"{this.Name} 對 {opponent.Name} 施展 {Skills.Keys.ToArray()[skillIdx]}");

            (int d, int r) = skill(opponent);
            if (r > 0)
                NowBuffs.Enqueue((Skills.Keys.ToArray()[skillIdx], r - 1, opponent));
            NowFightContext.Add(opponent.GetHurt(d));

        }


        internal override void SetState(int round)
        {
            float level = 50 * ((11 - (float)round) / 10);
            Hp = (int)level * 20;

            FightRoundUnit = (int)(1000 / level + 1);
            this.Attack = (int)level;
            this.Speed = Speed = 1000 / level;
            this.Avoid = level / 1000;
            this.Collide = level / 1000;
            this.Stun = level / 1000;
        
        }

        internal void ShowState()
        {
            List<string> builder = new List<string>();
            builder.Add($"魔像狀態：");
            builder.Add($"Hp:{Hp}");
            builder.Add($"力竭:{Stamina}");
            builder.Add($"速度:{Speed}");
            builder.Add($"攻擊力：{Attack}");
            builder.Add($"閃避率：{Avoid}");
            builder.Add($"擊暈率：{Stun}");
            builder.Add($"格檔成功率：{Collide}");
            builder.Add("");
            builder.Add($"狀態：");
            builder.AddRange(NowBuffs.Select(i => i.Item1).ToArray());
            UiGenerate.RenderOut(false, UiGenerate.WindowSelect.Note, builder.ToArray());
        }
    }

}

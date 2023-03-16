using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console2048
{
    /// <summary>
    /// 傳入：目標。回傳：攻擊 、回合數
    /// </summary>
    public delegate (int damage, int rounds) ReleaseSkill(FightCharacter target);
    public class Player : FightCharacter
    {
        //前置屬性
        public int OriginPoint { get; private set; }
        public Player()
        {
            Name = "勇者";
            OriginPoint = 100;
        }

        #region 分配階段

        int _power;
        int _endurance;
        int _agile;
        int _sword;
        int _sheild;

        public enum PlayerBasicProperty
        {
            Power,
            Endurance,
            Agile,
            Sword,
            Sheild,
        }

        //體質
        public int Power { get { return _power; } set { if (DecreacePointWhenPropertyAdd(value - _power)) _power = value; } }
        public int Endurance { get { return _endurance; } set { if (DecreacePointWhenPropertyAdd(value - _endurance)) _endurance = value; } }
        public int Agile { get { return _agile; } set { if (DecreacePointWhenPropertyAdd(value - _agile)) _agile = value; } }
        public int SwordPoint { get { return _sword; } set { if (DecreacePointWhenPropertyAdd(value - _sword)) _sword = value; } }
        public int ShieldPoint { get { return _sheild; } set { if (DecreacePointWhenPropertyAdd(value - _sheild)) _sheild = value; } }

        //戰鬥前置屬性
        public override int FightRoundUnit { get; protected set; }
        public override int FightRoundPoint { get; set; }
        public override FightCharacter opponent { get; protected set; }

        //戰鬥體質
        public override Dictionary<string, ReleaseSkill> Skills { get; } = new Dictionary<string, ReleaseSkill>();
        public override int Hp { get; set; }
        public override float Stamina { get; set; }
        public override float Speed { get; set; }
        public override Sword Sword { get; protected set; }
        public override Shield Shield { get; protected set; }
        public override int Attack { get; set; }
        public override float Avoid { get; set; }
        public override float Stun { get; set; }
        public override float Collide { get; set; }

        internal void ShowDistribute()
        {
            UiGenerate.RenderOut(true, UiGenerate.WindowSelect.Plot, "分配你的點數，以找出擊敗魔像的方法。", $"剩餘點數：{OriginPoint}", "請選擇屬性");
        }

        public void Distribute(PlayerBasicProperty s)
        {
            UiGenerate.RenderOut(true, UiGenerate.WindowSelect.Plot, "分配你的點數，以找出擊敗魔像的方法。", $"剩餘點數：{OriginPoint}", $"已選擇：{s}");
            string input = UiGenerate.RenderOutMenuReadLine("請輸入要加多少點數：");
            int inputPoint = -1;
            if (!int.TryParse(input, out inputPoint))
            {
                UiGenerate.RenderOut(true, UiGenerate.WindowSelect.Plot, "錯誤：輸入的不是數值。");
                UiGenerate.PressAnyKeyToContinue();
                return;
            }

            switch (s)
            {
                case PlayerBasicProperty.Power:
                    Power += inputPoint;
                    break;
                case PlayerBasicProperty.Endurance:
                    Endurance += inputPoint;
                    break;
                case PlayerBasicProperty.Agile:
                    Agile += inputPoint;
                    break;
                case PlayerBasicProperty.Sword:
                    SwordPoint += inputPoint;
                    break;
                case PlayerBasicProperty.Sheild:
                    ShieldPoint += inputPoint;
                    break;
                default:
                    break;
            }
        }



        internal void DistributeProperty()
        {

            while (true)
            {

                if (OriginPoint < 10)
                {
                    UiGenerate.RenderOut(true, UiGenerate.WindowSelect.Plot, "是否進入戰鬥？");
                    ShowState();
                    if (UiGenerate.RenderOut(false, UiGenerate.WindowSelect.Menu, "是", "否") == 0)
                        return;
                }
                ShowDistribute();
                ShowState();
                int s = UiGenerate.RenderOutEnumMenu<Player.PlayerBasicProperty>();
                Distribute((Player.PlayerBasicProperty)s);

            }

        }
        /// <summary>
        /// return: 點數是否足夠
        /// </summary>
        private bool DecreacePointWhenPropertyAdd(int val)
        {
            if (val > OriginPoint)
            {
                UiGenerate.RenderOut(true, UiGenerate.WindowSelect.Plot, "錯誤：你的點數不足。");
                UiGenerate.PressAnyKeyToContinue();
                return false;
            }
            OriginPoint -= val;
            return true;
        }
        #endregion
        #region 戰鬥前置階段

        public void SetFightProperty()
        {
            Hp = Power * 3 + Endurance * 5;
            Stamina = Endurance;
            Speed = Stamina;

            //gen Shield
            if (ShieldPoint > 35 && SwordPoint <= 0)
                Shield = new SuperHeavyShield(this);
            else if (ShieldPoint > 35)
                Shield = new HeavyShield(this);
            else if (ShieldPoint > 20)
                Shield = new NiceShield(this);
            else if (ShieldPoint > 0)
                Shield = new NormalShield(this);
            else
                Shield = new NoShield(this);

            //gen Sword
            if (SwordPoint > 35 && ShieldPoint <= 0)
                Sword = new TwoHandSword(this);
            else if (SwordPoint > 35)
                Sword = new NiceSword(this);
            else if (SwordPoint > 20)
                Sword = new GoodSword(this);
            else if (SwordPoint > 0)
                Sword = new NormalSword(this);
            else
                Sword = new NoSword(this);

            this.GetSkills();

        }

        private void GetSkills()
        {
            //from gears
            foreach (var item in Shield.GetSkills())
            {
                if (!Skills.ContainsKey(item.Key))
                    Skills.Add(item.Key, item.Value);
                else
                    throw new Exception("技能名稱出現重複");
            }
            foreach (var item in Sword.GetSkills())
            {
                if (!Skills.ContainsKey(item.Key))
                    Skills.Add(item.Key, item.Value);
                else
                    throw new Exception("技能名稱出現重複");
            }   

            //from property
            if (this.Power >= 35)
            {
                Skills.Add("勢如破竹", p =>
                {
                    p.Stamina -= 1;
                    p.AttackBuff += 0.2f;
                    return (0, 5);
                });
            }
            else if (this.Endurance >= 35)
            {
                Skills.Add("血牛爆發", p =>
                {
                    p.Stamina -= 1;
                    p.Stamina *= 0.75f;
                    Attack = (int)(p.Attack * 1.5f);
                    return (0, 0);
                });
            }
            else if (this.Agile >= 35)
            {
                Skills.Add("暗隱伏擊", p =>
                {
                    p.Stamina -= 1;
                    p.Stun *= 3;
                    return (0, 3);
                });
            }
        }

        public void SetFightRoundUnitValue()
        {
            FightRoundUnit = 1 / Agile;
        }

        internal override void SetState(int round)
        {
            this.FightRoundUnit = 1000 / (Agile + 1);
            this.Attack = Power + Sword.AttackPoint;
            this.Speed = 1000 / (Agile + 1);
            this.Avoid = Agile / 1000;
            this.Collide = Power / 1000;
            this.Stun = Power / 1000;
        }

        #endregion
        #region 戰鬥階段
        internal void SelectSkill()
        {
            //跳出選單讓玩家選擇技能(回傳陣列字串)
            var skillsKeyArray = Skills.Keys.ToArray();
            int skillIndex = UiGenerate.RenderOut(false, UiGenerate.WindowSelect.Menu, skillsKeyArray);

            UiGenerate.RenderOut(true, UiGenerate.WindowSelect.Plot, "請選擇對象");
            int targetIdx = UiGenerate.RenderOut(false, UiGenerate.WindowSelect.Menu, "魔像", "自己");
            FightCharacter t = targetIdx == 0 ? opponent : this;
            NowFightContext.Add($"{this.Name} 對 {t.Name} 施展 {skillsKeyArray[skillIndex]}");
            int damage = Move(t, skillsKeyArray[skillIndex]);

            NowFightContext.Add(t.GetHurt(damage));
            int Move(FightCharacter target, string skillName)
            {
                //注入玩家選擇的技能，並回傳值。
                var skill = Skills[skillName];
                if (skill != null)
                {
                    (var d, var r) = skill(target);
                    if (r > 0)
                        NowBuffs.Enqueue((skillName, r, target));

                    return d;
                }
                return 0;
            }
        }


        #endregion



        internal void ShowState()
        {
            List<string> builder = new List<string>();
            builder.Add($"基本數值：");
            builder.Add($"力量：{Power}");
            builder.Add($"耐力：{Endurance}");
            builder.Add($"敏捷：{Agile}");
            builder.Add($"劍：{Sword}");
            builder.Add($"盾：{Shield}");
            builder.Add($"");
            builder.Add($"戰鬥數值：");
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
            UiGenerate.RenderOut(false, UiGenerate.WindowSelect.State, builder.ToArray());
        }

    }
}

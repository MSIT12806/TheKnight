using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console2048
{
    public abstract class Sword
    {
        protected Player _player;
        /// <summary>
        /// 注入角色以取得其屬性，依據屬性會生成不同的技能。
        /// </summary>
        public Sword(Player player)
        {
            _player = player;
        }
        public abstract string Name { get; }
        public int AttackPoint { get; protected set; }

        protected Dictionary<string, ReleaseSkill> Skills;

        /// <summary>
        /// 取得本裝備的技能(不想讓外部改到這個，所以回傳一個新的)
        /// </summary>
        public Dictionary<string, ReleaseSkill> GetSkills()
        {
            return new Dictionary<string, ReleaseSkill>(Skills);
        }
        /// <summary>
        /// 所有裝備連動的技能都從sword整合。
        /// </summary>
        protected virtual IEnumerable<string> AppendSkillsByShield()
        {
            return Skills.Keys;
        }
        /// <summary>
        /// 攻擊
        /// </summary>
        protected virtual (int damage, int rounds) Attack(FightCharacter p)
        {
            _player.Stamina -= 1;
            return ((int)((_player.Attack + AttackPoint) * _player.AttackBuff), 0);
        }
    }
    /// <summary>
    /// 赤手空拳
    /// </summary>
    internal class NoSword : Sword
    {

        /// <summary>
        /// 注入角色以取得其屬性，依據屬性會生成不同的技能。
        /// </summary>
        public NoSword(Player player) : base(player)
        {
            AttackPoint = 0;
            Skills = new Dictionary<string, ReleaseSkill>();
            Skills.Add("徒手毆打", Attack);
        }

        public override string Name { get; } = "赤手空拳";

        /// <summary>
        /// 赤手空拳可得雙手盾相關技能
        /// </summary>
        protected override IEnumerable<string> AppendSkillsByShield()
        {
            Shield shield = _player.Shield;
            if (shield.GetType().Name == typeof(SuperHeavyShield).Name)
            {
                Skills.Add("固若金湯", p =>
                {
                    p.Stamina -= 2;//雙手武器技能耐力值消耗*2
                    p.Collide *= 10;
                    return (0, 5);
                });
                Skills.Add("天崩地裂", p =>
                {
                    p.Stamina -= 5;
                    p.Collide = 1;
                    return (p.Attack, 0);
                });
            }

            return Skills.Keys;
        }
    }
    /// <summary>
    /// 劍
    /// </summary>
    internal class NormalSword : Sword
    {
        public override string Name { get; } = "劍";

        /// <summary>
        /// 注入角色以取得其屬性，依據屬性會生成不同的技能。
        /// </summary>
        public NormalSword(Player player) : base(player)
        {
            AttackPoint = 10;
            Skills = new Dictionary<string, ReleaseSkill>();
            Skills.Add("普通砍擊", Attack);
        }
    }
    /// <summary>
    /// 利劍
    /// </summary>
    internal class GoodSword : Sword
    {
        public override string Name { get; } = "利劍";

        /// <summary>
        /// 注入角色以取得其屬性，依據屬性會生成不同的技能。
        /// </summary>
        public GoodSword(Player player) : base(player)
        {
            AttackPoint = 18;
            Skills = new Dictionary<string, ReleaseSkill>();
            Skills.Add("好的砍擊", Attack);
        }
    }
    /// <summary>
    /// 精良利劍
    /// </summary>
    internal class NiceSword : Sword
    {
        public override string Name { get; } = "精良利劍";

        /// <summary>
        /// 注入角色以取得其屬性，依據屬性會生成不同的技能。
        /// </summary>
        public NiceSword(Player player) : base(player)
        {
            AttackPoint = 28;
            Skills = new Dictionary<string, ReleaseSkill>();
            Skills.Add("優良砍擊", Attack);
            if (player.Shield.Name == "精良盾")
                Skills.Add("精良狀態", t =>
                {
                    _player.AttackBuff += 0.2f;
                    _player.DefenceBuff += 20f;
                    return (0, 5);
                });
        }
    }
    /// <summary>
    /// 雙手劍
    /// </summary>
    internal class TwoHandSword : Sword
    {
        public override string Name { get; } = "雙手劍";
        protected override (int damage, int rounds) Attack(FightCharacter target)
        {
            _player.Stamina -= 1;
            return base.Attack(target);
        }

        /// <summary>
        /// 注入角色以取得其屬性，依據屬性會生成不同的技能。
        /// </summary>
        public TwoHandSword(Player player) : base(player)
        {
            AttackPoint = 40;
            Skills = new Dictionary<string, ReleaseSkill>();
            Skills.Add("特級砍擊", Attack);
            Skills.Add("天崩地裂斬", p =>
            {
                p.AttackBuff += 4;
                return Attack(p.opponent);
            });
            Skills.Add("以劍代盾", p =>
            {
                p.DefenceBuff += p.Sword.AttackPoint * 0.8f;
                return (0, 5);
            });
        }
    }
}

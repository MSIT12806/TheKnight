using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console2048
{
    /// <summary>
    /// 抽象類 盾牌
    /// </summary>
    public abstract class Shield
    {
        protected Dictionary<string, ReleaseSkill> Skills = new Dictionary<string, ReleaseSkill>();
        protected Player _player;
        public Shield(Player player)
        {
            _player = player;
        }

        public abstract string Name { get; }
        /// <summary>
        /// 防禦點數(增加在FightChracter)
        /// </summary>
        public int DefensePoint { get; protected set; }
        /// <summary>
        /// 可覆寫：防禦行為
        /// </summary>
        public virtual int Defense(int damage)
        {
            damage = Collide(damage);
            damage -= DefensePoint;
            return damage;
        }
        public virtual int Collide(int damage)
        {
            Random r = new Random();
            int cPoint = r.Next(0, 100);
            if (cPoint > _player.Collide)
            {
                return damage - _player.Power;
            }

            return damage;
        }
        /// <summary>
        /// 攻擊
        /// </summary>
        public virtual (int damage, int rounds) Attack(FightCharacter target)
        {
            //target.擊暈判定(_player.Stun + _player.StunBuff);
            return ((int)((_player.Attack + DefensePoint * 0.5f) * _player.AttackBuff), 0);
        }

        /// <summary>
        /// 取得本裝備的技能(不想讓外部改到這個，所以回傳一個新的)
        /// </summary>
        public Dictionary<string, ReleaseSkill> GetSkills()
        {
            return new Dictionary<string, ReleaseSkill>(Skills);
        }
    }
    /// <summary>
    /// 超重盾
    /// </summary>
    internal class SuperHeavyShield : Shield
    {
        public SuperHeavyShield(Player player) : base(player)
        {
            DefensePoint = 50;
            player.Stun *= 1.2f;
            Skills.Add("超重量盾擊", t =>
            {
                _player.Stamina -= 1;
                return Attack(t);
            });
        }

        public override string Name => "超重盾";


    }
    /// <summary>
    /// 厚重盾
    /// </summary>
    internal class HeavyShield : Shield
    {
        public override string Name => "厚重盾";
        public HeavyShield(Player player) : base(player)
        {
            DefensePoint = 30;
            Skills.Add("重盾擊", Attack);
        }

    }
    /// <summary>
    /// 精良盾
    /// </summary>
    internal class NiceShield : Shield
    {
        public override string Name => "精良盾";
        public NiceShield(Player player) : base(player)
        {
            DefensePoint = 20;
            Skills.Add("良盾擊", Attack);
        }

    }
    /// <summary>
    /// 盾
    /// </summary>
    internal class NormalShield : Shield
    {
        public override string Name => "盾";
        public NormalShield(Player player) : base(player)
        {
            DefensePoint = 10;
            Skills.Add("盾擊", Attack);
        }

    }
    /// <summary>
    /// 無盾
    /// </summary>
    internal class NoShield : Shield
    {
        public override string Name => "無盾";
        public NoShield(Player player) : base(player)
        {
            DefensePoint = 0;
        }
        public override int Collide(int damage)
        {
            return damage;
        }
    }
}

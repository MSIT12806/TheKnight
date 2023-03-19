using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console2048
{
    /// <summary>
    /// 成員有戰鬥基本屬性、戰鬥增益屬性、戰鬥方式、技能列等。
    /// </summary>
    public abstract class FightCharacter
    {
        public string Name = "佚失";
        public FightCharacter()
        {
        }
        //戰鬥前置屬性
        public abstract int FightRoundUnit { get; protected set; }
        public abstract int FightRoundPoint { get; set; }
        public abstract FightCharacter opponent { get; protected set; }
        public abstract Dictionary<string, ReleaseSkill> Skills { get; }
        public void SetOpponent(FightCharacter t)
        {
            opponent = t;
        }

        //戰鬥基本屬性... 基本上不能在戰鬥階段被更動。
        /// <summary>
        /// 血，<=0就結束
        /// </summary>
        public abstract int Hp { set; get; }
        /// <summary>
        /// 耐力(每移動一次要減少)
        /// </summary>
        public abstract float Stamina { set; get; }
        /// <summary>
        /// 速度(決定可連續幾回合)
        /// </summary>
        public abstract float Speed { set; get; }
        /// <summary>
        /// 劍
        /// </summary>
        public abstract Sword Sword { protected set; get; }
        /// <summary>
        /// 盾
        /// </summary>
        public abstract Shield Shield { protected set; get; }
        /// <summary>
        /// 攻擊力
        /// </summary>
        public abstract int Attack { set; get; }
        /// <summary>
        /// 閃避力
        /// </summary>
        public abstract float Avoid { set; get; }
        /// <summary>
        /// 擊暈力
        /// </summary>
        public abstract float Stun { set; get; }
        /// <summary>
        /// 格檔力
        /// </summary>
        public abstract float Collide { set; get; }
        /// <summary>
        /// 受傷
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public string GetHurt(int damage)
        {
            //閃避
            Random random = new Random();
            int p = random.Next(1, 1000);
            int damageAfterCount = 0;
            if (p < Avoid * 1000)
            {
                return $"{this.Name} 閃避了這次攻擊";
            }

            //格檔
            string block = "";
            if (Shield != null)
            {
                damageAfterCount = Shield.Defense(damageAfterCount);
                block = $"${this.Name}擋住了部分傷害，";
            }
            //傷害計算
            if (Shield != null)
                damageAfterCount = damage - Shield.DefensePoint;
            else
                damageAfterCount = damage;

            Hp -= damageAfterCount;
            if (damageAfterCount == 0) return block + "本次攻擊沒有造成任何傷害";

            return block + $"對 {this.Name} 造成 {damageAfterCount} 點 傷害";
        }

        //戰鬥增益屬性，因增益有作用回合數，所以之後要能變回原本狀態
        /// <summary>
        /// 攻擊力加成
        /// </summary>
        public float AttackBuff { set; get; }
        /// <summary>
        /// 防禦力加成
        /// </summary>
        public float DefenceBuff { set; get; }
        /// <summary>
        /// 速度加乘
        /// </summary>
        public float SpeedBuff { set; get; }
        /// <summary>
        /// 閃避率加成
        /// </summary>
        public float AvoidBuff { set; get; }
        /// <summary>
        /// 擊暈率加乘
        /// </summary>
        public float StunBuff { set; get; }
        /// <summary>
        /// 格擋發生率加乘
        /// </summary>
        public float CollideBuff { set; get; }
        /// <summary>
        /// 戰鬥內容(顯示於螢幕)
        /// </summary>
        public List<string> NowFightContext = new List<string>();
        /// <summary>
        /// 設定戰鬥屬性
        /// </summary>
        /// <param name="round"></param>
        internal abstract void SetState(int round);
        /// <summary>
        /// 每回合將增益狀態重設
        /// </summary>
        internal virtual void ResetState()
        {
            AttackBuff = SpeedBuff = 1;
            AvoidBuff = StunBuff = CollideBuff = 0;
        }
        /// <summary>
        /// 施加在角色身上的增益技能隊列
        /// </summary>
        protected Queue<(string, int, FightCharacter target)> NowBuffs { get; } = new Queue<(string, int, FightCharacter target)>();
        /// <summary>
        /// 設定本回合的增益技能影響
        /// </summary>
        public void SetBuff()
        {
            int buffNum = NowBuffs.Count;
            while (buffNum != 0)
            {
                (string sName, int round, var t) = NowBuffs.Dequeue();
                Skills[sName](t);
                int r = round - 1;
                if (r > 0)
                    NowBuffs.Enqueue((sName, r, t));
                buffNum--;
            }
        }


    }
}
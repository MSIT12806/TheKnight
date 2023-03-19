using Console2048;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFinalWork_TheKnight.Domain.Entities
{
    public abstract class Skill
    {
        public abstract Action<FightCharacter> GetEffect();
    }
    public class Fire : Skill
    {
        public override Action<FightCharacter> GetEffect()
        {
            return (FightCharacter f) =>f.Hp -= 10;
        }
    }
}

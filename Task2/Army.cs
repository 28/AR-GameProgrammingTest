using System;
using System.Collections.Generic;

namespace Task2
{
    public enum CombatModifier
    {
        Weak,
        Strong,
        None
    }

    public abstract class Unit
    {
        private static readonly Random Random = new Random();

        public int HitPoints { get; private set; }

        public bool Dead { get; set; }

        protected Unit(int hitPoints)
        {
            this.HitPoints = hitPoints;
        }

        public void DoDamage(Unit enemy)
        {
            var attackerCombatModifier = this.GetCombatModifierAgainst(enemy);
            var damage = GetBaseDamage();
            switch (attackerCombatModifier)
            {
                case CombatModifier.Strong:
                    damage += Random.Next(0, 5);
                    break;
                case CombatModifier.Weak:
                    damage -= Random.Next(0, 3);
                    break;
                case CombatModifier.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var enemyCombatModifier = enemy.GetCombatModifierAgainst(this);
            switch (enemyCombatModifier)
            {
                case CombatModifier.Strong:
                    damage -= Random.Next(0, 5);
                    break;
                case CombatModifier.Weak:
                    damage += Random.Next(0, 3);
                    break;
                case CombatModifier.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            enemy.TakeDamage(damage);
        }

        private void TakeDamage(int damage)
        {
            HitPoints -= damage;
            Dead |= HitPoints <= 0;
        }

        protected abstract int GetBaseDamage();

        protected abstract CombatModifier GetCombatModifierAgainst(Unit unit);
    }

    public class Warrior : Unit
    {
        public Warrior(int hitPoints) : base(hitPoints)
        {
        }

        protected override CombatModifier GetCombatModifierAgainst(Unit unit)
        {
            switch (unit)
            {
                case Archer _:
                    return CombatModifier.Weak;
                case Cavalry _:
                    return CombatModifier.Strong;
                default:
                    return CombatModifier.None;
            }
        }

        protected override int GetBaseDamage()
        {
            return 5;
        }

        public override string ToString()
        {
            return "Warrior (" + HitPoints + " hit points, dead: " + Dead + ")";
        }
    }

    public class Archer : Unit
    {
        public Archer(int hitPoints) : base(hitPoints)
        {
        }

        protected override CombatModifier GetCombatModifierAgainst(Unit unit)
        {
            switch (unit)
            {
                case Warrior _:
                    return CombatModifier.Strong;
                case Cavalry _:
                    return CombatModifier.Weak;
                default:
                    return CombatModifier.None;
            }
        }

        protected override int GetBaseDamage()
        {
            return 3;
        }

        public override string ToString()
        {
            return "Archer (" + HitPoints + " hit points, dead: " + Dead + ")";
        }
    }

    public class Cavalry : Unit
    {
        public Cavalry(int hitPoints) : base(hitPoints)
        {
        }

        protected override CombatModifier GetCombatModifierAgainst(Unit unit)
        {
            switch (unit)
            {
                case Warrior _:
                    return CombatModifier.Weak;
                case Archer _:
                    return CombatModifier.Strong;
                default:
                    return CombatModifier.None;
            }
        }

        protected override int GetBaseDamage()
        {
            return 7;
        }

        public override string ToString()
        {
            return "Cavalry (" + HitPoints + " hit points, dead: " + Dead + ")";
        }
    }

    public class Army
    {
        private static readonly Random Random = new Random();
        private List<Unit> Units { get; } = new List<Unit>();

        public Army()
        {
        }

        public Unit GetLiveUnit()
        {
            ShuffleUnits(); // add probability that a different unit is chosen each time
            return Units.Find((u) => !u.Dead);
        }

        public void AddUnit(Unit unit)
        {
            Units.Add(unit);
        }

        public bool Defeated()
        {
            return Units.TrueForAll((u) => u.Dead);
        }

        private void ShuffleUnits()
        {
            var n = Units.Count;
            while (n > 1)
            {
                n--;
                var k = Random.Next(n + 1);
                var value = Units[k];
                Units[k] = Units[n];
                Units[n] = value;
            }
        }

        public override string ToString()
        {
            return "Army: " + string.Join(",", Units.ConvertAll(u => u.ToString()));
        }
    }

    public static class Armies
    {
        private static readonly Random Random = new Random();

        public static void Main()
        {
            var army1 = new Army();
            army1.AddUnit(new Warrior(100));
            army1.AddUnit(new Archer(50));
            army1.AddUnit(new Cavalry(150));

            Console.WriteLine("Army 1 : " + army1);

            var army2 = new Army();
            army2.AddUnit(new Warrior(100));
            army2.AddUnit(new Archer(50));
            army2.AddUnit(new Cavalry(150));

            Console.WriteLine("Army 2 : " + army2);

            while (!army1.Defeated() && !army2.Defeated())
            {
                var unit1 = army1.GetLiveUnit();
                var unit2 = army2.GetLiveUnit();

                if (Random.Next(0, 1) == 0) // choose who attacks first
                {
                    unit1.DoDamage(unit2);
                    if (!unit2.Dead)
                        unit2.DoDamage(unit1);
                }
                else
                {
                    unit2.DoDamage(unit1);
                    if (!unit1.Dead)
                        unit1.DoDamage(unit2);
                }
            }

            if (army1.Defeated())
            {
                Console.WriteLine("Winner Army 2: " + army2);
                Console.WriteLine("Loser Army 1: " + army1);
            }
            else
            {
                Console.WriteLine("Winner Army 1: " + army1);
                Console.WriteLine("Loser Army 2: " + army2);
            }
        }
    }
}
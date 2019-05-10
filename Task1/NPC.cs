using System;
using System.Collections.Generic;

namespace Task1
{
    public enum Color
    {
        Black,
        Blond
    }

    public interface IJob
    {
        void Perform(NPC npc);
    }

    public class FarmerJob : IJob
    {
        public void Perform(NPC npc)
        {
            Console.WriteLine(npc.Name + " is farming.");
        }
    }

    public class LoggerJob : IJob
    {
        public void Perform(NPC npc)
        {
            Console.WriteLine(npc.Name + " is logging.");
        }
    }

    public class FishermanJob : IJob
    {
        public void Perform(NPC npc)
        {
            Console.WriteLine(npc.Name + " is fishing.");
        }
    }

    public class ScribeJob : IJob
    {
        public void Perform(NPC npc)
        {
            Console.WriteLine(npc.Name + " is scribing.");
        }
    }

    public class ArmourMerchantJob : IJob
    {
        public void Perform(NPC npc)
        {
            Console.WriteLine(npc.Name + " is buying/selling armour.");
        }
    }

    public class WeaponMerchantJob : IJob
    {
        public void Perform(NPC npc)
        {
            Console.WriteLine(npc.Name + " is buying/selling weapons.");
        }
    }

    public class MagicMerchantJob : IJob
    {
        public void Perform(NPC npc)
        {
            Console.WriteLine(npc.Name + " is buying/selling magic.");
        }
    }

    public class GuardJob : IJob
    {
        public void Perform(NPC npc)
        {
            Console.WriteLine(npc.Name + " is guarding.");
        }
    }

    public class InnkeeperJob : IJob
    {
        public void Perform(NPC npc)
        {
            Console.WriteLine(npc.Name + " is innkeeping.");
        }
    }

    public enum RelativeType
    {
        Brother,
        Sister,
        Mother,
        Father,
        Son,
        Daughter
    }

    public class Relative
    {
        private NPC Person { get; }
        private RelativeType RelativeType1 { get; }

        public Relative(RelativeType relativeType, NPC person)
        {
            RelativeType1 = relativeType;
            Person = person;
        }
    }

    public class NPC
    {
        public string Name { get; }
        public int Age { get; }
        public Color HairColor { get; }
        private IJob CurrentJob;
        private readonly List<Relative> _family;

        public NPC(string name, int age, Color hairColor)
        {
            _family = new List<Relative>();
            Name = name;
            Age = age;
            HairColor = hairColor;
        }

        public void AssignJob(IJob job)
        {
            CurrentJob = job;
        }

        public IJob GetCurrentJob()
        {
            return CurrentJob;
        }

        public void PerformWork()
        {
            CurrentJob?.Perform(this);
        }

        public void AssignRelative(Relative relative)
        {
            _family.Add(relative);
        }

        public void AssignRelatives(params Relative[] relatives)
        {
            _family.AddRange(relatives);
        }

        public List<Relative> GetFamily()
        {
            return _family;
        }
    }

    public static class NPCs
    {
        public static void Main(string[] args)
        {
            var father = new NPC("Father", 50, Color.Black);
            var mother = new NPC("Mother", 50, Color.Blond);
            var son = new NPC("Son", 25, Color.Black);

            father.AssignRelative(new Relative(RelativeType.Son, son));
            mother.AssignRelative(new Relative(RelativeType.Son, son));
            son.AssignRelatives(new Relative(RelativeType.Mother, mother), new Relative(RelativeType.Father, father));

            father.AssignJob(new FarmerJob());
            mother.AssignJob(new InnkeeperJob());
            son.AssignJob(new ScribeJob());

            father.PerformWork();
            mother.PerformWork();
            son.PerformWork();
        }
    }
}

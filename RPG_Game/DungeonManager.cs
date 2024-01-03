using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game
{
    //몬스터를 추가할 시 여기서 관리할 예정
    internal class DungeonManager
    {
        public DungeonManager() 
        {
            List<Dungeon>? d = (List<Dungeon>?)Utilities.LoadFile(LoadType.Map);
            dungeons = d;
        }
        private readonly List<Dungeon> dungeons = [];
        public int StageCount { get { return dungeons.Count; } }
        public int GetHp(int value) => dungeons[value].HP;
        public string GetName(int value) => dungeons[value].Name;
        public void ShowDungeon()
        {
            foreach(Dungeon dungeon in dungeons)
            {
                if(dungeon.DEF < 10 || dungeon.EXP < 100) 
                {
                    if (dungeon.DEF < 10 && dungeon.EXP > 99)
                    {
                        Console.WriteLine($"{dungeon.Stage}. {dungeon.Name} | " + "0" + $"{dungeon.DEF} | {dungeon.HP} | {dungeon.EXP} | {dungeon.Gold}");
                        continue;
                    }
                    if (dungeon.EXP < 100 && dungeon.DEF > 9)
                    {
                        Console.WriteLine($"{dungeon.Stage}. {dungeon.Name} | {dungeon.DEF} | {dungeon.HP} | " + "0" + $"{dungeon.EXP} | {dungeon.Gold}");
                        continue;
                    }
                    Console.WriteLine($"{dungeon.Stage}. {dungeon.Name} | "+"0"+$"{dungeon.DEF} | {dungeon.HP} | " + "0" + $"{dungeon.EXP} | {dungeon.Gold}");
                    continue;
                }

                Console.WriteLine($"{dungeon.Stage}. {dungeon.Name} | {dungeon.DEF} | {dungeon.HP} | {dungeon.EXP} | {dungeon.Gold}");
            }
        }

        public bool SelectDungeon(int stage, int def, int atk)
        {
            int _def = dungeons[stage].DEF - def;
            if (_def > 0)
            {
                int r = new Random().Next(0, 100);
                if (r <= 40)
                {
                    int hp = new Random().Next((dungeons[stage].HP - 5) + _def, dungeons[stage].HP + _def);
                    EventManager.Instance.PostEvent(EventType.eHealthChage, (hp / 2) * -1);
                    return false;
                }
                else
                {
                    int hp = new Random().Next((dungeons[stage].HP - 5) + _def, dungeons[stage].HP + _def);
                    int gold = new Random().Next(atk, atk * 2);
                    gold = (int)(dungeons[stage].Gold * (gold / 100f));
                    EventManager.Instance.PostEvent(EventType.eGoldChage, dungeons[stage].Gold + gold);
                    EventManager.Instance.PostEvent(EventType.eExpChage, dungeons[stage].EXP);
                    EventManager.Instance.PostEvent(EventType.eHealthChage, hp * -1);
                    return true;
                }
            }else
            {
                int hp = new Random().Next((dungeons[stage].HP - 5) + _def, dungeons[stage].HP + _def);
                int gold = new Random().Next(atk, atk * 2);
                gold = dungeons[stage].Gold * (gold / 100);
                EventManager.Instance.PostEvent(EventType.eGoldChage, dungeons[stage].Gold + gold);
                EventManager.Instance.PostEvent(EventType.eExpChage, dungeons[stage].EXP);
                EventManager.Instance.PostEvent(EventType.eHealthChage, hp * -1);
                return true;
            }
        }
    }

    class Dungeon
    {

        public Dungeon(string name, int stage, int def, int hp, int exp, int gold)
        {
            Name = name;
            Stage = stage;
            DEF = def;
            HP = hp;
            EXP = exp;
            Gold = gold;
        }

        public string Name { get; private set; }
        public int Stage { get; private set; }
        public int DEF { get; private set; }
        public int HP { get; private set; }
        public int EXP { get; private set; }
        public int Gold { get; private set; }
    }
}

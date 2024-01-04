using RPG_Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game
{
    internal class Player : IListener
    {
        private int health;
        private int gold;
        private int exp;
        private int level;
        private int armor;
        private int power;

        private float initATK;
        private int initDEF;
        private int itemATK = 0;
        private int itemDEF = 0;

        private string? Class { get; set; }
        public int Level { get { return exp / 100; } }
        public int EXP { get { return exp % 100; } private set { exp += value; } }
        public int DEF { get { return armor; } }
        public int ATK { get { return (int)power; } }
        public string? Name { get; private set; }
        public int Gold { get { return gold; }
            private set
            {
                gold = Math.Clamp(value, 0, 999999999);
            }
        }
        public int Health { get { return health; }
            private set
            {
                health = Math.Clamp(value, 0, 100);
            }
            
        }

        public Player()
        {
            EventManager.Instance.AddListener(EventType.eGameInit, this);
            EventManager.Instance.AddListener(EventType.eGameEnd, this);
            EventManager.Instance.AddListener(EventType.eHealthChage, this);
            EventManager.Instance.AddListener(EventType.eGoldChage, this);
            EventManager.Instance.AddListener(EventType.eItemChage, this);
            EventManager.Instance.AddListener(EventType.eExpChage, this);
        }

        public void Init(string name,string job, int? _gold = null, int? _health = null, int? _exp = null)
        {
            if (_health > 100)
                _health = 100;

            Name = name;
            Class = job;
            gold = _gold ?? 1500;
            health = _health ?? 100;
            exp = _exp ?? 100;
            level = 1;

            initATK = 1;
            initDEF = 0;
            LevelCheck();
            power = power > 1 ? power + (int)initATK : (int)initATK;
            armor = armor > 1 ? armor + initDEF :  initDEF;
        }

        public void LevelCheck()
        {
            int result = Level - level;
            if (level != Level)
            {
                initATK += 0.5f * result;
                initDEF += 1 * result;
                level = Level;
            }
            else
                level = Level;

        }

        public void ShowStat()
        {
            Console.Write($"Lv : {Level}");
            Console.WriteLine($"\tEXP : {EXP}");
            Console.WriteLine($"{Name}  ( {Class} )");
            Console.WriteLine($"공격력 : {power}");
            Console.WriteLine($"방어력 : {armor}");
            Console.WriteLine($"체력 : {Health}");
            Console.WriteLine($"Gold : {Gold}G");
        }

        void IListener.OnEvent(EventType type, object data)
        {
            switch (type)
            {
                case EventType.eHealthChage:
                    {
                        Health += (int)data;
                        break;
                    }
                case EventType.eGameInit:
                    {
                        PlayerState? state = (PlayerState?)Utilities.LoadFile(LoadType.Player);
                        if (state != null)
                            Init(state.Value.name, "전사", state.Value.gold, state.Value.health, state.Value.exp);

                        break;
                    }
                case EventType.eGameEnd:
                    {
                        PlayerState playerState = new PlayerState
                        {
                            name = Name,
                            job = Class,
                            gold = Gold,
                            health = Health,
                            exp = exp
                        };
                        Utilities.SaveFile(SaveType.Player, playerState);
                        break;
                    }
                case EventType.eGoldChage:
                    {
                        Gold += (int)data;
                        break;
                    }
                case EventType.eItemChage:
                    {
                        List<Item> items = [];
                        items = (List<Item>)data;
                        if (items != null)
                        {
                            itemATK = 0;
                            itemDEF = 0;
                            foreach (Item item in items)
                            {
                                itemATK += item.Power;
                                itemDEF += item.Armor;
                            }
                        }
                        power = (int)initATK + itemATK;
                        armor = initDEF + itemDEF;

                        break;
                    }
                case EventType.eExpChage:
                    {
                        LevelCheck();
                        EXP = (int)data;
                        LevelCheck();
                        EventManager.Instance.PostEvent(EventType.eItemChage);

                        break;
                    }
                default:
                    {
                        Console.WriteLine("Player OnEvent Type Error!");
                        break;
                    }
            }
        }
    }
}

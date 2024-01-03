using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace RPG_Game
{
    enum MapType
    {
        Stats,
        Inventory,
        Shop,
        Rest,
        Dungeon,
        NONE
    }

    internal class Map
    {
        private Player player;
        private Inventory inventory;
        private ItemManager itemManager;
        private Shop shop;
        private DungeonManager dungeonManager;
        private MapType mapType = MapType.NONE;

        public Map()
        {
            player = new Player();
            inventory = new Inventory();
            itemManager = new ItemManager();
            shop = new Shop(itemManager.GetItems());
            dungeonManager = new DungeonManager();

            EventManager.Instance.PostEvent(EventType.eGameInit, "스파르타");
        }

        public void DrawMap()
        {
            switch (mapType)
            {
                case MapType.Stats:
                    {
                        ShowStats();
                        break;
                    }
                case MapType.Inventory:
                    {
                        ShowInventory();
                        break;
                    }
                case MapType.Shop:
                    {
                        ShowShop();
                        break;
                    }
                case MapType.Rest:
                    {
                        ShowRest();
                        break;
                    }
                case MapType.Dungeon:
                    {
                        ShowDungeon();
                        break;
                    }
                default:
                    {
                        ShowTown();
                        break;
                    }
            }
        }

        string CreateName()
        {
            while (true)
            {
                Console.WriteLine("RPG_GAME에 오신것을 환영합니다!");
                Console.WriteLine("이름을 정해주세요!");
                Console.WriteLine();
                Console.WriteLine("당신의 이름은? [※주의 : 띄워쓰기 금지 / 10글자 이내]");
                Console.Write(">>");
                string? str = Console.ReadLine();
                bool isCheck = Regex.IsMatch(str, @"[^a-zA-Z0-9가-힣]");
                if (str != null && str.Length <= 10 && isCheck == false)
                {
                    return str;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 이름입니다!");
                    Console.WriteLine("===================================================");
                }
            }
        }

        string CreateJob()
        {
            while (true)
            {
                Console.WriteLine("RPG_GAME에 오신것을 환영합니다!");
                Console.WriteLine("직업을 정해주세요!");
                Console.WriteLine();
                Console.WriteLine("1. 전사");
                Console.WriteLine("2. 마법사");
                Console.WriteLine("3. 궁수");
                Console.WriteLine("4. 도적");
                Console.WriteLine("당신의 직업은?");
                Console.Write(">>");
                string? str = Console.ReadLine();
                
                if (str != null && int.TryParse(str, out int a))
                {
                    int type = int.Parse(str);
                    if(type > 0 && type < 5)
                    {
                        switch(type)
                        {
                            case 1: return "전사";
                            case 2: return "마법사";
                            case 3: return "궁수";
                            case 4: return "도적";
                        }
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 이름입니다!");
                        Console.WriteLine("===================================================");
                    }    
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 이름입니다!");
                    Console.WriteLine("===================================================");
                }
            }
            return "";
        }

        public bool CreatePlayer()
        {
            if (player.Name != null)
                return true;

            while (true)
            {
                Console.WriteLine("RPG_GAME에 오신것을 환영합니다!");
                Console.WriteLine("이곳은 캐릭터 생성을 하는 곳 입니다.");
                Console.WriteLine();
                Console.WriteLine("1. 캐릭터 생성");
                Console.WriteLine();
                Console.WriteLine("0. 종료");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요");
                Console.Write(">>");
                string? str = Console.ReadLine();
                if (str != null && int.TryParse(str, out int a))
                {
                    int type = int.Parse(str);
                    if (type == 0)
                    {
                        Console.Clear();
                        return false;
                    }
                    else if (type == 1)
                    {
                        Console.Clear();
                        string name = CreateName();
                        Console.Clear();
                        string job = CreateJob();
                        player.Init(name, job);
                        Console.Clear();
                        return true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다!");
                        Console.WriteLine("===================================================");
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다!");
                    Console.WriteLine("===================================================");
                }
            }
        }

        void ShowStats()
        {
            while (true)
            {
                player.ShowStat();
                Console.WriteLine();
                Console.WriteLine("0.나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요");
                Console.Write(">>");
                string? str = Console.ReadLine();
                if (str != null && int.TryParse(str, out int a))
                {
                    int type = int.Parse(str);
                    if (type == 0)
                    {
                        mapType = MapType.NONE;
                        Console.Clear();
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다!");
                        Console.WriteLine("===================================================");
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다!");
                    Console.WriteLine("===================================================");
                }
            }
        }

        void ShowInventory()
        {
            while (true)
            {
                Console.WriteLine("인벤토리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine();
                inventory.ShowInventory();
                Console.WriteLine();
                Console.WriteLine("1. 장착 관리");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요");
                Console.Write(">>");

                string? str = Console.ReadLine();
                if (str != null && int.TryParse(str, out int a))
                {
                    int type = int.Parse(str);
                    if (type == 1)
                    {
                        Console.Clear();
                        while (true)
                        {
                            Console.WriteLine("인벤토리");
                            Console.WriteLine("보유 중인 아이템을 장착할 수 있습니다.");
                            Console.WriteLine();
                            inventory.ShowEquipped();
                            Console.WriteLine();
                            Console.WriteLine("0. 나가기");
                            Console.WriteLine();
                            Console.WriteLine("장착할 아이템을 입력해주세요");
                            Console.Write(">>");
                            str = Console.ReadLine();
                            if (str != null && int.TryParse(str, out int b))
                            {
                                type = int.Parse(str);
                                //장착관리 시작
                                if (type > 0 && type < inventory.ItemsCount + 1)
                                {
                                    inventory.ItemEquip(type - 1);
                                    Console.Clear();
                                    continue;
                                }
                                else if (type == 0)
                                {
                                    Console.Clear();
                                    break;
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("잘못된 입력입니다!");
                                    Console.WriteLine("===================================================");
                                }
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("잘못된 입력입니다!");
                                Console.WriteLine("===================================================");
                            }
                        }
                    }
                    else if (type == 0)
                    {
                        Console.Clear();
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다!");
                        Console.WriteLine("===================================================");
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다!");
                    Console.WriteLine("===================================================");
                }
            }
        }

        void ShowShop()
        {
            while (true)
            {
                Console.WriteLine("상점 - 아이템 구매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine();
                Console.WriteLine($"[보유골드]\n{player.Gold} G\n");
                shop.ShowShop();
                Console.WriteLine();
                Console.WriteLine("1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요");
                Console.Write(">>");
                string? str = Console.ReadLine();
                if (str != null && int.TryParse(str, out int b))
                {
                    int type = int.Parse(str);
                    if (type == 0)
                    {
                        Console.Clear();
                        break;
                    }
                    else if (type == 1)
                    {
                        Console.Clear();
                        while (true)
                        {
                            Console.WriteLine("상점 - 아이템 구매");
                            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                            Console.WriteLine();
                            Console.WriteLine($"[보유골드]\n{player.Gold} G\n");
                            shop.ShowSellingItems();
                            Console.WriteLine();
                            Console.WriteLine("0. 나가기");
                            Console.WriteLine();
                            Console.WriteLine("구매할 아이템을 입력해주세요");
                            Console.Write(">>");
                            str = Console.ReadLine();
                            if (str != null && int.TryParse(str, out int a))
                            {
                                type = int.Parse(str);
                                if (type > 0 && type < itemManager.GetItems().Length + 1)
                                {
                                    Item item = shop.BuyItem(type - 1, player.Gold);

                                    if (item != null)
                                    {
                                        inventory.AddItem(item);
                                        Console.Clear();
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("금액이 모자라거나 구매한 상품입니다!");
                                        Console.WriteLine("===================================================");
                                        continue;
                                    }
                                }
                                else if (type == 0)
                                {
                                    Console.Clear();
                                    break;
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("잘못된 입력입니다!");
                                    Console.WriteLine("===================================================");
                                }
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("잘못된 입력입니다!");
                                Console.WriteLine("===================================================");
                            }
                        }
                    }
                    else if (type == 2)
                    {
                        Console.Clear();
                        while (true)
                        {
                            Console.WriteLine("상점 - 아이템 판매");
                            Console.WriteLine("필요 없는 아이템을 팔 수 있는 상점입니다.");
                            Console.WriteLine();
                            Console.WriteLine($"[보유골드]\n{player.Gold} G\n");
                            inventory.ShowEquipped(true);
                            Console.WriteLine();
                            Console.WriteLine("0. 나가기");
                            Console.WriteLine();
                            Console.WriteLine("판매할 아이템을 입력해주세요");
                            Console.Write(">>");
                            str = Console.ReadLine();
                            if (str != null && int.TryParse(str, out int a))
                            {
                                type = int.Parse(str);
                                if (type > 0 && type < itemManager.GetItems().Length + 1)
                                {
                                    Console.Clear();
                                    shop.SaleItem(inventory.SaleItem(type - 1));
                                    continue;
                                }
                                else if (type == 0)
                                {
                                    Console.Clear();
                                    break;
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("잘못된 입력입니다!");
                                    Console.WriteLine("===================================================");
                                }
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("잘못된 입력입니다!");
                                Console.WriteLine("===================================================");
                            }
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다!");
                        Console.WriteLine("===================================================");
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다!");
                    Console.WriteLine("===================================================");
                }
            }
        }

        void ShowRest()
        {
            while (true)
            {
                Console.WriteLine("휴식터에 오신것을 환영합니다!");
                Console.WriteLine();
                Console.WriteLine($"[HP]\n{player.Health} G");
                Console.WriteLine($"[보유골드]\n{player.Gold} G\n");
                Console.WriteLine();
                Console.WriteLine("1.휴식하기(200G)");
                Console.WriteLine("0.나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요");
                Console.Write(">>");
                string? str = Console.ReadLine();
                if (str != null && int.TryParse(str, out int a))
                {
                    int type = int.Parse(str);
                    if (type == 0)
                    {
                        mapType = MapType.NONE;
                        Console.Clear();
                        break;
                    }
                    else if (type == 1)
                    {
                        if (player.Gold > 200 && player.Health <= 99)
                        {
                            Console.Clear();
                            EventManager.Instance.PostEvent(EventType.eGoldChage, -200);
                            EventManager.Instance.PostEvent(EventType.eHealthChage, +10);
                            continue;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("금액이 모자라거나 회복할 HP가 없습니다!");
                            Console.WriteLine("===================================================");
                            continue;
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다!");
                        Console.WriteLine("===================================================");
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다!");
                    Console.WriteLine("===================================================");
                }
            }
        }

        void ShowDungeon()
        {
            while (true)
            {
                Console.WriteLine("던전에 오신것을 환영합니다!");
                Console.WriteLine();
                player.ShowStat();
                Console.WriteLine();
                Console.WriteLine("[던전 목록]");
                Console.WriteLine("\tName   | DEF| HP | EXP | GOLD");
                dungeonManager.ShowDungeon();
                Console.WriteLine("0.나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요");
                Console.Write(">>");
                string? str = Console.ReadLine();
                if (str != null && int.TryParse(str, out int a))
                {
                    int type = int.Parse(str);
                    if (type == 0)
                    {
                        mapType = MapType.NONE;
                        Console.Clear();
                        break;
                    }
                    else if (type > 0 && type < dungeonManager.StageCount + 1)
                    {
                        if (dungeonManager.GetHp(type - 1) > player.Health)
                        {
                            Console.Clear();
                            Console.WriteLine("HP가 부족합니다!");
                            Console.WriteLine("===================================================");
                            continue;
                        }
                        if (dungeonManager.SelectDungeon(type - 1, player.DEF, player.ATK))
                        {
                            Console.Clear();
                            Console.WriteLine("던전 클리어!");
                            Console.WriteLine("===================================================");
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("던전 클리어 실패..");
                            Console.WriteLine("===================================================");
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다!");
                        Console.WriteLine("===================================================");
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다!");
                    Console.WriteLine("===================================================");
                }
            }
        }

        void ShowTown()
        {
            while (true)
            {
                Console.WriteLine("스파르타 마을에 오신것을 환영합니다!");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 휴식");
                Console.WriteLine("5. 던전");
                Console.WriteLine();
                Console.WriteLine("0. 종료 및 저장");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요");
                Console.Write(">>");

                string? str = Console.ReadLine();
                if (str != null && int.TryParse(str, out int a))
                {
                    int type = int.Parse(str);
                    if (type > 0 && type < 6)
                    {
                        mapType = (MapType)(type - 1);
                        Console.Clear();
                        DrawMap();
                    }
                    else if (type == 0)
                    {
                        Console.Clear();
                        EventManager.Instance.PostEvent(EventType.eGameEnd);
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다!");
                        Console.WriteLine("===================================================");
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다!");
                    Console.WriteLine("===================================================");
                }
            }
        }


    }
}

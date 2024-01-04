using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game
{
    internal class Shop : IListener
    {
        private Item[] ShopItems;
        private List<Item> SaleItems = [];

        public Shop(Item[] shopItems)
        {
            ShopItems = shopItems;
            EventManager.Instance.AddListener(EventType.eGameInit, this);
            EventManager.Instance.AddListener(EventType.eGameEnd, this);
        }

        public void ShowShop()
        {
            Console.WriteLine("[아이템 목록]");
            foreach (Item item in ShopItems)
            {
                Console.Write("- ");

                if(SaleItems.Count == 0)
                {
                    Console.WriteLine($"{item.Name}{(item.Power > 0 ? " | 공격력 +" + (item.Power < 10 ? " " + item.Power : item.Power) : "")}" +
                        $"{(item.Armor > 0 ? " | 방어력 +" + (item.Armor < 10 ? " " + item.Armor : item.Armor) : "")} | {item.Description} | {item.Cost}G");
                }else
                {
                    Item? i = SaleItems.Find(x => x.Name == item.Name);
                    if (i != null)
                    {
                        Console.WriteLine($"{item.Name}{(item.Power > 0 ? " | 공격력 +" + (item.Power < 10 ? " " + item.Power : item.Power) : "")}" +
                            $"{(item.Armor > 0 ? " | 방어력 +" + (item.Armor < 10 ? " " + item.Armor : item.Armor) : "")} | {item.Description} | 구매 완료");
                    }
                    else
                    {
                        Console.WriteLine($"{item.Name}{(item.Power > 0 ? " | 공격력 +" + (item.Power < 10 ? " " + item.Power : item.Power) : "")}" +
                            $"{(item.Armor > 0 ? " | 방어력 +" + (item.Armor < 10 ? " " + item.Armor : item.Armor) : "")} | {item.Description} | {item.Cost}G");
                    }
                }
            }
        }

        public void ShowSellingItems()
        {
            Console.WriteLine("[아이템 목록]");
            int count = 0;

            foreach (Item item in ShopItems)
            {
                count++;
                Console.Write($"- {count} ");

                if (SaleItems.Count == 0)
                {
                    Console.WriteLine($"{item.Name}{(item.Power > 0 ? " | 공격력 +" + (item.Power < 10 ? " " + item.Power : item.Power) : "")}" +
                        $"{(item.Armor > 0 ? " | 방어력 +" + (item.Armor < 10 ? " " + item.Armor : item.Armor) : "")} | {item.Description} | {item.Cost}G");
                }
                else
                {
                    Item? i = SaleItems.Find(x => x.Name == item.Name);
                    if (i != null)
                    {
                        Console.WriteLine($"{item.Name}{(item.Power > 0 ? " | 공격력 +" + (item.Power < 10 ? " " + item.Power : item.Power) : "")}" +
                            $"{(item.Armor > 0 ? " | 방어력 +" + (item.Armor < 10 ? " " + item.Armor : item.Armor) : "")} | {item.Description} | 구매 완료");
                    }
                    else
                    {
                        Console.WriteLine($"{item.Name}{(item.Power > 0 ? " | 공격력 +" + (item.Power < 10 ? " " + item.Power : item.Power) : "")}" +
                            $"{(item.Armor > 0 ? " | 방어력 +" + (item.Armor < 10 ? " " + item.Armor : item.Armor) : "")} | {item.Description} | {item.Cost}G");
                    }
                }
            }

        }

        public Item BuyItem(int number, int maxMoney)
        {
            if (maxMoney < ShopItems[number].Cost)
                return null;
            foreach (Item item in SaleItems)
            {
                if (ShopItems[number].Name == item.Name)
                    return null;
            }

            SaleItems.Add(ShopItems[number]);
            EventManager.Instance.PostEvent(EventType.eGoldChage, ShopItems[number].Cost * -1);
            return ShopItems[number];
        }

        public void SaleItem(Item item)
        {
            if (item == null)
            {
                Console.WriteLine($"장착중인 아이템 입니다!");
                Console.WriteLine("=========================================================================");
                return;
            }
            Console.WriteLine($"아이템 {item.Name}을 팔아 돈 {(int)(item.Cost * (85 / 100f))}G를 얻었습니다!");
            Console.WriteLine("=========================================================================");
            SaleItems.Remove(item);
        }

        public void OnEvent(EventType type, object data)
        {
            if(type == EventType.eGameInit)
            {
                List<Item>? items = (List<Item>?)Utilities.LoadFile(LoadType.Shop);
                if (items != null)
                    SaleItems = items;

            }else if(type == EventType.eGameEnd)
            {
                if (SaleItems.Count > 0)
                    Utilities.SaveFile(SaveType.Shop, SaleItems);
            }
        }
    }
}


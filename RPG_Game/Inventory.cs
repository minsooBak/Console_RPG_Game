using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game
{
    internal class Inventory : IListener
    {
        private List<Item> items;
        private List<Item> eItems;

        public int ItemsCount { get { return items.Count; } }

        public Inventory() 
        {
            items = [];
            eItems = [];

            EventManager.Instance.AddListener(EventType.eGameInit, this);
            EventManager.Instance.AddListener(EventType.eGameEnd, this);
        }

        public void AddItem(Item item)
        {
            items.Add(item);
        }

        public Item SaleItem(int number)
        {
            Item item = items[number];
            if (items[number].isEquipped == false)
            {
                items.Remove(item);
                int result = (int)(item.Cost * (85 / 100f));
                EventManager.Instance.PostEvent(EventType.eGoldChage, result);
                return item;
            }
            
            Debug.WriteLine("Invectory Class : The item is equipped and cannot be sold");
            return null;
        }

        public void ItemEquip(int number)
        {
            if (number > items.Count)
                return;

            Item? result = items[number];
            if(result.isEquipped)
            {
                ItemUnEquip(result);
                return;
            }
            Item? result1 = eItems.Find(value => value.Type == result.Type);
            if (result1 != null)
            {
                result1.isEquipped = false;
                eItems.Remove(result1);
            }
            result.isEquipped = true;
            eItems.Add(result);
            EventManager.Instance.PostEvent(EventType.eItemChage, eItems);
        }

        public void ItemUnEquip(Item item)
        {
            if (item.isEquipped == true)
            {
                eItems.Remove(item);
                item.isEquipped = false;
                EventManager.Instance.PostEvent(EventType.eItemChage, eItems);
                return;
            }

            Debug.WriteLine("Invectory Class : The item you are trying to equip is not in your inventory");
        }

        public void ShowInventory()
        {
            Console.WriteLine("[아이템 목록]");
            if (items.Count <= 0)
            {
                Console.WriteLine("아이템 없음");
                return;
            }

            foreach(Item item in items)
            {
                if(item.isEquipped == true)
                {
                    Console.Write("[E] ");
                }
                if(item.Type == ItemType.Weapon) 
                {
                    Console.WriteLine($"{item.Name} | 공격력 +{item.Power} | {item.Description}");
                }else
                {
                    Console.WriteLine($"{item.Name} | 방어력 +{item.Armor} | {item.Description}");
                }
            }
        }

        public void ShowEquipped(bool isSale = false)
        {
            Console.WriteLine("[아이템 목록]");
            if (items.Count <= 0)
            {
                Console.WriteLine("아이템 없음");
                return;
            }

            int count = 0;
            foreach (Item item in items)
            {
                count++;
                if (item.isEquipped == true)
                    Console.Write($"{count}. [E] ");
                else
                    Console.Write($"{count}. ");

                if(isSale)
                {
                    if (item.Type == ItemType.Weapon)
                    {
                        Console.WriteLine($"{item.Name} | 공격력 +{item.Power} | {item.Description} | {item.Cost}G");
                    }
                    else
                    {
                        Console.WriteLine($"{item.Name} | 방어력 +{item.Armor} | {item.Description} | {item.Cost}G");
                    }
                }else
                {
                    if (item.Type == ItemType.Weapon)
                    {
                        Console.WriteLine($"{item.Name} | 공격력 +{item.Power} | {item.Description}");
                    }
                    else
                    {
                        Console.WriteLine($"{item.Name} | 방어력 +{item.Armor} | {item.Description}");
                    }
                }
                
            }
        }

        public void OnEvent(EventType type, object data)
        {
            if (type == EventType.eGameInit)
            {
                List<Item>? items = (List<Item>?)Utilities.LoadFile(DataType.Inventory);
                if (items != null)
                {
                    this.items = items;
                    foreach (Item item in items)
                        if (item.isEquipped)
                            eItems.Add(item);

                    EventManager.Instance.PostEvent(EventType.eItemChage, eItems);
                }
            }
            else if (type == EventType.eGameEnd)
            {
                if (items.Count > 0)
                    Utilities.SaveFile(DataType.Inventory, items);
            }

        }
    }
}

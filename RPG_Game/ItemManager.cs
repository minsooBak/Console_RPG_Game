using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game
{
    enum ItemType
    {
        Weapon,
        Head,
        Armor
    }

    struct ItemData
    {
        public string[] inventory;
        public string[] equippedItem;
        public string[] saleItem;
    }

    internal class ItemManager : IListener
    {
        private readonly Item[] items;
        private readonly Item[] shopItem;
        private List<Item> saleItem;
        private List<Item> inventory;

        public int GetInventorySize { get { return inventory.Count; } }
        public int GetShopSize { get { return shopItem.Length; } }

        public ItemManager()
        {
            List<Item>? list = (List<Item>?)Utilities.LoadFile(LoadType.Item);
            items = list.ToArray();

            ItemData? data = (ItemData?)Utilities.LoadFile(LoadType.Inventory);
            inventory = [];
            saleItem = [];
            if(data != null)
            {
                foreach(string item in data.Value.inventory)
                {
                    Item _item = list.Find(x => x.Name == item);
                    
                    inventory.Add(_item);
                }
                foreach (string item in data.Value.saleItem)
                {
                    Item _item = list.Find(x => x.Name == item);
                    saleItem.Add(_item);
                }

                foreach(string item in data.Value.equippedItem)
                {
                    Item _item = list.Find(x=> x.Name == item);
                    _item.IsEquipped = true;
                }
            }

            shopItem = list.FindAll(x => x.ForSale == true).ToArray();

            List<Item>? lItem = list.FindAll(x => x.IsEquipped);
            EventManager.Instance.PostEvent(EventType.eItemChage, lItem);


            EventManager.Instance.AddListener(EventType.eGameEnd, this);
            EventManager.Instance.AddListener(EventType.eItemGet, this);
        }

        public void SaleItem(int number)
        {
            Item item = inventory[number];
            if (item.IsEquipped == false)
            {
                inventory.Remove(item);
                Item? sale = saleItem.Find(x => x.Name == item.Name);
                int result = (int)(item.Cost * (85 / 100f));
                if (sale != null)
                    saleItem.Remove(sale);

                Console.WriteLine($"아이템 {item.Name}을 팔아 돈 {result}G를 얻었습니다!");
                Console.WriteLine("=========================================================================");
                EventManager.Instance.PostEvent(EventType.eGoldChage, result);
            }else
            {
                Console.WriteLine($"장착중인 아이템 입니다!");
                Console.WriteLine("=========================================================================");
            }
        }

        public void ItemEquip(int number)
        {
            Item item = inventory[number];
            if(item.IsEquipped)
            {
                item.IsEquipped = false;
                return;
            }
            Item? item1 = inventory.Find(x => x.Type == item.Type && x.IsEquipped);
            if (item1 != null)
            {
                item1.IsEquipped = false;
            }
            item.IsEquipped = true;
            List<Item> invenItems = inventory.FindAll(x => x.IsEquipped);
            EventManager.Instance.PostEvent(EventType.eItemChage, invenItems);
        }

        public void ShowInventory(bool isEquipped = false, bool isSale = false)
        {
            Console.WriteLine("[아이템 목록]");
            if (inventory.Count <= 0)
            {
                Console.WriteLine("아이템 없음");
                return;
            }

            int count = 0;
            foreach (Item item in inventory)
            {
                if (isSale || isEquipped)
                {
                    count++;
                    if (item.IsEquipped == true)
                        Console.Write($"{count}. [E] ");
                    else
                        Console.Write($"{count}. ");
                    if (isEquipped == false)
                        Console.WriteLine($"{item.Name}{(item.Power > 0 ? " | 공격력 +" + (item.Power < 10 ? " " + item.Power : item.Power) : "")}" +
                            $"{(item.Armor > 0 ? " | 방어력 +" + (item.Armor < 10 ? " " + item.Armor : item.Armor) : "")} | {item.Description} | {item.Cost}G");
                    else
                        Console.WriteLine($"{item.Name}{(item.Power > 0 ? " | 공격력 +" + (item.Power < 10 ? " " + item.Power : item.Power) : "")}" +
                            $"{(item.Armor > 0 ? " | 방어력 +" + (item.Armor < 10 ? " " + item.Armor : item.Armor) : "")} | {item.Description}");
                    continue; 
                }
                else if (item.IsEquipped == true)
                {
                    Console.Write("[E] ");
                }
                Console.WriteLine($"{item.Name}{(item.Power > 0 ? " | 공격력 +" + (item.Power < 10 ? " " + item.Power : item.Power) : "")}" +
                         $"{(item.Armor > 0 ? " | 방어력 +" + (item.Armor < 10 ? " " + item.Armor : item.Armor) : "")} | {item.Description}");
            }
        }

        public void ShowShop(bool isSeal = false)
        {
            Console.WriteLine("[아이템 목록]");
            int count = 0;
            foreach (Item item in shopItem)
            {
                if (isSeal == false)
                    Console.Write("- ");
                else
                {
                    count++;
                    Console.Write($"- {count} ");
                }

                if (saleItem.Count == 0)
                {
                    Console.WriteLine($"{item.Name}{(item.Power > 0 ? " | 공격력 +" + (item.Power < 10 ? " " + item.Power : item.Power) : "")}" +
                        $"{(item.Armor > 0 ? " | 방어력 +" + (item.Armor < 10 ? " " + item.Armor : item.Armor) : "")} | {item.Description} | {item.Cost}G");
                }
                else
                {
                    Item? i = saleItem.Find(x => x.Name == item.Name);
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

        public bool BuyItem(int number, int maxMoney)
        {
            if (maxMoney < shopItem[number].Cost)
                return false;
            Item? item = saleItem.Find(x => x.Name == shopItem[number].Name);
            if (item != null)
                return false;

            item = shopItem[number];
            saleItem.Add(item);
            EventManager.Instance.PostEvent(EventType.eGoldChage, item.Cost * -1);
            inventory.Add(item);
            return true;
        }

        public void OnEvent(EventType type, object data)
        {
            if(type == EventType.eGameEnd)
            {
                List<string> sInventory = [];
                List<string> sSale = [];
                List<string> sEquip = [];
                List<Item> _item = items.ToList().FindAll(x => x.IsEquipped);
                foreach (Item inven in inventory)
                    sInventory.Add(inven.Name);
                foreach (Item sale in saleItem)
                    sSale.Add(sale.Name);
                foreach (Item equip in _item)
                    sEquip.Add(equip.Name);


                ItemData item = new()
                {
                    inventory = sInventory.ToArray(),
                    saleItem = sSale.ToArray(),
                    equippedItem = sEquip.ToArray()
                };

                Utilities.SaveFile(SaveType.ItemData, item);
            }
            else
            {
                //던전에서 아이템 드랍 시
                inventory.Add((Item)data);
            }
        }
    }

    class Item
    {
        public string Name { get; private set; }
        public int Armor { get; private set; }
        public int Power { get; private set; }
        public ItemType Type { get; private set; }
        public string Description {  get; private set; }
        public bool IsEquipped { get; set; }
        public bool ForSale { get; private set; }
        public int Cost { get; private set; }
        public Item(string name, int armor, int power, ItemType type, string description, int cost, bool forSale)
        {
            Name = name;
            Armor = armor;
            Power = power;
            Type = type;
            Description = description;
            Cost = cost;
            ForSale = forSale;
        }
    }
}

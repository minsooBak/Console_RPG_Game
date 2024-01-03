using System;
using System.Collections.Generic;
using System.Linq;
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
    internal class ItemManager
    {
        private readonly Item[] _items;
        public ItemManager()
        {
            List<Item>? list = (List<Item>?)Utilities.LoadFile(DataType.Item);

            _items = list.ToArray();
        }

        public Item? GetItem(string itemName)
        {
            foreach(Item item in _items)
            {
                if(item.Name == itemName) return item;
            }

            return null;
        }

        public Item[] GetItems()
        {
            return _items;
        }
    }

    class Item
    {
        public string Name { get; private set; }
        public int Armor { get; private set; }
        public int Power { get; private set; }
        public ItemType Type { get; private set; }
        public string Description {  get; private set; }
        public bool isEquipped { get; set; }
        public int Cost { get; private set; }
        public Item(string name, int armor, int power, ItemType type, string description, int cost)
        {
            Name = name;
            Armor = armor;
            Power = power;
            Type = type;
            Description = description;
            Cost = cost;
        }
    }
}

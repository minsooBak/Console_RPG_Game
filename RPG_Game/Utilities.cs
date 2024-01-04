using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace RPG_Game
{
    enum SaveType
    {
        NONE,
        Player,
        ItemData
    }
    enum LoadType
    {
        NONE,
        Player,
        Map,
        Item,
        Inventory,
        Shop
    }
    struct ObjectState
    {
        public string Name { get; set; }
        public string Class {  get; set; }
        public int Health { get; set; }
        public int Gold { get; set; }
        public int Level { get { return EXP / 100; } }
        public int EXP { get; set; }
        public float InitATK { get; set; }
        public int InitDEF { get; set; }
        public int ATK { get; set; }
        public int DEF { get; set; }
    }

    internal static class Utilities
    {
        public static void TextColor(string str, ConsoleColor color1, ConsoleColor color2)
        {
            Console.ForegroundColor = color1;
            Console.WriteLine(str);
            Console.ForegroundColor = color2;
        }

        public static object? LoadFile(LoadType type)
        {
            switch (type)
            {
                case LoadType.Player:
                    {
                        string? path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\P_Data.json";
                        if (File.Exists(path) == false)
                            return null;
                        StreamReader? file = File.OpenText(path);
                        if (file != null)
                        {
                            JsonTextReader reader = new JsonTextReader(file);

                            JObject json = (JObject)JToken.ReadFrom(reader);

                            ObjectState state = new()
                            {
                                Name = json["Name"].ToString(),
                                Class = json["Job"].ToString(),
                                Gold = (int)json["Gold"],
                                Health = (int)json["Health"],
                                EXP = (int)json["EXP"]
                            };

                            file.Close();
                            return state;
                        }
                        break;
                    }
                case LoadType.Map:
                    {
                        string? path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Map_Data.json";

                        if (File.Exists(path) == false)
                            return null;
                        StreamReader? file = File.OpenText(path);
                        if (file != null)
                        {
                            JsonTextReader reader = new JsonTextReader(file);

                            JArray json = (JArray)JToken.ReadFrom(reader);
                            string? str = JsonConvert.SerializeObject(json);
                            file.Close();
                            return JsonConvert.DeserializeObject<List<Dungeon>>(str);
                        }

                        break;
                    }
                case LoadType.Item:
                    {
                        string? path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Item_Data.json";
                        if (File.Exists(path) == false)
                            return null;
                        StreamReader? file = File.OpenText(path);
                        if (file != null)
                        {
                            JsonTextReader reader = new JsonTextReader(file);

                            JArray json = (JArray)JToken.ReadFrom(reader);
                            string? str = JsonConvert.SerializeObject(json);
                            file.Close();
                            return JsonConvert.DeserializeObject<List<Item>>(str);

                        }

                        return null;
                    }
                case LoadType.Shop:
                case LoadType.Inventory:
                    {
                        string? path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\I_Data.json";
                        if (File.Exists(path) == false)
                            return null;
                        StreamReader? file = File.OpenText(path);
                        if (file != null)
                        {
                            JsonTextReader reader = new JsonTextReader(file);

                            JObject json = (JObject)JToken.ReadFrom(reader);
                            string? str = JsonConvert.SerializeObject(json);
                            file.Close();
                            return JsonConvert.DeserializeObject<ItemData>(str);

                        }
                        return null;
                    }
            }
            return null;
        }

        public static void SaveFile(SaveType dataType, object data)
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "";

            switch (dataType)
            {
                case SaveType.Player:
                    {
                        path += @"\P_Data.json";
                        ObjectState state = (ObjectState)data;
                        JObject configData = new JObject
                            (
                            new JProperty("Name", state.Name),
                            new JProperty("Job", state.Class),
                            new JProperty("Gold", state.Gold),
                            new JProperty("Health", state.Health),
                            new JProperty("EXP", state.EXP)
                            );
                        File.WriteAllText(path, JsonConvert.SerializeObject(configData));
                        break;
                    }
                case SaveType.ItemData:
                    {
                        path += @"\I_Data.json";
                        string json = JsonConvert.SerializeObject((ItemData)data, Formatting.Indented);
                        File.WriteAllText(path, json);
                        break;
                    }
            }
        }
    }
}

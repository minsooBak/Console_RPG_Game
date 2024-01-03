using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace RPG_Game
{
    enum SaveType
    {
        NONE,
        Player,
        Inventory,
        Shop
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
    struct PlayerState
    {
        public string name;
        public string job;
        public int gold;
        public int health;
        public int exp;
    }

    internal static class Utilities
    {
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

                            PlayerState state = new();
                            
                            state.name = json["Name"].ToString();
                            state.job = json["Job"].ToString();
                            state.gold = (int)json["Gold"];
                            state.health = (int)json["Health"];
                            state.exp = (int)json["EXP"];

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
                case LoadType.Inventory:
                case LoadType.Shop:
                    {
                        string? path;
                        if (type == LoadType.Shop)
                            path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\S_Data.json";
                        else if (type == LoadType.Inventory)
                            path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\I_Data.json";
                        else
                            path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Item_Data.json";
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
                        PlayerState state = (PlayerState)data;
                        JObject configData = new JObject
                            (
                            new JProperty("Name", state.name),
                            new JProperty("Job", state.job),
                            new JProperty("Gold", state.gold),
                            new JProperty("Health", state.health),
                            new JProperty("EXP", state.exp)
                            );
                        File.WriteAllText(path, JsonConvert.SerializeObject(configData));
                        break;
                    }
                case SaveType.Inventory:
                    {
                        path += @"\I_Data.json";
                        string json = JsonConvert.SerializeObject((List<Item>)data, Formatting.Indented);
                        File.WriteAllText(path, json);
                        break;
                    }
                case SaveType.Shop:
                    {
                        path += @"\S_Data.json";
                        string json = JsonConvert.SerializeObject((List<Item>)data, Formatting.Indented);
                        File.WriteAllText(path, json);
                        break;
                    }
            }
        }
    }
}

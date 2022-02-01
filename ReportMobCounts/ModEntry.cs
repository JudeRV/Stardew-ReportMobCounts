using System.Linq;
using System.Collections.Generic;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Monsters;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework;

namespace ReportMobCounts
{
    class ModEntry : Mod
    {
        ModConfig Config;
        public override void Entry(IModHelper helper)
        {
            Config = this.Helper.ReadConfig<ModConfig>();
            Helper.Events.Player.Warped += OnPlayerWarped;
        }

        private void OnPlayerWarped(object sender, WarpedEventArgs e)
        {
            Dictionary<string, int> monsterTypes = new();
            foreach (Monster monster in e.NewLocation.characters.OfType<Monster>())
            {
                if (monsterTypes.ContainsKey(monster.Name))
                {
                    monsterTypes[monster.Name]++;
                }
                else monsterTypes.Add(monster.Name, 1);
            }
            foreach (KeyValuePair<string, int> kvp in new Dictionary<string, int>(monsterTypes))
            {
                if (kvp.Key == "Sludge")
                {
                    int value = kvp.Value;
                    monsterTypes.Remove("Sludge");
                    if (e.NewLocation is MineShaft ms)
                    {
                        if (ms.mineLevel < 120)
                        {
                            monsterTypes.Add("Red Slime", value);
                        }
                        else monsterTypes.Add("Purple Slime", value);
                    }
                }
                if (kvp.Key == "Green Slime")
                {
                    int value = kvp.Value;
                    monsterTypes.Remove("Green Slime");
                    monsterTypes.Add("Slime", value);
                }
            }
            foreach (KeyValuePair<string, int> kvp in monsterTypes)
            {
                PrintInGame($"{kvp.Value} {kvp.Key}{(kvp.Value > 1 ? "s" : "")} detected!");
            }
            if (monsterTypes.Count > 0) PrintInGame("-----");
        }

        private void PrintInGame(string msg)
        {
            if (Config.PrintReportsToInGameChat) Game1.chatBox.addMessage(msg, Color.White);
            else Monitor.Log(msg, LogLevel.Debug);
        }
    }

    class ModConfig
    {
        public bool PrintReportsToInGameChat { get; set; } = true;
    }
}

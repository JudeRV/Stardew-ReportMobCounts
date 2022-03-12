using System;
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
            bool? isQuarryArea = null;
            try
            {
                isQuarryArea = Helper.Reflection.GetProperty<bool>(e.NewLocation as MineShaft, "isQuarryArea").GetValue();
            }   
            catch (ArgumentNullException) { }
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
                        if (isQuarryArea ?? false)
                        {
                            monsterTypes.Add("Slime", value);
                        }
                        else if (ms.mineLevel < 120)
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
                if (kvp.Key == "Prismatic Slime")
                {
                    PrintInGame($"{kvp.Value} Prismatic Slime detected!", Color.Purple);
                    monsterTypes.Remove("Prismatic Slime");
                    Game1.playSound("newRecord");
                }
            }
            foreach (KeyValuePair<string, int> kvp in monsterTypes)
            {
                PrintInGame($"{kvp.Value} {kvp.Key}{(kvp.Value > 1 ? "s" : "")} detected!");
            }
            if (monsterTypes.Count > 0 && !Config.PrintReportsToInGameHud) PrintInGame("-----");
        }

        private void PrintInGame(string msg, Color? color = null)
        {
            if (Config.PrintReportsToInGameChat) Game1.chatBox.addMessage(msg, color ?? Color.White);
            if(Config.PrintReportsToInGameHud)
            {
                HUDMessage message = new HUDMessage(message: msg, color: color ?? Color.Black, timeLeft: Config.InGameHudTimeOnScreen, fadeIn: true);
                Game1.addHUDMessage(message);
            }
            else Monitor.Log(msg, LogLevel.Debug);
        }
    }

    class ModConfig
    {
        public bool PrintReportsToInGameChat { get; set; } = false;
        public bool PrintReportsToInGameHud { get; set; } = true;
        public float InGameHudTimeOnScreen { get; set; } = 1100f;
    }
}

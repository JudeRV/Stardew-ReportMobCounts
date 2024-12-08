using GenericModConfigMenu;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using MobCountReports;


namespace ReportMobCounts
{
    internal class ModEntry : Mod
    {
        ModConfig? Config;

        KeybindList? toggleKey;
        bool displayNotifications;
        int notificationLength;
        bool displayDelimiter;

        bool printInConsole;
        bool printInChat;

        bool canPrintToggleMessage = true;
        public override void Entry(IModHelper helper)
        {
            Config = Helper.ReadConfig<ModConfig>();
            toggleKey = Config.DisplayReportButton;
            displayNotifications = Config.DisplayReportsAsNotifications;
            notificationLength = Config.HowLongToDisplayNotificationsInMilliseconds;
            printInConsole = Config.PrintReportsToConsole;
            printInChat = Config.PrintReportsToInGameChat;
            displayDelimiter = Config.WhetherToDisplayFloorDelimiterNotification;

            Helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            Helper.Events.Player.Warped += OnPlayerWarped;
            Helper.Events.Input.ButtonsChanged += OnButtonsChanged;
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
            {
                return;
            }

            configMenu.Register(
                mod: this.ModManifest,
                reset: () => this.Config = new ModConfig(),
                save: () => this.Helper.WriteConfig(this.Config)
                );

            configMenu.AddKeybindList(
                mod: this.ModManifest,
                name: () => "Notifications Toggle Key",
                tooltip: () => "When the set keybind is pressed in-game, it will toggle whether or not notifications are displayed in the bottom left for reporting mob counts.",
                getValue: () => toggleKey,
                setValue: value => toggleKey = value
                );

            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => "Display Notifications",
                tooltip: () => "Whether or not the notifications in the bottom right should be displayed when reporting mob counts. Toggleable with the keybind above this option",
                getValue: () => displayNotifications,
                setValue: value => Config.DisplayReportsAsNotifications = displayNotifications = value
                );

            configMenu.AddNumberOption(
                mod: ModManifest,
                name: () => "Notification Length (ms)",
                tooltip: () => "How many milliseconds each notification for mob count reports should be displayed. For reference, 1 second = 1000 milliseconds",
                getValue: () => notificationLength,
                setValue: value => Config.HowLongToDisplayNotificationsInMilliseconds = notificationLength = value
                );

            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => "Display Notification Delimiter",
                tooltip: () => "Whether or not to add '-----' between each floor's set of notifications to separate them",
                getValue: () => displayDelimiter,
                setValue: value => Config.WhetherToDisplayFloorDelimiterNotification = displayDelimiter = value
                );

            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => "Print Reports to In-Game Chat",
                tooltip: () => "Whether or not reports for mob counts should be printed as messages in the in-game chat box",
                getValue: () => printInChat,
                setValue: value => Config.PrintReportsToInGameChat = printInChat = value
                );

            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => "Print Reports to Console",
                tooltip: () => "Whether or not reports for mob counts should be printed in the SMAPI console",
                getValue: () => printInConsole,
                setValue: value => Config.PrintReportsToConsole = printInConsole = value
                );
        }

        private void OnButtonsChanged(object? sender, ButtonsChangedEventArgs e)
        {
            if (toggleKey != null && toggleKey.JustPressed())
            {
                displayNotifications = !displayNotifications;
                Game1.addHUDMessage(new HUDMessage($"{(displayNotifications ? "Enabling" : "Disabling")} mob count notifications!", notificationLength));
            }
            else if (toggleKey == null && canPrintToggleMessage)
            {
                Monitor.Log("DisplayReportButton option in config is invalid. Please read the Nexus page for instructions on valid values for this option.", LogLevel.Error);
                canPrintToggleMessage = false;
            }
        }

        private void OnPlayerWarped(object? sender, WarpedEventArgs e)
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
                else
                {
                    monsterTypes.Add(monster.Name, 1);
                }
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
                            // monsterTypes.Add("Slime", value);
                            for (int i = 1; i <= value; i++)
                            {
                                Game1.addHUDMessage(MobReportHUDMessage.NewMonster("Slime")); // THESE ARE ALL 1 MORE THAN THEY SHOULD BE >>>:(
                            }
                        }
                        else if (ms.mineLevel < 120)
                        {
                            // monsterTypes.Add("Red Slime", value);
                            for (int i = 1; i <= value; i++)
                            {
                                Game1.addHUDMessage(MobReportHUDMessage.NewMonster("Red Slime"));
                            }                            
                        }
                        else
                        {
                            // monsterTypes.Add("Purple Slime", value);
                            for (int i = 1; i <= value; i++)
                            {
                                Game1.addHUDMessage(MobReportHUDMessage.NewMonster("Purple Slime"));
                            }
                        }
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
                    Game1.playSound("newRecord");
                }
            }
            foreach (KeyValuePair<string, int> kvp in monsterTypes)
            {
                for (int i = 0; i <= kvp.Value; i++)
                {
                    Game1.addHUDMessage(MobReportHUDMessage.NewMonster(kvp.Key)); // This works!!! LFG!!!!
                }
                // PrintInGame($"{kvp.Value} {kvp.Key}{(kvp.Value > 1 ? "s" : "")} detected!");
            }
            if (displayDelimiter && monsterTypes.Count > 0)
            {
                PrintInGame("-----");
            }
        }

        private void PrintInGame(string msg, Color? color = null)
        {
            if (printInChat)
            {
                Game1.chatBox.addMessage(msg, color ?? Color.White);
            }
            if (printInConsole)
            {
                Monitor.Log(msg, LogLevel.Info);
            }
            if (displayNotifications)
            {
                Game1.addHUDMessage(new HUDMessage(msg, notificationLength));                
            }
        }
    }

    internal class ModConfig
    {
        public KeybindList DisplayReportButton { get; set; } = KeybindList.Parse("Y");
        public bool DisplayReportsAsNotifications { get; set; } = true;
        public int HowLongToDisplayNotificationsInMilliseconds { get; set; } = 5000;
        public bool WhetherToDisplayFloorDelimiterNotification { get; set; } = false;
        public bool PrintReportsToInGameChat { get; set; } = false;
        public bool PrintReportsToConsole { get; set; } = false;
    }
}
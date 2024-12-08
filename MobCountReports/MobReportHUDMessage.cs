using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Extensions;
using StardewValley.Menus;
using StardewValley.Monsters;

namespace MobCountReports
{
    class MobReportHUDMessage : HUDMessage
    {
        static string spriteSheetHeader = "Characters\\Monsters\\";
        // OK HERE'S THE FUCKIN DEAL
        // Define a dictionary for every mob to their respective sprite files
        // Define another one for their rectangles (offset + dimensions) in the sprite files
        // Alter the draw() function and use a 
        public static Dictionary<string, string> monsterSpriteSheets = new()
        {
            { "Slime", spriteSheetHeader + "Green Slime" },
            { "Bug", spriteSheetHeader + "Bug" },
            { "Grub", spriteSheetHeader + "Grub" },
            { "Fly", spriteSheetHeader + "Fly" },
            { "Duggy", spriteSheetHeader + "Duggy" },
            { "Rock Crab", spriteSheetHeader + "Rock Crab" },
            { "Stone Golem", spriteSheetHeader + "Stone Golem" },
            { "Frost Jelly", spriteSheetHeader + "Green Slime" },
            { "Dust Spirit", spriteSheetHeader + "Dust Spirit" },
            { "Frost Bat", spriteSheetHeader + "Frost Bat" },
            { "Skeleton", spriteSheetHeader + "Skeleton" },
            { "Red Slime", spriteSheetHeader + "Green Slime" },
            { "Metal Head", spriteSheetHeader + "Metal Head" },
            { "Shadow Brute", spriteSheetHeader + "Shadow Brute" },
            { "Shadow Shaman", spriteSheetHeader + "Shadow Shaman" },
            { "Squid Kid", spriteSheetHeader + "Squid Kid" },
            { "Bat", spriteSheetHeader + "Bat" },
            { "Ghost", spriteSheetHeader + "Ghost" },
            { "Lava Bat", spriteSheetHeader + "Lava Bat" },
            { "Haunted Skull", spriteSheetHeader + "Haunted Skull" },
            { "Hot Head", spriteSheetHeader + "Hot Head" },
            { "Iridium Bat", spriteSheetHeader + "Iridium Bat" },
            { "Iridium Crab", spriteSheetHeader + "Iridium Crab" },
            { "Lava Crab", spriteSheetHeader + "Lava Crab" },
            { "Lava Lurk", spriteSheetHeader + "Lava Lurk" },
            { "Magma Duggy", spriteSheetHeader + "Magma Duggy" },
            { "Magma Sparker", spriteSheetHeader + "Magma Sparker" },
            { "Magma Sprite", spriteSheetHeader + "Magma Sprite" },
            { "Mummy", spriteSheetHeader + "Mummy" },
            { "Purple Slime", spriteSheetHeader + "Green Slime" },
            { "Pepper Rex", spriteSheetHeader + "Pepper Rex" },
            { "Putrid Ghost", spriteSheetHeader + "Putrid Ghost" },
            { "Serpent", spriteSheetHeader + "Serpent" },
            { "Royal Serpent", spriteSheetHeader + "Royal Serpent" },
            { "Shadow Sniper", spriteSheetHeader + "Shadow Sniper" },
            { "Skeleton Mage", spriteSheetHeader + "Skeleton Mage" },
            { "Carbon Ghost", spriteSheetHeader + "Carbon Ghost" },
            { "Spider", spriteSheetHeader + "Spider" },
            { "Spiker", spriteSheetHeader + "Spiker" },
            { "Stick Bug", spriteSheetHeader + "Stick Bug" },
            { "Tiger Slime", spriteSheetHeader + "Tiger Slime" },
            { "Wilderness Golem", spriteSheetHeader + "Wilderness Golem" },
            { "Blue Squid", spriteSheetHeader + "Blue Squid" },
            { "Big Slime", spriteSheetHeader + "Big Slime" },
            { "Dwarvish Sentry", spriteSheetHeader + "Dwarvish Sentry" },
            { "Prismatic Slime", spriteSheetHeader + "Green Slime" },
            { "False Magma Cap", spriteSheetHeader + "False Magma Cap" }
        };

        public static Dictionary<string, Rectangle> monsterSpriteLocations = new()
        {
            { "Slime", new Rectangle(1, 265, 14, 14) },
            { "Bug", new Rectangle(0, 0, 16, 16) },
            { "Grub", new Rectangle(0, 34, 16, 16) },
            { "Fly", new Rectangle(0, 9, 16, 16) },
            { "Duggy", new Rectangle(48, 32, 16, 16) },
            { "Rock Crab", new Rectangle(16, 5, 16, 20) },
            { "Stone Golem", new Rectangle(1, 1, 15, 23) },
            { "Frost Jelly", new Rectangle(16, 264, 16, 16) },
            { "Dust Spirit", new Rectangle(1, 8, 16, 16) },
            { "Frost Bat", new Rectangle(0, 8, 16, 16) },
            { "Skeleton", new Rectangle(1, 3, 15, 28) },
            { "Red Slime", new Rectangle(32, 264, 16, 16) },
            { "Metal Head", new Rectangle(0, 0, 15, 17) },
            { "Shadow Brute", new Rectangle(0, 4, 16, 30) },
            { "Shadow Shaman", new Rectangle(0, 0, 16, 24) },
            { "Squid Kid", new Rectangle(0, 0, 16, 16) },
            { "Bat", new Rectangle(0, 8, 16, 16) },
            { "Ghost", new Rectangle(0, 0, 16, 22) },
            { "Lava Bat", new Rectangle(0, 8, 16, 16) },
            { "Haunted Skull", new Rectangle(0, 0, 16, 16) },
            { "Hot Head", new Rectangle(0, 0, 16, 16) },
            { "Iridium Bat", new Rectangle(0, 6, 16, 16) },
            { "Iridium Crab", new Rectangle(16, 5, 16, 19) },
            { "Lava Crab", new Rectangle(16, 6, 16, 18) },
            { "Lava Lurk", new Rectangle(16, 16, 16, 16) },
            { "Magma Duggy", new Rectangle(1, 32, 15, 16) },
            { "Magma Sparker", new Rectangle(66, 33, 14, 15) },
            { "Magma Sprite", new Rectangle(66, 33, 14, 15) },
            { "Mummy", new Rectangle(1, 5, 14, 26) },
            { "Purple Slime", new Rectangle(0, 34, 16, 15) },
            { "Pepper Rex", new Rectangle(4, 1, 24, 28) },
            { "Putrid Ghost", new Rectangle(2, 1, 13, 20) },
            { "Serpent", new Rectangle(0, 0, 32, 32) },
            { "Royal Serpent", new Rectangle(0, 0, 32, 32) },
            { "Shadow Sniper", new Rectangle(8, 2, 16, 31) },
            { "Skeleton Mage", new Rectangle(0, 2, 17, 31) },
            { "Carbon Ghost", new Rectangle(0, 1, 16, 20) },
            { "Spider", new Rectangle(8, 16, 16, 16) },
            { "Spiker", new Rectangle(0, 0, 16, 16) },
            { "Stick Bug", new Rectangle(16, 6, 16, 18) },
            { "Tiger Slime", new Rectangle(0, 11, 16, 13) },
            { "Wilderness Golem", new Rectangle(1, 0, 15, 24) },
            { "Blue Squid", new Rectangle(4, 3, 16, 16) },
            { "Big Slime", new Rectangle(64, 3, 32, 29) },
            { "Dwarvish Sentry", new Rectangle(0, 0, 16, 16) },
            { "Prismatic Slime", new Rectangle(0, 34, 16, 15) },
            { "False Magma Cap", new Rectangle(16, 2, 16, 23) }
        };

        // Assign values in constructors
        Texture2D? spriteFile;
        Rectangle? spriteLocation;

        /// <summary>Construct an instance with the default time and an empty icon.</summary>
        /// <param name="message">The message text to show.</param>
        public MobReportHUDMessage(string message) : base(message)
        {
        }

        /// <summary>Construct an instance with the given values.</summary>
        /// <param name="message">The message text to show.</param>
        /// <param name="timeLeft">The duration in milliseconds for which to show the message.</param>
        /// <param name="fadeIn">Whether the message should start transparent and fade in.</param>
        public MobReportHUDMessage(string message, float timeLeft, bool fadeIn = false) : base(message, timeLeft, fadeIn)
        {
        }

        /// <summary>Construct a message indicating an item received.</summary>
        /// <param name="monster"></param>
        public static MobReportHUDMessage NewMonster(string monster)
        {
            return new MobReportHUDMessage(monster)
            {
                number = 1,
                type = monster,
                spriteFile = Game1.content.Load<Texture2D>(monsterSpriteSheets[monster]),
                spriteLocation = monsterSpriteLocations[monster]
            };
        }

        public override void draw(SpriteBatch b, int i, ref int heightUsed)
        {
            Rectangle tsarea = Game1.graphics.GraphicsDevice.Viewport.GetTitleSafeArea();
            if (this.noIcon)
            {
                int overrideX = tsarea.Left + 16;
                int height2 = (int)Game1.dialogueFont.MeasureString(this.message).Y + 64;
                int overrideY = ((Game1.uiViewport.Width < 1400) ? (-64) : 0) + tsarea.Bottom - height2 - heightUsed - 64;
                heightUsed += height2;
                IClickableMenu.drawHoverText(b, this.message, Game1.dialogueFont, 0, 0, -1, null, -1, null, null, 0, null, -1, overrideX, overrideY, this.transparency);
                return;
            }
            int height = 112;
            Vector2 itemBoxPosition = new Vector2(tsarea.Left + 16, tsarea.Bottom - height - heightUsed - 64);
            heightUsed += height;
            if (Game1.isOutdoorMapSmallerThanViewport())
            {
                itemBoxPosition.X = Math.Max(tsarea.Left + 16, -Game1.uiViewport.X + 16);
            }
            if (Game1.uiViewport.Width < 1400)
            {
                itemBoxPosition.Y -= 48f;
            }
            // Draws the actual item frame and message background -- no touchy >>>:(
            b.Draw(Game1.mouseCursors, itemBoxPosition, (this.messageSubject is StardewValley.Object obj && obj.sellToStorePrice(-1L) > 500) ? new Rectangle(163, 399, 26, 24) : new Rectangle(293, 360, 26, 24), Color.White * this.transparency, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
            float messageWidth = Game1.smallFont.MeasureString(this.message ?? "").X;
            b.Draw(Game1.mouseCursors, new Vector2(itemBoxPosition.X + 104f, itemBoxPosition.Y), new Rectangle(319, 360, 1, 24), Color.White * this.transparency, 0f, Vector2.Zero, new Vector2(messageWidth, 4f), SpriteEffects.None, 1f);
            b.Draw(Game1.mouseCursors, new Vector2(itemBoxPosition.X + 104f + messageWidth, itemBoxPosition.Y), new Rectangle(323, 360, 6, 24), Color.White * this.transparency, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
            itemBoxPosition.X += 16f;
            itemBoxPosition.Y += 16f;
            if (this.spriteLocation != null)
            {
                float xOffset = 8f / (this.spriteLocation.Value.Width / 16f);
                float yOffset = 8f / (this.spriteLocation.Value.Height / 16f);
                Vector2 offset = new Vector2(xOffset, yOffset) * 4f;
                if (this.type == "Purple Slime") // Needs to be drawn purple manually & with eyes
                {
                    b.Draw(this.spriteFile, itemBoxPosition + offset, this.spriteLocation, new Color(142, 68, 173) * this.transparency, 0f, new Vector2(8f, 8f), (4f / (Math.Max(this.spriteLocation.Value.Width, this.spriteLocation.Value.Height) / 16f) + Math.Max(0f, (this.timeLeft - 3000f) / 900f)), SpriteEffects.None, 1f);
                    b.Draw(this.spriteFile, itemBoxPosition + offset, new Rectangle(32, 131, 16, 15), Color.White * this.transparency, 0f, new Vector2(8f, 8f), (4f / (Math.Max(this.spriteLocation.Value.Width, this.spriteLocation.Value.Height) / 16f) + Math.Max(0f, (this.timeLeft - 3000f) / 900f)), SpriteEffects.None, 1f);
                }
                else if (this.type == "Big Slime") // Just draw green for now. Kinda too lazy to check area for color but I might fix this later
                {
                    b.Draw(this.spriteFile, itemBoxPosition + offset, this.spriteLocation, new Color(167, 226, 46) * this.transparency, 0f, new Vector2(8f, 8f), (4f / (Math.Max(this.spriteLocation.Value.Width, this.spriteLocation.Value.Height) / 16f) + Math.Max(0f, (this.timeLeft - 3000f) / 900f)), SpriteEffects.None, 1f);
                }
                else if (this.type == "Prismatic Slime") // idk how the frick to do the prismatic thing, so just make bro pink
                {
                    b.Draw(this.spriteFile, itemBoxPosition + offset, this.spriteLocation, Color.LightPink * this.transparency, 0f, new Vector2(8f, 8f), (4f / (Math.Max(this.spriteLocation.Value.Width, this.spriteLocation.Value.Height) / 16f) + Math.Max(0f, (this.timeLeft - 3000f) / 900f)), SpriteEffects.None, 1f);
                    b.Draw(this.spriteFile, itemBoxPosition + offset, new Rectangle(32, 131, 16, 15), Color.White * this.transparency, 0f, new Vector2(8f, 8f), (4f / (Math.Max(this.spriteLocation.Value.Width, this.spriteLocation.Value.Height) / 16f) + Math.Max(0f, (this.timeLeft - 3000f) / 900f)), SpriteEffects.None, 1f);
                }
                else
                {
                    b.Draw(this.spriteFile, itemBoxPosition + offset, this.spriteLocation, Color.White * this.transparency, 0f, new Vector2(8f, 8f), (4f / (Math.Max(this.spriteLocation.Value.Width, this.spriteLocation.Value.Height) / 16f) + Math.Max(0f, (this.timeLeft - 3000f) / 900f)), SpriteEffects.None, 1f);
                    if (this.type == "Tiger Slime") // Add eyes for tiger slime
                    {
                        b.Draw(this.spriteFile, itemBoxPosition + offset, new Rectangle(32, 132, 16, 13), Color.White * this.transparency, 0f, new Vector2(8f, 8f), (4f / (Math.Max(this.spriteLocation.Value.Width, this.spriteLocation.Value.Height) / 16f) + Math.Max(0f, (this.timeLeft - 3000f) / 900f)), SpriteEffects.None, 1f);
                    }
                }
            }
            itemBoxPosition.X += 51f;
            itemBoxPosition.Y += 51f;
            if (this.number > 1)
            {
                Utility.drawTinyDigits(this.number, b, itemBoxPosition, 5f, 1f, Color.White * this.transparency);
            }
            itemBoxPosition.X += 32f;
            itemBoxPosition.Y -= 33f;
            Utility.drawTextWithShadow(b, this.message ?? "", Game1.smallFont, itemBoxPosition, Game1.textColor * this.transparency, 1f, 1f, -1, -1, this.transparency);
        }
    }
}

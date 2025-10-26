using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Microsoft.Xna.Framework;
using Luminance.Assets;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Reflection;
using Terraria.ModLoader;
using System.Collections;

namespace CSE.Core
{

    public static class CSEUtils
    {
        public static void ChangeBossProgressions(params (int npcID, float newProgression)[] changes)
        {
            if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
            {
                // get access to bossTracker
                object bossTracker = bossChecklist.GetType()
                    .GetField("bossTracker", BindingFlags.NonPublic | BindingFlags.Static)
                    .GetValue(null);

                // get entries list
                FieldInfo sortedEntriesField = bossTracker.GetType()
                    .GetField("SortedEntries", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                IList entries = (IList)sortedEntriesField.GetValue(bossTracker);

                // prepare for reflection
                FieldInfo npcIDsField = null;
                FieldInfo progressionField = null;
                var entriesToChange = new List<(object entry, float newProg)>();

                // find all entries
                foreach (object entry in entries)
                {
                    if (npcIDsField == null)
                    {
                        npcIDsField = entry.GetType().GetField("npcIDs",
                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                        progressionField = entry.GetType().GetField("progression",
                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                        if (npcIDsField == null || progressionField == null)
                            throw new InvalidOperationException("Required fields was not found.");
                    }

                    List<int> currentNPCIDs = (List<int>)npcIDsField.GetValue(entry);
                    foreach (var change in changes)
                    {
                        if (currentNPCIDs.Contains(change.npcID))
                        {
                            entriesToChange.Add((entry, change.newProgression));
                            break;
                        }
                    }
                }

                // apply edits
                foreach (var change in entriesToChange)
                {
                    progressionField.SetValue(change.entry, change.newProg);
                }

                // re-sort
                List<object> sortedList = new List<object>();
                foreach (object entry in entries)
                    sortedList.Add(entry);

                sortedList.Sort((x, y) =>
                {
                    float xProg = (float)progressionField.GetValue(x);
                    float yProg = (float)progressionField.GetValue(y);
                    return xProg.CompareTo(yProg);
                });

                // update original list
                entries.Clear();
                foreach (object entry in sortedList)
                    entries.Add(entry);
            }
        }
        public static void CreateCrateRecipe(int result, int crate, int crateAmount, int hardmodeCrate, int extraItem = -1, params Condition[] conditions)
        {
            if (crate != -1)
            {
                var recipe = Recipe.Create(result);
                recipe.AddIngredient(crate, crateAmount);
                if (extraItem != -1)
                {
                    recipe.AddIngredient(extraItem);
                }
                recipe.AddTile(TileID.WorkBenches);
                foreach (Condition condition in conditions)
                {
                    recipe.AddCondition(condition);
                }
                recipe.DisableDecraft();
                recipe.Register();
            }

            if (hardmodeCrate != -1)
            {
                var recipe = Recipe.Create(result);
                recipe.AddIngredient(hardmodeCrate, crateAmount);
                if (extraItem != -1)
                {
                    recipe.AddIngredient(extraItem);
                }
                recipe.AddTile(TileID.WorkBenches);
                foreach (Condition condition in conditions)
                {
                    recipe.AddCondition(condition);
                }
                recipe.DisableDecraft();
                recipe.Register();
            }
        }        
        public static float PlayerGetDistanceToNPC(Player player, NPC targetNPC)
        {
            if (targetNPC == null || !targetNPC.active)
            {
                return 99999f;
            }

            Vector2 playerPosition = player.Center;
            Vector2 npcPosition = targetNPC.Center;

            float distance = Vector2.Distance(playerPosition, npcPosition);

            return distance;
        }
        public static LazyAsset<Texture2D> LoadDeferred(string path)
        {
            if (Main.netMode == NetmodeID.Server)
                return default;

            return LazyAsset<Texture2D>.Request(path, AssetRequestMode.ImmediateLoad);
        }
        public static int GetPlayerCount()
        {
            return Main.player.Count(p => p.active);
        }
        public static void RemoveItem(int type)
        {
            for (int j = 0; j < Main.player[Main.myPlayer].inventory.Length; j++)
            {

                if (Main.player[Main.myPlayer].inventory[j].type == type)
                {
                    int index = Main.LocalPlayer.FindItem(type);
                    Main.LocalPlayer.inventory[index].TurnToAir();
                }
            }
        }
        public static Point FindTileOrigin(int x, int y)
        {
            int originX = x;
            int originY = y;
            int maxSteps = 20;

            for (int i = 0; i < maxSteps; i++)
            {
                Tile tile = Main.tile[originX, originY];
                if (tile == null || !tile.HasTile) break;

                bool moved = false;

                if (tile.TileFrameX != 0)
                {
                    originX--;
                    moved = true;
                }

                if (tile.TileFrameY != 0)
                {
                    originY--;
                    moved = true;
                }

                if (!moved) break;
            }

            return new Point(originX, originY);
        }
        public static string GetItemInternalName(Item item)
        {
            if (item.ModItem != null)
            {
                return $"{item.ModItem.Mod.Name}/{item.ModItem.Name}";
            }
            return ItemID.Search.GetName(item.type);
        }
        public static float ProjGetDistanceToNPC(Projectile player, NPC targetNPC)
        {
            if (targetNPC == null || !targetNPC.active)
            {
                return 99999f;
            }
            Vector2 playerPosition = player.Center;
            Vector2 npcPosition = targetNPC.Center;

            float distance = Vector2.Distance(playerPosition, npcPosition);

            return distance;
        }
        public static Point? FindNearestMultitile(Vector2 searchPosition, int tileType, int maxDistance = 100)
        {
            Point startTile = searchPosition.ToTileCoordinates();
            Point? nearestStart = null;
            float minDistanceSq = float.MaxValue;

            int left = Math.Max(0, startTile.X - maxDistance);
            int right = Math.Min(Main.maxTilesX - 1, startTile.X + maxDistance);
            int top = Math.Max(0, startTile.Y - maxDistance);
            int bottom = Math.Min(Main.maxTilesY - 1, startTile.Y + maxDistance);

            HashSet<Point> processedStarts = new HashSet<Point>();

            for (int x = left; x <= right; x++)
            {
                for (int y = top; y <= bottom; y++)
                {
                    Tile tile = Main.tile[x, y];
                    if (tile == null || !tile.HasTile || tile.TileType != tileType)
                        continue;

                    Point start = FindTileOrigin(x, y);

                    if (!processedStarts.Add(start))
                        continue;

                    Tile originTile = Main.tile[start.X, start.Y];
                    if (originTile == null || !originTile.HasTile || originTile.TileType != tileType)
                        continue;

                    Vector2 startWorldPos = new Vector2(start.X * 16f + 8f, start.Y * 16f + 8f);
                    float distanceSq = Vector2.DistanceSquared(searchPosition, startWorldPos);

                    if (distanceSq < minDistanceSq)
                    {
                        minDistanceSq = distanceSq;
                        nearestStart = start;
                    }
                }
            }
            return nearestStart;
        }
        public static void AddToNextEmptySlot(ref int startIndex, Item[] items, int itemType, int price)
        {
            for (int i = startIndex; i < items.Length; i++)
            {
                if (items[i].type == ItemID.None)
                {
                    items[i] = new Item(itemType);
                    items[i].shopCustomPrice = price;
                    startIndex = i + 1;
                    break;
                }
            }
        }
        public static bool IsModItem(Item item, string mod)
        {
            if (item.ModItem is not null)
            {
                string modName = item.ModItem.Mod.Name;
                return modName.Equals(mod);
            }

            return false;
        }
        public static Projectile ProjectileRain(IEntitySource source, Vector2 targetPos, float xLimit, float xVariance, float yLimitLower, float yLimitUpper, float projSpeed, int projType, int damage, float knockback, int owner)
        {
            float x = targetPos.X + Main.rand.NextFloat(-xLimit, xLimit);
            float y = targetPos.Y - Main.rand.NextFloat(yLimitLower, yLimitUpper);
            Vector2 spawnPosition = new Vector2(x, y);
            Vector2 velocity = targetPos - spawnPosition;
            velocity.X += Main.rand.NextFloat(-xVariance, xVariance);
            float speed = projSpeed;
            float targetDist = velocity.Length();
            targetDist = speed / targetDist;
            velocity.X *= targetDist;
            velocity.Y *= targetDist;
            return Projectile.NewProjectileDirect(source, spawnPosition, velocity, projType, damage, knockback, owner);
        }
        public static void HomeInOnNPC(Projectile projectile, bool ignoreTiles, float distanceRequired, float homingVelocity, float inertia)
        {
            if (!projectile.friendly)
                return;

            Vector2 destination = projectile.Center;
            float maxDistance = distanceRequired;
            bool locatedTarget = false;

            float npcDistCompare = 30000f;
            int index = -1;
            foreach (NPC n in Main.ActiveNPCs)
            {
                float extraDistance = (n.width / 2) + (n.height / 2);
                if (!n.CanBeChasedBy(projectile, false) || !projectile.WithinRange(n.Center, maxDistance + extraDistance))
                    continue;

                float currentNPCDist = Vector2.Distance(n.Center, projectile.Center);
                if ((currentNPCDist < npcDistCompare) && (ignoreTiles || Collision.CanHit(projectile.Center, 1, 1, n.Center, 1, 1)))
                {
                    npcDistCompare = currentNPCDist;
                    index = n.whoAmI;
                }
            }
            if (index != -1)
            {
                destination = Main.npc[index].Center;
                locatedTarget = true;
            }

            if (locatedTarget)
            {
                Vector2 homeDirection = (destination - projectile.Center).SafeNormalize(Vector2.UnitY);
                projectile.velocity = (projectile.velocity * inertia + homeDirection * homingVelocity) / (inertia + 1f);
            }
        }
        public static Player ToPlayer(this int ins)
        {
            if (ins < 0 || !Main.player[ins].active)
            {
                return Main.LocalPlayer;
            }

            return Main.player[ins];
        }
        public static bool AnyBossAlive()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].boss)
                    return true;
            }
            return false;
        }
        public static void DisplayLocalizedText(string key, Color? textColor = null)
        {
            if (!textColor.HasValue)
            {
                textColor = Color.Green;
            }
            if (Main.netMode == 0)
            {
                Main.NewText(Language.GetTextValue(key), (Color?)textColor.Value);
            }
            else if (Main.netMode == 2 || Main.netMode == 1)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key, Array.Empty<object>()), textColor.Value, -1);
            }
        }
        public static NPC FindClosestNPCForProj(float maxDetectDistance, Projectile proj)
        {
            NPC closestNPC = null;
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC npc = Main.npc[k];
                if (npc.CanBeChasedBy())
                {
                    float sqrDistanceToTarget = Vector2.DistanceSquared(proj.Center, npc.Center);
                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestNPC = npc;
                    }
                }
            }

            return closestNPC;
        }
    }
}

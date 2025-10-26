using Terraria.ModLoader;

namespace CSE.Core
{
    public static class ModCompatibility
    {
        public static class CatTech
        {
            public const string Name = "CatTech";
            public static bool Loaded => ModLoader.HasMod(Name);
            private static Mod mod = null;
            public static Mod Mod
            {
                get
                {
                    mod ??= ModLoader.GetMod(Name);
                    return mod;
                }
            }
        }
        public static class Crossmod
        {
            public const string Name = "FargowiltasCrossmod";
            public static bool Loaded => ModLoader.HasMod(Name);
            private static Mod mod = null;
            public static Mod Mod
            {
                get
                {
                    mod ??= ModLoader.GetMod(Name);
                    return mod;
                }
            }
        }
        public static class Homeward
        {
            public const string Name = "ContinentOfJourney";
            public static bool Loaded => ModLoader.HasMod(Name);
            private static Mod mod = null;
            public static Mod Mod
            {
                get
                {
                    mod ??= ModLoader.GetMod(Name);
                    return mod;
                }
            }
        }
        public static class Mutant
        {
            public const string Name = "Fargowiltas";
            public static bool Loaded => ModLoader.HasMod(Name);
            private static Mod mod = null;
            public static Mod Mod
            {
                get
                {
                    mod ??= ModLoader.GetMod(Name);
                    return mod;
                }
            }
        }
        public static class Souls
        {
            public const string Name = "FargowiltasSouls";
            public static bool Loaded => ModLoader.HasMod(Name);
            private static Mod mod = null;
            public static Mod Mod
            {
                get
                {
                    mod ??= ModLoader.GetMod(Name);
                    return mod;
                }
            }
        }
        public static class Calamity
        {
            public const string Name = "CalamityMod";
            public static bool Loaded => ModLoader.HasMod(Name);
            private static Mod mod = null;
            public static Mod Mod
            {
                get
                {
                    mod ??= ModLoader.GetMod(Name);
                    return mod;
                }
            }
        }
        public static class Thorium
        {
            public const string Name = "ThoriumMod";
            public static bool Loaded => ModLoader.HasMod(Name);
            private static Mod mod = null;
            public static Mod Mod
            {
                get
                {
                    mod ??= ModLoader.GetMod(Name);
                    return mod;
                }
            }
        }
        public static class Redemption
        {
            public const string Name = "Redemption";
            public static bool Loaded => ModLoader.HasMod(Name);
            private static Mod mod = null;
            public static Mod Mod
            {
                get
                {
                    mod ??= ModLoader.GetMod(Name);
                    return mod;
                }
            }
        }
        public static class SacredTools
        {
            public const string Name = "SacredTools";
            public static bool Loaded => ModLoader.HasMod(Name);
            private static Mod mod = null;
            public static Mod Mod
            {
                get
                {
                    mod ??= ModLoader.GetMod(Name);
                    return mod;
                }
            }
        }
    }
}
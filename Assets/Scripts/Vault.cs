public static class Vault
{
    public static class scene
    {
        public const string TitleScreen = "TitleScreen";
        public const string Planet = "Planet";
        public const string Ship = "Ship";
        public const string SelectorScreen = "SelectorScreen";
    }

    public static class layer
    {
        public const string Ennemies = "Ennemies";
        public const string Resources = "Resources";
        public const string EnnemiesOnly = "EnnemiesOnly";
        public const string ResourcesOnly = "ResourcesOnly";
        public const string ResourcesAndEnnemies = "ResourcesAndEnnemies";
        public const string Player = "Player";
        public const string Interactible = "Interactible";
    }

    public static class tag
    {
        public const string Player = "Player";
        public const string Ennemy = "Ennemy";
        public const string Resource = "Resource";
        public const string Obstacle = "Obstacle";

        public const string PurpleCollectible = "VioletCollectible";
        public const string GreenCollectible = "GreenCollectible";
        public const string OrangeCollectible = "OrangeCollectible";
    }

    public static class baseStats
    {
        public const int MaxHealth = 100;
        public const float Speed = 3.5f;
        public const float DamageResistance = 0;
    }

    public static class unlockable
    {
        public const string Radar = "RADAR";
        public const string ShipIndicator = "SHIP_INDICATOR";

    }

    public static class key
    {
        public const string ButtonTitle = "_title";
        public const string ButtonDescription = "_text";
        public const string Name = "Name";
        public const string Key = "Key";
        //TODO : switch from name to key everywhere

        public static class localization
        {
            public const string EN = "EN";
            public const string FR = "FR";
        }


        public static class upgrade
        {
            public const string CostGreen = "CostGreen";
            public const string CostOrange = "CostOrange";
            public const string UpgradesEnabled = "UpgradesEnabled";
            public const string UpgradesDisabled = "UpgradesDisabled";

            public const string MaxHealth = "MaxHealth";
            public const string BaseSpeed = "BaseSpeed";
            public const string DamageResistance = "DamageResistance";

            public const string BaseDamage = "BaseDamage";
            public const string AttackSpeed = "AttackSpeed";
            public const string Range = "Range";
            public const string Cooldown = "Cooldown";
            public const string Pierce = "Pierce";
            public const string Projectiles = "Projectiles";
            public const string Spread = "Spread";
            public const string AimingSpeed = "SpeedWhileAiming";
            public const string CriticalChance = "CriticalChance";
            public const string CriticalMultiplier = "CriticalMultiplier";
            public const string Magazine = "Magazine";
            public const string MagazineCooldown = "MagazineReloadTime";

            public const string MaxPurple = "MaxPurple";
            public const string MaxGreen = "MaxGreen";
            public const string MaxOrange = "MaxOrange";

            public const string AttractorRange = "AttractorRange";
            public const string AttractorForce = "AttractorForce";

            public const string Unlocks = "Unlocks";
        }
    }

    public static class other
    {
        public const string scriptableObjectMenu = "Cosmic Deserter/";
        public const string inputGamepad = "Gamepad";
        public const string cultureInfoFR = "fr-FR";
    }

}
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

    public static class other
    {
        public const string scriptableObjectMenu = "Cosmic Deserter/";
        public const string inputGamepad = "Gamepad";
        public const string cultureInfoFR = "fr-FR";
    }


    /*
    public const string character_Base = "Base";

    public const string playerParam_name = "Name";
    public const string playerParam_maxHealth = "maxHealth";
    public const string playerParam_baseSpeed = "BaseSpeed";
    public const string playerParam_damageResistance = "DamageResistance";
    public const string playerParam_baseDamage = "BaseDamage";
    public const string playerParam_attackSpeed = "AttackSpeed";
    public const string playerParam_range = "Range";
    public const string playerParam_cooldown = "Cooldown";
    public const string playerParam_pierce = "Pierce";
    public const string playerParam_projectiles = "Projectiles";
    public const string playerParam_spread = "Spread";
    public const string playerParam_aimingSpeed = "SpeedWhileAiming";
    public const string playerParam_criticalChance = "CriticalChance";
    public const string playerParam_criticalMultiplier = "CriticalMultiplier";
    public const string playerParam_magazine = "Magazine";
    public const string playerParam_magazineReloadTime = "MagazineReloadTime";

    public const string playerParam_toolPower = "ToolPower";
    public const string playerParam_toolRange = "ToolRange";
    public const string playerParam_toolSpeed = "ToolSpeed";

    public const string playerParam_maxPurple = "MaxPurple";
    public const string playerParam_maxGreen = "MaxGreen";
    public const string playerParam_maxOrange = "MaxOrange";
    */
}
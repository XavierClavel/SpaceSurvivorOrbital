public static class Vault
{

    public static class character
    {
        public const string Knil = "Knil";
    }

    public static class weapon
    {
        public const string Gun = "Gun";
    }

    public static class power
    {
        public const string DivineLightning = "Power_Lightning";
        public const string Shield = "Shield";
        public const string Ghost = "Ghost";
        public const string Fairy = "Fairy";
        public const string BlackHole = "BlackHole";
        public const string SynthWave = "SynthWave";
        public const string ToxicZone = "ToxicZone";
        public const string Dagger = "Dagger";
        public const string FlameThrower = "FlameThrower";
        public const string IceSpike = "IceSpike";
        public const string HuntersMark = "HuntersMark";
    }
    
    public static class sfx
    {
        public const string BreakResource = "BreakResource";
        public const string PlayerHit = "PlayerHit";
        public const string Shoot = "Shoot";
        public const string BulletHit = "BulletHit";
        public const string EnnemyExplosion = "EnnemyExplosion";
        public const string Reload = "Reload";
    }
    
    public static class path
    {
        public const string BaseWeapons = "BaseWeapons/";
        public const string AllWeapons = "AllWeapons/";
        public const string Powers = "Powers/";
        public const string Characters = "Characters/";
        public const string Equipments = "Equipments/";
        public const string Artefacts = "Artefacts/";
    }
    public static class scene
    {
        public const string TitleScreen = "TitleScreen";
        public const string Planet = "Planet";
        public const string Ship = "Ship";
        public const string Shop = "Shop";
        public const string ShopArtefact = "ShopArtefact";
        public const string PlanetSelector = "Planet Selection";
        public const string Win = "Win";
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
        public const string Obstacles = "Obstacles";
        public const string ObstaclesAndEnnemies = "ObstaclesAndEnnemies";
        public const string ObstaclesAndResources = "ObstaclesAndResources";
        public const string ObstaclesAndEnnemiesAndResources = "ObstaclesAndEnnemiesAndResources";
    }

    public static class tag
    {
        public const string Player = "Player";
        public const string Ennemy = "Ennemy";
        public const string Resource = "Resource";
        public const string Obstacle = "Obstacle";

        public const string GreenCollectible = "GreenCollectible";
        public const string OrangeCollectible = "OrangeCollectible";
    }

    public static class baseStats
    {
        public const int MaxHealth = 3;
        public const int MaxShields = 0;
        public const float Speed = 4.2f;
        public const float DamageMultiplier = 1;
    }

    public static class unlockable
    {
        public const string Radar = "RADAR";
        public const string ShipIndicator = "SHIP_INDICATOR";

    }

    public static class animatorParameter
    {
        public const string WalkDirection = "walkDirection";
        public const string AimDirection = "aimDirection";
        public const string State = "state";
    }

    public static class key
    {
        public const string ButtonTitle = "_title";
        public const string ButtonDescription = "_text";
        public const string ButtonCost = "_text";
        public const string ButtonCharge = "_text";
        public const string Name = "Name";
        public const string Key = "Key";
        public const string Sprite = "Sprite";
        //TODO : switch from name to key everywhere

        public static class sprite
        {
            public const string Sniper = "SNIPER";
            public const string Shotgun = "SHOTGUN";
            public const string DoubleGun = "DOUBLEGUN";
        }

        public static class target
        {
            public const string Gun = "GUN";
            public const string Laser = "LASER";
            public const string Pistolero = "PISTOLERO";
            public const string Pickaxe = "PICKAXE";
            public const string Ship = "SHIP";
        }

        public static class localization
        {
            public const string EN = "EN";
            public const string FR = "FR";
        }


        public static class upgrade
        {
            public const string CostGreen = "CostGreen";
            public const string CostOrange = "CostOrange";
            public const string CostBlue = "CostBlue";
            public const string Cost = "Cost";
            public const string UpgradesEnabled = "UpgradesEnabled";
            public const string UpgradesDisabled = "UpgradesDisabled";
            public const string Target = "Target";
            public const string Row = "Row";
            public const string SpriteKey = "SpriteKey";

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
            public const string Knockback = "Knockback";

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
public readonly struct UINames
{
    public const string Menu = "Menu";
    public const string Game = "Game";
    public const string GameOver = "GameOver";
    public const string Pause = "Pause";
    public const string Settings = "Settings";
}

public enum UITypes 
{
    Menu,
    Game,
    GameOver,
    Pause,
    Settings
}

public enum PooledObject
{
    Player,
    Enemy,
    Projectile
}

public enum WeaponType
{
    Pistol,
    MachineGun,
    Sniper
}

public struct PlayerStatsMultipliers
{
    public float damage;
    public float ammo;
    public float reload;
    public float maxHealth;
    public float healthRegen;
    public float moveSpeed;
}

public struct EnemyStatsMultipliers
{
    public float damage;
    public float maxHealth;
    public float healthRegen;
    public float moveSpeed;
}
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

public enum Tags
{
    Player,
    Enemy,
    Projectile
}

public struct StatsMultipliers
{
    public float damage;
    public float ammo;
    public float reload;
    public float maxHealth;
    public float healthRegen;
    public float moveSpeed;
}
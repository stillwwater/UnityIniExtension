UnityIniExtension
=================

UnityIni is a mini library for serializing object properties to `ini` configuration files in unity. `ini` is great because it's fast to parse, and allows the player or modders to easily change game settings. The underlying `Config` class, which `IniSerializeable` inherits from, can also be used to support other file formats.

This library saves files to `Application.persistentDataPath` asynchronously, and uses `TextAsset` objects for loading initial default configuration files. To use default files drag an `.ini.txt` file from your `Assets` folder into a `GameObject` with a `TextAssets` component.

## Serialization example:

```csharp
class PlayerSettings : IniSerializeable
{
    public float maxHealth;

    public PlayerSettings()
        : base("player.ini") { }

    public override void Deserialize() {
        base.Deserialize(); // important, loads file data
        maxHealth = LoadFloat("Status", "fMaxHealth"); // Load data
        Destroy(); // data no longer needed
    }

    public override void Serialize() {
        base.Serialize() // important, makes sure data is not null
        Store("Status", "fMaxHealth", maxHealth); // store data
    }
}
```

## Dynamic serialization example:

```csharp
class Player
{
    class PlayerSettings : IniSerializeable
    {
        public PlayerSettings()
            : base("player.ini") { }
    }

    void Start() {
        var settings = new PlayerSettings();
        float health = settings.LoadFloat("Status", "fMaxHealth");
    }
}
```

## Serialize and save all configuration files to disk:

```csharp
void OnApplicationQuit() {
    Config.SaveAllAsync();
}
```

For more examples check out the [examples directory](./Examples)

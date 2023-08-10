using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PlayerSaveLoadSystem : MonoBehaviour
{
    private PlayerExperienceSystem _experienceSystem;
    private PlayerHealthSystem _healthSystem;

    private string path;
    private string fileName = "/save.data";

    [Serializable]
    public class SPlayerData
    {
        public int level;
        public float experience;
        public float health;
    }

    public void Init(
        PlayerExperienceSystem experienceSystem, 
        PlayerHealthSystem healthSystem)
    {
        _experienceSystem = experienceSystem;
        _healthSystem = healthSystem;
    }

    private void Start()
    {
        path = Application.persistentDataPath + fileName;
    }

    public void Save()
    {
        var data = GatherPlayerData();
        WriteData(data);
        
        Debug.Log("Saved");
    }

    public void Load()
    {
        var data = ReadData();
        if (data != null)
        {
            LoadState(data);
        }
        
        Debug.Log("Loaded");
    }

    private SPlayerData GatherPlayerData()
    {
        var data = new SPlayerData();
        data.health = _healthSystem.Health;
        data.level = _experienceSystem.GetLevel();
        data.experience = _experienceSystem.GetExperience();

        return data;
    }

    private void LoadState(SPlayerData data)
    {
        _experienceSystem.SetLevel(data.level);
        _experienceSystem.AddExperience(data.experience);
        _healthSystem.Health = data.health;
    }

    private void WriteData(SPlayerData data)
    {
        var formatter = new BinaryFormatter();
        var stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    private SPlayerData ReadData()
    {
        if (File.Exists(path))
        {
            var formatter = new BinaryFormatter();
            var stream = new FileStream(path, FileMode.Open);
            var data = formatter.Deserialize(stream) as SPlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
}

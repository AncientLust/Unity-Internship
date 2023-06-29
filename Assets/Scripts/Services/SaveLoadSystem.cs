using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class EntityData
{
    public SerializableVector3 position;
    public int level;
    public float experience;
    public float health;
    public int equippedWeaponIndex;
    public int ammo;
}

[Serializable]
public class SaveData
{
    public EntityData playerData;
    public List<EntityData> enemiesData;
}

public class SaveLoadSystem : MonoBehaviour
{
    private string path;
    private string fileName = "/save.data";
    private const string _enemy = "Enemy";

    private void Start()
    {
        path = Application.persistentDataPath + fileName;
    }

    public void Save()
    {
        var data = GatherSaveData();
        WriteData(data);
        Debug.Log("Saved");
    }

    public void Load()
    {
        var data = ReadData();
        if (data != null)
        {
            DropAllEnemies();
            SetupPlayerState(data.playerData);
            RestoreEnemiesState(data.enemiesData);
            Debug.Log("Loaded");
        }
    }

    private void WriteData(SaveData data)
    {
        var formatter = new BinaryFormatter();
        var stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    private SaveData ReadData()
    {
        if (File.Exists(path))
        {
            var formatter = new BinaryFormatter();
            var stream = new FileStream(path, FileMode.Open);

            var saveData = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return saveData;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }

    private SaveData GatherSaveData()
    {
        var saveData = new SaveData();
        saveData.enemiesData = new List<EntityData>();

        foreach (ISaveable saveable in FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>())
        {
            if (saveable is Player)
            {
                saveData.playerData = saveable.CaptureState();
            }
            else if (saveable is Enemy)
            {
                saveData.enemiesData.Add(saveable.CaptureState());
            }
        }

        return saveData;
    }

    private void SetupPlayerState(EntityData playerData)
    {
        var player = FindObjectOfType<Player>();
        var saveable = player.gameObject.GetComponent<ISaveable>();
        if (saveable != null)
        {
            saveable.LoadState(playerData);
        }
    }

    private void DropAllEnemies()
    {
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            ObjectPool.Instance.Return(enemy.gameObject);
        }
    }

    private void RestoreEnemiesState(List<EntityData> enemiesData)
    {
        foreach (EntityData enemyData in enemiesData)
        {
            var enemy = ObjectPool.Instance.Get(_enemy);
            if (enemy != null)
            {
                enemy.GetComponent<Enemy>().Init();
                enemy.GetComponent<ISaveable>().LoadState(enemyData);
            }
        }
    }
}
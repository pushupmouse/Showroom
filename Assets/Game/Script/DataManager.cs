using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SaveData<T>(T data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/" + typeof(T).ToString() + ".json", json);
    }

    public T LoadData<T>()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/" + typeof(T).ToString() + ".json");
        return JsonUtility.FromJson<T>(json);
    }

    public void DeleteData<T>()
    {
        File.Delete(Application.persistentDataPath + "/" + typeof(T).ToString() + ".json");
    }

    public bool HasData<T>()
    {
        return File.Exists(Application.persistentDataPath + "/" + typeof(T).ToString() + ".json");
    }
}

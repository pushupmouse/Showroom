using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance;

    private DatabaseReference databaseReference;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        GetStudentInfo();
    }

    public void AddStudent(string name, int birthYear, string address, double grade)
    {
        Student newStudent = new Student(name, birthYear, address, grade);

        string json = JsonUtility.ToJson(newStudent);

        string newStudentKey = databaseReference.Child("students").Push().Key;

        databaseReference.Child("students").Child(newStudentKey).SetRawJsonValueAsync(json);
    }

    public IEnumerator GetName(Action<string> callback)
    {
        var studentData = databaseReference.Child("students").Child("-O78OMYFKkxI9PoMLY6X").Child("Name").GetValueAsync();
        //get student key
        //var studentData = databaseReference.Child("students").Child(studentKey).Child("Name").GetValueAsync();

        yield return new WaitUntil(predicate: () => studentData.IsCompleted);

        if (studentData != null)
        {
            DataSnapshot snapshot = studentData.Result;

            callback.Invoke(snapshot.Value.ToString());
        }
    }

    public void GetStudentInfo()
    {
        StartCoroutine(GetName(name => { Debug.Log("Name: " + name); }));
    }

}

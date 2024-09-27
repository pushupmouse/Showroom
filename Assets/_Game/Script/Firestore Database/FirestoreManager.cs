using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using System;
using Firebase.Extensions;

public class FirestoreManager : MonoBehaviour
{
    public static FirestoreManager Instance;

    private FirebaseFirestore database;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        database = FirebaseFirestore.DefaultInstance;
    }

    public void AddStudent(Student student)
    {
        CollectionReference studentsReference = database.Collection("students");

        studentsReference.AddAsync(student);
    }

    public void UpdateStudent(Student student)
    {
        DocumentReference studentReference = database.Collection("students").Document(student.Id);

        studentReference.SetAsync(student);
    }

    public void DeleteStudent(string studentId)
    {
        DocumentReference studentReference = database.Collection("students").Document(studentId);

        studentReference.DeleteAsync();
    }


    public IEnumerator GetAllStudents(Action<List<Student>> callback)
    {
        CollectionReference studentsReference = database.Collection("students");
        List<Student> studentList = new List<Student>();

        var task = studentsReference.OrderByDescending("Grade").GetSnapshotAsync();

        while (!task.IsCompleted)
        {
            yield return null; // Wait for the next frame
        }

        QuerySnapshot snapshot = task.Result;

        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            if (document.Exists)
            {
                Student student = document.ConvertTo<Student>();
                student.Id = document.Id;
                studentList.Add(student);
            }
        }

        callback.Invoke(studentList);
    }

    public IEnumerator GetStudentById(string id, Action<Student> callback)
    {
        CollectionReference studentsReference = database.Collection("students");

        var task = studentsReference.Document(id).GetSnapshotAsync();

        while (!task.IsCompleted)
        {
            yield return null;
        }

        DocumentSnapshot snapshot = task.Result;

        if (snapshot.Exists)
        {
            Student student = snapshot.ConvertTo<Student>();
            student.Id = snapshot.Id;
            callback.Invoke(student);
        }
        else
        {
            callback.Invoke(null);
        }
    }
}

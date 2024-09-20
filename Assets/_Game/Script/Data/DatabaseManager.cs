using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;
using System.Net;

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
        GetAllStudents();
    }

    public void AddStudent(string name, int birthYear, string address, double grade)
    {
        databaseReference.Child("lastStudentId").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                //get last id to set a new id
                int newStudentId = snapshot.Exists && snapshot.Value != null
                    ? int.Parse(snapshot.Value.ToString()) + 1
                    : 1; //set id to 1 if db empty

                Student newStudent = new Student(newStudentId, name, birthYear, address, grade);

                string json = JsonUtility.ToJson(newStudent);

                databaseReference.Child("students").Child(newStudentId.ToString()).SetRawJsonValueAsync(json);

                databaseReference.Child("lastStudentId").SetValueAsync(newStudentId);
            }
        });
    }

    public IEnumerator GetStudentDetails(Action<Student> callback, int id)
    {
        var studentData = databaseReference.Child("students").Child(id.ToString()).GetValueAsync();

        yield return new WaitUntil(predicate: () => studentData.IsCompleted);

        if (studentData.IsCompleted && studentData.Result != null)
        {
            DataSnapshot snapshot = studentData.Result;

            string json = snapshot.GetRawJsonValue();
            Student student = JsonUtility.FromJson<Student>(json);

            callback.Invoke(student);
        }
    }

    public void GetStudentById(int id)
    {
        StartCoroutine(GetStudentDetails(student =>
        {
            Debug.Log($"Name: {student.Name}, BirthYear: {student.BirthYear}, Address: {student.Address}, Grade: {student.Grade}");
        }, id));
    }

    public IEnumerator GetAllStudentsDetails(Action<List<Student>> callback)
    {
        var allStudentsData = databaseReference.Child("students").GetValueAsync();

        yield return new WaitUntil(predicate: () => allStudentsData.IsCompleted);

        if (allStudentsData.IsCompleted && allStudentsData.Result != null)
        {
            DataSnapshot snapshot = allStudentsData.Result;

            List<Student> studentsList = new List<Student>();

            foreach (var child in snapshot.Children)
            {
                string json = child.GetRawJsonValue();
                Student student = JsonUtility.FromJson<Student>(json);
                studentsList.Add(student);
            }

            callback.Invoke(studentsList);
        }
    }

    public void GetAllStudents()
    {
        StartCoroutine(GetAllStudentsDetails(studentList =>
        {
            Debug.Log("Number of students: " + studentList.Count);
        }));
    }
}

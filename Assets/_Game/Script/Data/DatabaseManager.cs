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

        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        // Disable persistence for real-time data only
        FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);

        // Ensure real-time sync for the "students" node
        databaseReference.Child("students").KeepSynced(true);
    }

    private void Start()
    {
        //AddStudent("test image", 2000, "hcm", 6.7);
    }

    public void AddStudent(string name, int birthYear, string address, double grade)
    {
        string hardcode = "https://firebasestorage.googleapis.com/v0/b/showroom-73238.appspot.com/o/studentmale.jpg?alt=media&token=049a9ffb-e233-4a5b-aa52-502ee87eb9f1";

        databaseReference.Child("lastStudentId").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                int newStudentId;

                // Get last id to set a new id
                if (snapshot.Exists && snapshot.Value != null)
                {
                    newStudentId = int.Parse(snapshot.Value.ToString()) + 1;
                }
                else // If the database is empty
                {
                    newStudentId = 1; // Set id to 1
                }

                Student newStudent = new Student(newStudentId, name, birthYear, address, grade, hardcode);
                string json = JsonUtility.ToJson(newStudent);

                databaseReference.Child("students").Child(newStudentId.ToString()).SetRawJsonValueAsync(json);
                databaseReference.Child("lastStudentId").SetValueAsync(newStudentId);
            }
        });
    }

    public void UpdateStudent(int studentId, string name, int birthYear, string address, double grade)
    {
        // Create a dictionary to hold the updated values
        var updatedData = new Dictionary<string, object>
        {
            { "Name", name },
            { "BirthYear", birthYear },
            { "Address", address },
            { "Grade", grade }
        };

        // Update the student data in the database
        databaseReference.Child("students").Child(studentId.ToString()).UpdateChildrenAsync(updatedData);
    }

    private IEnumerator GetStudentDetails(Action<Student> callback, int id)
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

    public void GetStudentById(int id, Action<Student> callback)
    {
        StartCoroutine(GetStudentDetails(student =>
        {
            callback.Invoke(student);
        }, id));
    }

    private IEnumerator GetAllStudentsDetails(Action<List<Student>> callback)
    {
        List<Student> studentsList = new List<Student>();

        // Fetch all students data from the database
        var allStudentsData = databaseReference.Child("students").GetValueAsync();

        yield return new WaitUntil(predicate: ()=> allStudentsData.IsCompleted);

        DataSnapshot snapshot = allStudentsData.Result;

        if (allStudentsData.IsCompleted)
        {
            // Iterate through all students and add them to the list
            foreach (var child in snapshot.Children)
            {
                string json = child.GetRawJsonValue();
                Student student = JsonUtility.FromJson<Student>(json);
                if (student != null)
                {
                    studentsList.Add(student);
                }
            }
        }

        // Invoke the callback with the retrieved list of students
        callback.Invoke(studentsList);
    }

    public void DeleteStudent(int studentId)
    {
        databaseReference.Child("students").Child(studentId.ToString()).RemoveValueAsync();
    }

    public void GetAllStudents(Action<List<Student>> callback)
    {
        StartCoroutine(GetAllStudentsDetails(callback));
    }
}

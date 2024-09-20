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

    private IEnumerator GetTotalStudentsCount(Action<int> callback)
    {
        var allStudentsData = databaseReference.Child("students").GetValueAsync();

        yield return new WaitUntil(predicate: () => allStudentsData.IsCompleted);

        if (allStudentsData.IsCompleted && allStudentsData.Result != null)
        {
            DataSnapshot snapshot = allStudentsData.Result;
            int count = (int)snapshot.ChildrenCount; // Get the number of children (students)
            callback.Invoke(count);
        }
        else
        {
            Debug.LogError("Failed to retrieve student count.");
            callback.Invoke(0); // Return 0 if there was an error
        }
    }

    private IEnumerator GetAllStudentsDetails(Action<List<Student>> callback, int expectedCount, int maxRetries = 3)
    {
        int attempts = 0;
        List<Student> studentsList = new List<Student>();

        while (attempts < maxRetries)
        {
            var allStudentsData = databaseReference.Child("students").GetValueAsync();

            yield return new WaitUntil(predicate: () => allStudentsData.IsCompleted);

            if (!allStudentsData.IsCompleted || allStudentsData.Result == null)
            {
                Debug.LogError("Failed to retrieve student data.");
                attempts++;
                yield return new WaitForSeconds(1); // Wait before retrying
                continue; // Skip to the next iteration
            }

            DataSnapshot snapshot = allStudentsData.Result;

            foreach (var child in snapshot.Children)
            {
                string json = child.GetRawJsonValue();
                Student student = JsonUtility.FromJson<Student>(json);
                if (student != null)
                {
                    studentsList.Add(student);
                }
            }

            // Check if we got the expected count
            if (studentsList.Count == expectedCount)
            {
                callback.Invoke(studentsList);
                yield break; // Exit if we got the expected count
            }

            Debug.LogWarning($"Attempt {attempts + 1}: Expected {expectedCount} students, but got {studentsList.Count}. Retrying...");
            attempts++;
            yield return new WaitForSeconds(1); // Wait before retrying
        }

        // Final callback with the retrieved list (even if incomplete)
        callback.Invoke(studentsList);
    }

    public void GetAllStudents(Action<List<Student>> callback)
    {
        StartCoroutine(GetTotalStudentsCount(count =>
        {
            StartCoroutine(GetAllStudentsDetails(callback, count)); // Pass the count to the details fetch
        }));
    }
}

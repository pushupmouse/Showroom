using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InteractiveScreen : MonoBehaviour
{
    [SerializeField] private StudentEntry entryTemplate;
    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject studentListCanvas;
    [SerializeField] private StudentDetails studentDetailsCanvas;
    [SerializeField] private StudentAddForm studentAddFormCanvas;
    [SerializeField] private Transform screen;
    [SerializeField] private Button addButton;

    private List<StudentEntry> spawnedEntries = new List<StudentEntry>();

    private void Start()
    {
        studentListCanvas.SetActive(true);
        addButton.onClick.AddListener(GoToAddForm);
        SortAndDisplayEntries();

        //GenerateEntries();
    }

    private void SortAndDisplayEntries()
    {
        DatabaseManager.Instance.GetAllStudents(students =>
        {
            // Sort the list based on grades
            students.Sort((s1, s2) => s2.Grade.CompareTo(s1.Grade)); // Sort in descending order

            SpawnEntries(students);
        });
    }

    private void SpawnEntries(List<Student> students)
    {
        //display from the list
        for (int i = 0; i < students.Count; i++)
        {
            StudentEntry newEntry = Instantiate(entryTemplate, grid.transform);
            int rankNum = i + 1;
            string rankString;

            switch (rankNum)
            {
                default: rankString = rankNum + "th"; break;

                case 1: rankString = rankNum + "st"; break;
                case 2: rankString = rankNum + "nd"; break;
                case 3: rankString = rankNum + "rd"; break;
            }

            spawnedEntries.Add(newEntry);

            newEntry.DisplayEntry(students[i], rankString, this);
        }
    }



    private void GoToAddForm()
    {
        //for (int i = spawnedEntries.Count; i > 0; i--)
        //{
        //    Destroy(spawnedEntries[i - 1].gameObject);
        //    spawnedEntries.Remove(spawnedEntries[i - 1]);
        //}

        //studentListCanvas.SetActive(false);
        //StudentAddForm studentAddForm = Instantiate(studentAddFormCanvas, screen);
    }

    public void SeeStudentDetails(int id)
    {
        for (int i = spawnedEntries.Count; i > 0; i--)
        {
            Destroy(spawnedEntries[i - 1].gameObject);
            spawnedEntries.Remove(spawnedEntries[i - 1]);
        }

        studentListCanvas.SetActive(false);
        StudentDetails studentDetails = Instantiate(studentDetailsCanvas, screen);


        DatabaseManager.Instance.GetStudentById(id, student =>
        {
            if (student != null)
            {
                studentDetails.SetText(student, this);
            }
        });
    }

    //public void AddStudent(string name, int yob, string address, double grade)
    //{
    //    //create
    //    Student student = new Student(name, yob, address, grade);

    //    //load
    //    string jsonString = PlayerPrefs.GetString("Student Entries");
    //    StudentEntries studentEntries = JsonUtility.FromJson<StudentEntries>(jsonString);

    //    //add to the list
    //    studentEntries.studentEntryList.Add(student);

    //    //save updated list
    //    string json = JsonUtility.ToJson(studentEntries);
    //    PlayerPrefs.SetString("Student Entries", json);
    //    PlayerPrefs.Save();

    //    SortEntries();
    //    DisplayStudentList();
    //}

    //public void DeleteStudent(int rankNum)
    //{
    //    string jsonString = PlayerPrefs.GetString("Student Entries");
    //    StudentEntries studentEntries = JsonUtility.FromJson<StudentEntries>(jsonString);

    //    studentEntries.studentEntryList.RemoveAt(rankNum);
    //    //save list
    //    string json = JsonUtility.ToJson(studentEntries);
    //    PlayerPrefs.SetString("Student Entries", json);
    //    PlayerPrefs.Save();

    //    //display updated list
    //    DisplayStudentList();
    //}

    public void RefreshStudentList()
    {
        studentListCanvas.SetActive(true);

        SortAndDisplayEntries();
    }

    //private class StudentEntries
    //{
    //    public List<Student> studentEntryList;
    //}

    //[Serializable]
    //public class Student
    //{
    //    public string Name;
    //    public int YoB;
    //    public string Address;
    //    public double Grade;
    //    //add image

    //    public Student(string name, int yob, string address, double grade)
    //    {
    //        Name = name;
    //        YoB = yob;
    //        Address = address;
    //        Grade = grade;
    //    }
    //}
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class StudentManager : MonoBehaviour
{
    public static StudentManager Instance;

    [SerializeField] private StudentEntry entryTemplate;
    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject studentListCanvas;
    [SerializeField] private StudentDetails studentDetailsCanvas;
    [SerializeField] private Transform screen;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //getting the student list from player prefs
        string jsonString = PlayerPrefs.GetString("Student Entries");
        StudentEntries studentEntries = JsonUtility.FromJson<StudentEntries>(jsonString);

        //sort the list
        for (int i = 0; i < studentEntries.studentEntryList.Count; i++)
        {
            for (int j = i + 1; j < studentEntries.studentEntryList.Count; j++)
            {
                if (studentEntries.studentEntryList[i].Grade < studentEntries.studentEntryList[j].Grade)
                {
                    Student tmp = studentEntries.studentEntryList[i];
                    studentEntries.studentEntryList[i] = studentEntries.studentEntryList[j];
                    studentEntries.studentEntryList[j] = tmp;
                }
            }
        }

        //display from the list
        for (int i = 0; i < studentEntries.studentEntryList.Count; i++)
        {
            StudentEntry newEntry = Instantiate(entryTemplate, grid.transform);

            int rank = i + 1;
            string rankString;

            switch (rank)
            {
                default: rankString = rank + "th"; break;

                case 1: rankString = rank + "st"; break;
                case 2: rankString = rank + "nd"; break;
                case 3: rankString = rank + "rd"; break;
            }

            newEntry.SaveDetails(rankString, studentEntries.studentEntryList[i].Name, studentEntries.studentEntryList[i].Grade);

            newEntry.DisplayEntry();
        }
    }

    private void AddStudent(string name, int yob, string address, double grade)
    {
        //create
        Student student = new Student(name, yob, address, grade);

        //load
        string jsonString = PlayerPrefs.GetString("Student Entries");
        StudentEntries studentEntries = JsonUtility.FromJson<StudentEntries>(jsonString);

        //add to the list
        studentEntries.studentEntryList.Add(student);

        //save updated list
        string json = JsonUtility.ToJson(studentEntries);
        PlayerPrefs.SetString("Student Entries", json);
        PlayerPrefs.Save();
    }

    public void SeeStudentDetails(string rank, string studentName, double grade)
    {
        studentListCanvas.SetActive(false);
        StudentDetails studentDetails = Instantiate(studentDetailsCanvas, screen);
        studentDetails.SetText(rank, studentName, grade);
    }

    private class StudentEntries
    {
        public List<Student> studentEntryList;
    }

    [Serializable]
    private class Student
    {
        public string Name;
        public int YoB;
        public string Address;
        public double Grade;
        //add image

        public Student(string name, int yob, string address, double grade)
        {
            Name = name;
            YoB = yob;
            Address = address;
            Grade = grade;
        }
    }
}

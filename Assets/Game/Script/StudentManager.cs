using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class StudentManager : MonoBehaviour
{
    [SerializeField] private Transform entryTemplate;
    [SerializeField] private GameObject grid;


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
            Transform newEntry = Instantiate(entryTemplate, grid.transform);

            int rank = i + 1;
            string rankString;

            switch (rank)
            {
                default: rankString = rank + "th"; break;

                case 1: rankString = rank + "st"; break;
                case 2: rankString = rank + "nd"; break;
                case 3: rankString = rank + "rd"; break;
            }

            newEntry.Find("Rank Text").GetComponent<TextMeshProUGUI>().text = rankString;
            newEntry.Find("Name Text").GetComponent<TextMeshProUGUI>().text = studentEntries.studentEntryList[i].Name;
            newEntry.Find("Grade Text").GetComponent<TextMeshProUGUI>().text = studentEntries.studentEntryList[i].Grade.ToString();
        }
    }

    private void AddStudent(string name, double grade)
    {
        //create
        Student student = new Student(name, grade);

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

    private class StudentEntries
    {
        public List<Student> studentEntryList;
    }

    [Serializable]
    private class Student
    {
        public string Name;
        public double Grade;

        public Student(string name, double grade)
        {
            Name = name;
            Grade = grade;
        }
    }
}

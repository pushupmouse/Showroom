using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class StudentManager : MonoBehaviour
{
    //public static StudentManager Instance;

    //[SerializeField] private StudentEntry entryTemplate;
    //[SerializeField] private GameObject grid;
    [SerializeField] private GameObject studentListCanvas;
    //[SerializeField] private StudentDetails studentDetailsCanvas;
    [SerializeField] private StudentAddForm studentAddFormCanvas;
    [SerializeField] private Transform screen;
    [SerializeField] private Button addButton;


    //private List<StudentEntry> spawnedEntries = new List<StudentEntry>();

    //private void Awake()
    //{
    //    Instance = this;
    //}

    private void Start()
    {
        addButton.onClick.AddListener(GoToAddForm);

        //SortEntries();

        //GenerateEntries();
    }



    //private void SortEntries()
    //{
    //    //getting the student list from player prefs
    //    string jsonString = PlayerPrefs.GetString("Student Entries");
    //    StudentEntries studentEntries = JsonUtility.FromJson<StudentEntries>(jsonString);

    //    //sort the list
    //    for (int i = 0; i < studentEntries.studentEntryList.Count; i++)
    //    {
    //        for (int j = i + 1; j < studentEntries.studentEntryList.Count; j++)
    //        {
    //            if (studentEntries.studentEntryList[i].Grade < studentEntries.studentEntryList[j].Grade)
    //            {
    //                Student tmp = studentEntries.studentEntryList[i];
    //                studentEntries.studentEntryList[i] = studentEntries.studentEntryList[j];
    //                studentEntries.studentEntryList[j] = tmp;
    //            }
    //        }
    //    }

    //    string jsonSorted = JsonUtility.ToJson(studentEntries);
    //    PlayerPrefs.SetString("Student Entries", jsonSorted);
    //    PlayerPrefs.Save();
    //}

    //private void GenerateEntries()
    //{
    //    //getting the student list from player prefs
    //    string jsonString = PlayerPrefs.GetString("Student Entries");
    //    StudentEntries studentEntries = JsonUtility.FromJson<StudentEntries>(jsonString);

    //    //display from the list
    //    for (int i = 0; i < studentEntries.studentEntryList.Count; i++)
    //    {
    //        StudentEntry newEntry = Instantiate(entryTemplate, grid.transform);
    //        spawnedEntries.Add(newEntry);
    //        int rankNum = i + 1;
    //        string rankString;

    //        switch (rankNum)
    //        {
    //            default: rankString = rankNum + "th"; break;

    //            case 1: rankString = rankNum + "st"; break;
    //            case 2: rankString = rankNum + "nd"; break;
    //            case 3: rankString = rankNum + "rd"; break;
    //        }

    //        newEntry.SaveDetails(rankNum, rankString, studentEntries.studentEntryList[i].Name, studentEntries.studentEntryList[i].Grade,
    //            studentEntries.studentEntryList[i].YoB, studentEntries.studentEntryList[i].Address);

    //        newEntry.DisplayEntry();
    //    }
    //}



    private void GoToAddForm()
    {
        //for (int i = spawnedEntries.Count; i > 0; i--)
        //{
        //    Destroy(spawnedEntries[i - 1].gameObject);
        //    spawnedEntries.Remove(spawnedEntries[i - 1]);
        //}

        studentListCanvas.SetActive(false);
        StudentAddForm studentAddForm = Instantiate(studentAddFormCanvas, screen);
    }

    //public void SeeStudentDetails(int rankNum, string rank)
    //{
    //    for(int i = spawnedEntries.Count; i > 0; i--)
    //    {
    //        Destroy(spawnedEntries[i - 1].gameObject);
    //        spawnedEntries.Remove(spawnedEntries[i - 1]);
    //    }

    //    studentListCanvas.SetActive(false);
    //    StudentDetails studentDetails = Instantiate(studentDetailsCanvas, screen);

    //    string jsonString = PlayerPrefs.GetString("Student Entries");
    //    StudentEntries studentEntries = JsonUtility.FromJson<StudentEntries>(jsonString);

    //    studentDetails.SetText(rank, studentEntries.studentEntryList[rankNum - 1].Name,
    //        studentEntries.studentEntryList[rankNum - 1].Grade,
    //        studentEntries.studentEntryList[rankNum - 1].YoB,
    //        studentEntries.studentEntryList[rankNum - 1].Address,
    //        rankNum - 1);

    //}

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

    //public void DisplayStudentList()
    //{
    //    studentListCanvas.SetActive(true);

    //    GenerateEntries();
    //}

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

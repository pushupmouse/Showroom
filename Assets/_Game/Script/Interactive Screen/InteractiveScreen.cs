using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        StartCoroutine(DisplayEntries());
    }

    private IEnumerator DisplayEntries()
    {
        yield return StartCoroutine(FirestoreManager.Instance.GetAllStudents(students =>
        {
            if (students != null)
            {
                SpawnEntries(students);
            }
        }));
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
        studentListCanvas.SetActive(false);

        StudentAddForm studentAddForm = Instantiate(studentAddFormCanvas, screen);
        studentAddForm.OnInit(this);
        studentAddForm.SetToAdd();
    }

    public void GoToEditForm(Student student)
    {
        studentListCanvas.SetActive(false);

        StudentAddForm studentEditForm = Instantiate(studentAddFormCanvas, screen);
        studentEditForm.OnInit(this);
        studentEditForm.SetToEdit(student);
    }

    public void DeleteStudent(Student student)
    {
        FirestoreManager.Instance.DeleteStudent(student.Id);
        RefreshStudentList();
    }

    public IEnumerator SeeStudentDetails(string id)
    {
        studentListCanvas.SetActive(false);
        StudentDetails studentDetails = Instantiate(studentDetailsCanvas, screen);

        yield return StartCoroutine(FirestoreManager.Instance.GetStudentById(id, student =>
        {
            if (student != null)
            {
                studentDetails.SetText(student, this);
            }
        }));
    }

    private void ClearEntries()
    {
        for (int i = spawnedEntries.Count; i > 0; i--)
        {
            Destroy(spawnedEntries[i - 1].gameObject);
            spawnedEntries.Remove(spawnedEntries[i - 1]);
        }
    }

    public void RefreshStudentList()
    {
        ClearEntries();
        studentListCanvas.SetActive(true);
        StartCoroutine(DisplayEntries());
    }
}

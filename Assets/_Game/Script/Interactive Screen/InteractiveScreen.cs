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
    [SerializeField] private AuthView authViewCanvas;
    [SerializeField] private StudentDetails studentDetailsCanvas;
    [SerializeField] private StudentAddForm studentAddFormCanvas;
    [SerializeField] private Transform screen;
    [SerializeField] private Button addButton;

    private List<StudentEntry> spawnedEntries = new List<StudentEntry>();

    private void Start()
    {
        addButton.onClick.AddListener(GoToAddForm);

        FirebaseAuthManager.Instance.OnLoginSuccess += HandleLoginSuccess;

        DisplayAuthView();
    }

    private void HandleLoginSuccess()
    {
        Debug.Log("Login Successful! Now showing student list view...");
        ShowStudentList();

        FirebaseAuthManager.Instance.GetUserRole(role =>
        {
            if (role != null)
            {
                Debug.Log("Current User Role: " + role);
            }
            else
            {
                Debug.Log("Failed to fetch the user role.");
            }
        });
    }
    private void DisplayAuthView()
    {
        AuthView authview = Instantiate(authViewCanvas, screen);
        authview.ShowLogin();
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
        ShowStudentList();
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

    public void ShowStudentList()
    {
        ClearEntries();
        studentListCanvas.SetActive(true);
        StartCoroutine(DisplayEntries());
    }
}

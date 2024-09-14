using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class StudentAddForm : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField yobInput;
    [SerializeField] private TMP_InputField addressInput;
    [SerializeField] private TMP_InputField gradeInput;

    private void Start()
    {
        backButton.onClick.AddListener(GoBack);
        saveButton.onClick.AddListener(CreateNewStudent);
    }

    private void CreateNewStudent()
    {
        Debug.Log(nameInput.text);
        Debug.Log(yobInput.text);
        Debug.Log(addressInput.text);
        Debug.Log(gradeInput.text);

        StudentManager.Instance.AddStudent(nameInput.text, int.Parse(yobInput.text), addressInput.text, double.Parse(gradeInput.text));
        Destroy(gameObject);
    }

    private void GoBack()
    {
        StudentManager.Instance.DisplayStudentList();
        Destroy(gameObject);
    }
}

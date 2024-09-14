using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        Regex yearValidation = new Regex("^\\d*$");
        Regex gradeValidation = new Regex("^\\d*\\.?\\d*$");

        if (yearValidation.IsMatch(yobInput.text) && gradeValidation.IsMatch(gradeInput.text))
        {
            StudentManager.Instance.AddStudent(nameInput.text, int.Parse(yobInput.text), addressInput.text, double.Parse(gradeInput.text));
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("not valid");
        }        
    }

    private void GoBack()
    {
        StudentManager.Instance.DisplayStudentList();
        Destroy(gameObject);
    }
}
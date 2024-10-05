using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StudentDetails : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI studentNameText;
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private TextMeshProUGUI yobText;
    [SerializeField] private TextMeshProUGUI addressText;
    [SerializeField] private Button backButton;
    [SerializeField] private Button editButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private ImageLoader imageLoader;

    private InteractiveScreen screen;
    private Student student;

    private void Start()
    {
        backButton.onClick.AddListener(OnBackButtonClick);
        editButton.onClick.AddListener(OnEditButtonClick);
        deleteButton.onClick.AddListener(OnDeleteButtonClick);
    }

    public void RestrictFunctions()
    {
        editButton.gameObject.SetActive(false);
        deleteButton.gameObject.SetActive(false);
    }

    public void SetText(Student student, InteractiveScreen screen)
    {
        this.student = student;

        studentNameText.text = student.Name;
        gradeText.text = student.Grade.ToString();
        yobText.text = student.BirthYear.ToString();
        addressText.text = student.Address;

        this.screen = screen;

        StartCoroutine(imageLoader.SetImage(student.ImageName));
    }

    private void OnBackButtonClick()
    {
        screen.ShowStudentList();
        Destroy(gameObject);
    }

    private void OnEditButtonClick()
    {
        screen.GoToEditForm(student);
        Destroy(gameObject);
    }

    private void OnDeleteButtonClick()
    {
        screen.DeleteStudent(student);
        Destroy(gameObject);
    }
}

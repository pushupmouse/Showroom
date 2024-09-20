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
    [SerializeField] private Button deleteButton;

    private InteractiveScreen screen;

    private void Start()
    {
        backButton.onClick.AddListener(GoBack);
        deleteButton.onClick.AddListener(DeleteStudent);
    }

    public void SetText(Student student, InteractiveScreen screen)
    {
        studentNameText.text = student.Name;
        gradeText.text = student.Grade.ToString();
        yobText.text = student.BirthYear.ToString();
        addressText.text = student.Address;

        this.screen = screen;
    }

    private void GoBack()
    {
        screen.RefreshStudentList();
        Destroy(gameObject);
    }

    private void DeleteStudent()
    {
        //StudentManager.Instance.DeleteStudent(rankNum);
        //Destroy(gameObject);
    }
}

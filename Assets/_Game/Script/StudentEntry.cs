using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StudentEntry : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI gradeText;

    private InteractiveScreen screen;
    private int studentId;

    private void Start()
    {
        button.onClick.AddListener(SeeDetails);
    }

    public void DisplayEntry(Student student, string rankString, InteractiveScreen screen)
    {
        rankText.text = rankString;
        nameText.text = student.Name;
        gradeText.text = student.Grade.ToString();

        this.screen = screen;
        studentId = student.Id;
    }

    public void SeeDetails()
    {
        screen.SeeStudentDetails(studentId);
    }
}

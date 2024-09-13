using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StudentDetails : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI studentNameText;
    [SerializeField] private TextMeshProUGUI gradeText;

    public void SetText(string rank, string studentName, double grade)
    {
        rankText.text = rank;
        studentNameText.text = studentName;
        gradeText.text = grade.ToString();
    }
}

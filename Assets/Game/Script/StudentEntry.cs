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
    private string rank, studentName;
    private double grade;

    private void Start()
    {
        button.onClick.AddListener(SeeDetails);
    }

    public void SaveDetails(string rank, string studentName, double grade)
    {
        this.rank = rank;
        this.studentName = studentName;
        this.grade = grade;
    }

    public void DisplayEntry()
    {
        transform.Find("Rank Text").GetComponent<TextMeshProUGUI>().text = rank;
        transform.Find("Name Text").GetComponent<TextMeshProUGUI>().text = studentName;
        transform.Find("Grade Text").GetComponent<TextMeshProUGUI>().text = grade.ToString();
    }

    public void SeeDetails()
    {
        StudentManager.Instance.SeeStudentDetails(rank, studentName, grade);
    }
}

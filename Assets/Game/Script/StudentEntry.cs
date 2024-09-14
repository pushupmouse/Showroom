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
    private string rank, studentName, address;
    private double grade;
    private int rankNum, yob;

    private void Start()
    {
        button.onClick.AddListener(SeeDetails);
    }

    public void SaveDetails(int rankNum, string rank, string studentName, double grade, int yob, string address)
    {
        this.rankNum = rankNum;
        this.rank = rank;
        this.studentName = studentName;
        this.grade = grade;
        this.address = address;
        this.yob = yob;
    }

    public void DisplayEntry()
    {
        transform.Find("Rank Text").GetComponent<TextMeshProUGUI>().text = rank.ToString();
        transform.Find("Name Text").GetComponent<TextMeshProUGUI>().text = studentName;
        transform.Find("Grade Text").GetComponent<TextMeshProUGUI>().text = grade.ToString();
    }

    public void SeeDetails()
    {
        StudentManager.Instance.SeeStudentDetails(rankNum, rank);
    }
}

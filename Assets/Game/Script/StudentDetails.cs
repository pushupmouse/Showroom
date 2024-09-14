using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StudentDetails : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI studentNameText;
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private TextMeshProUGUI yobText;
    [SerializeField] private TextMeshProUGUI addressText;
    [SerializeField] private Button backButton;
    [SerializeField] private Button deleteButton;

    private int rankNum;


    private void Start()
    {
        backButton.onClick.AddListener(GoBack);
        deleteButton.onClick.AddListener(DeleteStudent);
    }

    public void SetText(string rank, string studentName, double grade, int yob, string address, int rankNum)
    {
        rankText.text = rank;
        studentNameText.text = studentName;
        gradeText.text = grade.ToString();
        yobText.text = yob.ToString();
        addressText.text = address;
        this.rankNum = rankNum;
    }

    private void GoBack()
    {
        StudentManager.Instance.DisplayStudentList();
        Destroy(gameObject);
    }

    private void DeleteStudent()
    {
        StudentManager.Instance.DeleteStudent(rankNum);
        Destroy(gameObject);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StudentManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private GameObject grid;

    private List<Student> students = new List<Student>();

    private void Start()
    {
        for (int i = 1; i < 15; i++)
        {
            Student student = new Student("Nguyen Van A" + i, i+1);

            students.Add(student);
        }

        for(int i = 0; i < students.Count; i++)
        {
            TextMeshProUGUI newText = Instantiate(textName, grid.transform);
            newText.text = students[i].Name + " - Grade: " + students[i].Grade.ToString();
        }
    }
}

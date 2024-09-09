using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class StudentManager : MonoBehaviour
{
    [SerializeField] private Transform entryTemplate;
    [SerializeField] private GameObject grid;

    private List<Student> students = new List<Student>();

    private void Start()
    {
        //hard init
        for (int i = 0; i < 20; i++)
        {
            float randomGrade = Random.Range(0.0f, 10.0f);
            double roundedGrade = Math.Round(randomGrade, 1);
            Student student = new Student("Nguyen Van A" + i, roundedGrade);

            students.Add(student);
        }

        //sort
        for (int  i = 0; i < students.Count; i++)
        {
            for(int j = i + 1; j < students.Count; j++)
            {
                if (students[i].Grade < students[j].Grade)
                {
                    Student tmp = students[i];
                    students[i] = students[j];
                    students[j] = tmp;
                }
            }
        }

        //display
        for(int i = 0; i < students.Count; i++)
        {
            Transform newEntry = Instantiate(entryTemplate, grid.transform);

            int rank = i + 1;
            string rankString;

            switch (rank)
            {
                default: rankString = rank + "th"; break;

                case 1: rankString = rank + "st"; break;
                case 2: rankString = rank + "nd"; break;
                case 3: rankString = rank + "rd"; break;
            }

            newEntry.Find("Rank Text").GetComponent<TextMeshProUGUI>().text = rankString;
            newEntry.Find("Name Text").GetComponent<TextMeshProUGUI>().text = students[i].Name;
            newEntry.Find("Grade Text").GetComponent<TextMeshProUGUI>().text = students[i].Grade.ToString();
        }
    }
}

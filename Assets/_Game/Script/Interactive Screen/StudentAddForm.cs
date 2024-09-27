using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;


public class StudentAddForm : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField birthYearInput;
    [SerializeField] private TMP_InputField addressInput;
    [SerializeField] private TMP_InputField gradeInput;

    private InteractiveScreen screen;
    private string id;

    private void Start()
    {
        backButton.onClick.AddListener(GoBack);
    }

    public void OnInit(InteractiveScreen screen)
    {
        this.screen = screen;
    }

    public void SetToAdd()
    {
        saveButton.onClick.RemoveListener(UpdateStudent);
        saveButton.onClick.AddListener(CreateNewStudent);
    }

    public void SetToEdit(Student student)
    {
        id = student.Id;
        
        nameInput.text = student.Name;
        birthYearInput.text = student.BirthYear.ToString();
        addressInput.text = student.Address;
        gradeInput.text = student.Grade.ToString();

        saveButton.onClick.RemoveListener(CreateNewStudent);
        saveButton.onClick.AddListener(UpdateStudent);
    }

    private void CreateNewStudent()
    {
        Regex yearValidation = new Regex("^\\d*$");
        Regex gradeValidation = new Regex("^\\d*\\.?\\d*$");

        if (yearValidation.IsMatch(birthYearInput.text) && gradeValidation.IsMatch(gradeInput.text))
        {
            Student student = new Student(nameInput.text, int.Parse(birthYearInput.text), addressInput.text, double.Parse(gradeInput.text), "imageUrl");
            FirestoreManager.Instance.AddStudent(student);
            GoBack();
        }
        else
        {
            Debug.Log("not valid");
        }        
    }

    private void UpdateStudent()
    {
        Regex yearValidation = new Regex("^\\d*$");
        Regex gradeValidation = new Regex("^\\d*\\.?\\d*$");

        if (yearValidation.IsMatch(birthYearInput.text) && gradeValidation.IsMatch(gradeInput.text))
        {
            Student student = new Student(nameInput.text, int.Parse(birthYearInput.text), addressInput.text, double.Parse(gradeInput.text), "imageUrl");
            student.Id = id;

            FirestoreManager.Instance.UpdateStudent(student);
            GoBack();
        }
        else
        {
            Debug.Log("not valid");
        }
    }

    private void GoBack()
    {
        screen.RefreshStudentList();
        Destroy(gameObject);
    }
}

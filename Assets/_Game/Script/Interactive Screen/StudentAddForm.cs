using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StudentAddForm : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField birthYearInput;
    [SerializeField] private TMP_InputField addressInput;
    [SerializeField] private TMP_InputField gradeInput;
    [SerializeField] private Button uploadButton;

    private InteractiveScreen screen;
    private string id;
    private string imageName;
    private bool isImageUploaded = false; // Flag for image upload

    private void Start()
    {
        backButton.onClick.AddListener(GoBack);
        uploadButton.onClick.AddListener(UploadImage);
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

        imageName = student.ImageName;

        saveButton.onClick.RemoveListener(CreateNewStudent);
        saveButton.onClick.AddListener(UpdateStudent);
    }

    private void CreateNewStudent()
    {
        // Regex for year and grade validation
        Regex yearValidation = new Regex("^\\d*$");
        Regex gradeValidation = new Regex("^\\d*\\.?\\d*$");

        // Check if image is uploaded
        if (!isImageUploaded)
        {
            Debug.Log("Please upload an image before adding the student.");
            return;
        }

        // Validate that name and address fields are not empty
        if (string.IsNullOrWhiteSpace(nameInput.text))
        {
            Debug.Log("Name is required.");
            return;
        }

        if (string.IsNullOrWhiteSpace(addressInput.text))
        {
            Debug.Log("Address is required.");
            return;
        }

        // Validate birth year and grade
        if (yearValidation.IsMatch(birthYearInput.text) && gradeValidation.IsMatch(gradeInput.text))
        {
            // Create a new student and save it
            Student student = new Student(nameInput.text, int.Parse(birthYearInput.text), addressInput.text, double.Parse(gradeInput.text), imageName);
            FirestoreManager.Instance.AddStudent(student);
            GoBack();
        }
        else
        {
            Debug.Log("Invalid input. Please check birth year and grade.");
        }
    }

    private void UpdateStudent()
    {
        Regex yearValidation = new Regex("^\\d*$");
        Regex gradeValidation = new Regex("^\\d*\\.?\\d*$");

        if (yearValidation.IsMatch(birthYearInput.text) && gradeValidation.IsMatch(gradeInput.text))
        {
            Student student = new Student(nameInput.text, int.Parse(birthYearInput.text), addressInput.text, double.Parse(gradeInput.text), imageName);
            student.Id = id;

            FirestoreManager.Instance.UpdateStudent(student);
            GoBack();
        }
        else
        {
            Debug.Log("Invalid input. Please check birth year and grade.");
        }
    }

    private void GoBack()
    {
        screen.ShowStudentList();
        Destroy(gameObject);
    }

    private void UploadImage()
    {
        StartCoroutine(ImageUploader.Instance.ShowLoadDialogCoroutine(callback =>
        {
            imageName = callback.ToString();
            isImageUploaded = true; // Set flag to true when image is uploaded
            Debug.Log("Image uploaded successfully.");
        }));
    }
}

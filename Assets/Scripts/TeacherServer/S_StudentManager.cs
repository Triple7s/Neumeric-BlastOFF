using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_StudentManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField nameInputField;       // Input for student name
    public Button submitButton;                 // Button for submit

    [Header("Uploader")]
    public S_JsonUploader uploader;

    [Header("Student Info")]
    public string studentName;
    public string studentID;

    void Start()
    {
        if (submitButton != null)
            submitButton.onClick.AddListener(OnSubmit);
        else
            Debug.LogWarning("Submit button not assigned in the Inspector!");
    }

    void OnSubmit()
    {
        studentName = nameInputField.text;

        if (string.IsNullOrEmpty(studentName))
        {
            Debug.LogWarning("Name field is empty!");
            return;
        }
    

        studentID = GenerateStudentID();

        Debug.Log($"Student Name: {studentName}, Student ID: {studentID}");

        if (uploader != null)
        {
            uploader.UploadFromButton(studentID, studentName);
        }
        else
        {
            Debug.LogWarning("Uploader not assigned!");
        }
    }

    string GenerateStudentID()
    {
        int id = Random.Range(10000, 99999);
        return id.ToString();
    }
}

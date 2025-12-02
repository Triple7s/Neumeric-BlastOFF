using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class S_SubmitIP : MonoBehaviour
{
    [SerializeField] private TMP_InputField ipInputField;
    
    private string _ipAddress;

    public void OnSubmitIP()
    {
        _ipAddress = ipInputField.text;
        string formattedIP = "";
        string jsonPath = Path.Combine(Application.persistentDataPath, "answers.json");
        if (_ipAddress.Length >= 8)
        {
            formattedIP = FirstFormatVersion(_ipAddress);
            Debug.Log(formattedIP);

            string uploadUrl = "http://" + formattedIP + ":5000/upload";
            StartCoroutine(QtmJsonUploader.UploadJson(jsonPath, uploadUrl));
        }

        if (_ipAddress.Length >= 11)
        {
            formattedIP = SecondFormatVersion(_ipAddress);
            Debug.Log(formattedIP);

            string uploadUrl = "http://" + formattedIP + ":5000/upload";
            StartCoroutine(QtmJsonUploader.UploadJson(jsonPath, uploadUrl));
        }

        if (_ipAddress.Length >= 9)
        {
            formattedIP = ThirdFormatVersion(_ipAddress);
            Debug.Log(formattedIP);

            string uploadUrl = "http://" + formattedIP + ":5000/upload";
            StartCoroutine(QtmJsonUploader.UploadJson(jsonPath, uploadUrl));
        }

        if (_ipAddress.Length >= 9)
        {
            formattedIP = FourthFormatVersion(_ipAddress);
            Debug.Log(formattedIP);

            string uploadUrl = "http://" + formattedIP + ":5000/upload";
            StartCoroutine(QtmJsonUploader.UploadJson(jsonPath, uploadUrl));
        }

        if (_ipAddress.Length >= 9)
        {
            formattedIP = FifthFormatVersion(_ipAddress);
            Debug.Log(formattedIP);

            string uploadUrl = "http://" + formattedIP + ":5000/upload";
            StartCoroutine(QtmJsonUploader.UploadJson(jsonPath, uploadUrl));
        }

        if (_ipAddress.Length >= 12)
        {
            formattedIP = SixthFormatVersion(_ipAddress);
            Debug.Log(formattedIP);

            string uploadUrl = "http://" + formattedIP + ":5000/upload";
            StartCoroutine(QtmJsonUploader.UploadJson(jsonPath, uploadUrl));
        }

        if (_ipAddress.Length >= 10)
        {
            formattedIP = SeventhFormatVersion(_ipAddress);
            Debug.Log(formattedIP);

            string uploadUrl = "http://" + formattedIP + ":5000/upload";
            StartCoroutine(QtmJsonUploader.UploadJson(jsonPath, uploadUrl));
        }

    }
    
    private string FirstFormatVersion(string ip)
    {
        string first = ip.Substring(0, 3);
        string second = ip.Substring(3, 3);
        string third = ip.Substring(6, 1);
        string fourth = ip.Substring(7, 1);
        return first + "." + second + "." + third + "." + fourth;
    }

    private string SecondFormatVersion(string ip)
    {
        string first = ip.Substring(0, 3);
        string second = ip.Substring(3, 2);
        string third = ip.Substring(5, 3);
        string fourth = ip.Substring(8, 3);
        return first + "." + second + "." + third + "." + fourth;
    }
    
    private string ThirdFormatVersion(string ip)
    {
        string first = ip.Substring(0, 3);
        string second = ip.Substring(3, 2);
        string third = ip.Substring(5, 2);
        string fourth = ip.Substring(7, 2);
        return first + "." + second + "." + third + "." + fourth;
    }

    private string FourthFormatVersion(string ip)
    {
        string first = ip.Substring(0, 3);
        string second = ip.Substring(3, 2);
        string third = ip.Substring(5, 3);
        string fourth = ip.Substring(8, 1);
        return first + "." + second + "." + third + "." + fourth;
    }

    private string FifthFormatVersion(string ip)
    {
        string first = ip.Substring(0, 2);
        string second = ip.Substring(2, 3);
        string third = ip.Substring(5, 2);
        string fourth = ip.Substring(7, 2);
        return first + "." + second + "." + third + "." + fourth;
    }

    private string SixthFormatVersion(string ip)
    {
        string first = ip.Substring(0, 3);
        string second = ip.Substring(3, 3);
        string third = ip.Substring(6, 3);
        string fourth = ip.Substring(9, 3);
        return first + "." + second + "." + third + "." + fourth;
    }

    private string SeventhFormatVersion(string ip)
    {
        string first = ip.Substring(0, 3);
        string second = ip.Substring(3, 2);
        string third = ip.Substring(5, 3);
        string fourth = ip.Substring(8, 2);
        return first + "." + second + "." + third + "." + fourth;
    }
}


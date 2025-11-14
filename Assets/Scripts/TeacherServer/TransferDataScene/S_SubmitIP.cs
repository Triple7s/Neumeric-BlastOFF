using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_SubmitIP : MonoBehaviour
{
    [SerializeField] private TMP_InputField ipInputField;
    
    private string _ipAddress;

    public void OnSubmitIP()
    {
        _ipAddress = ipInputField.text;
        string formattedIP = FirstFormatVersion(_ipAddress);
        Debug.Log(formattedIP);
        formattedIP = SecondFormatVersion(_ipAddress);
        Debug.Log(formattedIP);
        formattedIP = ThirdFormatVersion(_ipAddress);
        Debug.Log(formattedIP);
        formattedIP = FourthFormatVersion(_ipAddress);
        Debug.Log(formattedIP);
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
        string fourth = ip.Substring(7, 1);
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
}


using TMPro;
using UnityEngine;
using System.IO;

public class S_SubmitIP : MonoBehaviour
{
    [SerializeField] private TMP_InputField ipInputField;

    public void OnSubmitIP()
    {
        string rawInput = ipInputField.text.Trim();
        string jsonPath = Path.Combine(Application.persistentDataPath, "answers.json");

        Debug.Log("[SubmitIP] Raw input: " + rawInput);
        Debug.Log("[SubmitIP] JSON path: " + jsonPath);

        if (!File.Exists(jsonPath))
        {
            Debug.LogError("[SubmitIP] ERROR: answers.json does NOT exist!");
            return;
        }

        // STEP 1 — Clean input (remove spaces)
        string cleaned = rawInput.Replace(" ", "");
        Debug.Log("[SubmitIP] Cleaned: " + cleaned);

        // STEP 2 — Try to convert digits-only input → dot-separated IP
        string formattedIP = TryAutoFormatIP(cleaned);

        if (formattedIP == null)
        {
            Debug.LogError("[SubmitIP] INVALID IP — user input does not map to IPv4 format.");
            return;
        }

        Debug.Log("[SubmitIP] Final IP used: " + formattedIP);

        string uploadURL = $"http://{formattedIP}:5000/upload";

        Debug.Log("[SubmitIP] Upload URL: " + uploadURL);

        StartCoroutine(QtmJsonUploader.UploadJson(jsonPath, uploadURL));
    }

    /// Generates valid IPv4 from numeric strings like:
    /// 158384412 → 158.38.44.12
    /// 192168110 → 192.168.1.10
    /// Rejects invalid patterns.
    private string TryAutoFormatIP(string s)
    {
        // If input already contains dots → assume it's directly entered IP
        if (s.Contains(".")) 
        {
            Debug.Log("[IP Formatter] Using user-entered dotted IP.");
            return s;
        }

        // Only allow digits
        foreach (char c in s)
        {
            if (!char.IsDigit(c)) return null;
        }

        // Must be 8–12 digits
        if (s.Length < 8 || s.Length > 12)
            return null;

        // Try slicing into 4 blocks
        for (int a = 1; a <= 3; a++)
        for (int b = 1; b <= 3; b++)
        for (int c = 1; c <= 3; c++)
        for (int d = 1; d <= 3; d++)
        {
            if (a + b + c + d != s.Length) continue;

            string p1 = s.Substring(0, a);
            string p2 = s.Substring(a, b);
            string p3 = s.Substring(a + b, c);
            string p4 = s.Substring(a + b + c, d);

            if (IsValidIPPart(p1) &&
                IsValidIPPart(p2) &&
                IsValidIPPart(p3) &&
                IsValidIPPart(p4))
            {
                string result = $"{p1}.{p2}.{p3}.{p4}";
                Debug.Log("[IP Formatter] Auto formatted → " + result);
                return result;
            }
        }

        return null; // No valid pattern found
    }

    private bool IsValidIPPart(string part)
    {
        if (part.Length == 0 || part.Length > 3) return false;
        if (part.StartsWith("0") && part.Length > 1) return false; // No leading zeros
        if (!int.TryParse(part, out int value)) return false;
        return value >= 0 && value <= 255;
    }
}
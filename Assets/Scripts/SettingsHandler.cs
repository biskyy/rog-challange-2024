using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsHandler : MonoBehaviour
{
  [SerializeField] private List<TMP_InputField> inputFields; // Assign multiple input fields in the Inspector

  // Dictionary to map input fields to their corresponding actions
  private Dictionary<TMP_InputField, Action<float>> inputFieldActions = new Dictionary<TMP_InputField, Action<float>>();

  private void Start()
  {
    foreach (var inputField in inputFields)
    {
      if (inputField != null)
      {
        inputField.onValueChanged.AddListener(input => ValidateAndSanitizeInput(inputField, input));
        inputField.onEndEdit.AddListener(input => HandleFinalizedInput(inputField, input));
      }
    }


    // Example: Assigning actions to specific input fields
    if (inputFields.Count > 0)
    {
      inputFields[0].text = Globals.Instance.sensitivityMultiplier.ToString();
      inputFields[1].text = Globals.Instance.musicVolume.ToString();
      inputFields[2].text = Globals.Instance.sfxVolume.ToString();
      AssignAction(inputFields[0], value => Globals.Instance.sensitivityMultiplier = value);
      AssignAction(inputFields[1], value => Globals.Instance.musicVolume = value);
      AssignAction(inputFields[2], value => Globals.Instance.sfxVolume = value);
    }
  }

  /// <summary>
  /// Assigns a specific action to an InputField.
  /// </summary>
  public void AssignAction(TMP_InputField inputField, Action<float> action)
  {
    if (inputField != null)
    {
      inputFieldActions[inputField] = action;
    }
  }

  /// <summary>
  /// Validates and sanitizes input in real-time for a specific InputField.
  /// </summary>
  private void ValidateAndSanitizeInput(TMP_InputField inputField, string input)
  {
    string sanitized = SanitizeInput(input);

    // Update the input field only if the sanitized value changes
    if (input != sanitized)
    {
      inputField.text = sanitized;
    }
  }

  /// <summary>
  /// Handles finalized input, performs the assigned action for a specific InputField.
  /// </summary>
  private void HandleFinalizedInput(TMP_InputField inputField, string input)
  {
    if (float.TryParse(input, out float result))
    {
      if (inputFieldActions.TryGetValue(inputField, out Action<float> action))
      {
        action.Invoke(result); // Perform the specific action for this input field
      }
    }
    else
    {
      Debug.LogWarning("Invalid float input");
      inputField.text = "0"; // Reset to a default value
    }
  }

  /// <summary>
  /// Sanitizes the input string, allowing only numeric characters, one decimal point, and optional leading '-' or '+'.
  /// </summary>
  private string SanitizeInput(string input)
  {
    if (string.IsNullOrEmpty(input))
      return string.Empty;

    string sanitized = string.Empty;
    bool hasDecimal = false;

    foreach (char c in input)
    {
      if (char.IsDigit(c))
      {
        sanitized += c;
      }
      else if (c == '.' && !hasDecimal)
      {
        sanitized += c;
        hasDecimal = true;
      }
      else if ((c == '-' || c == '+') && sanitized.Length == 0)
      {
        sanitized += c; // Allow sign only at the start
      }
    }

    return sanitized;
  }
}

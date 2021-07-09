using UnityEngine;

/// <summary>
/// Script for the option checkboxes
/// </summary>
public class CheckBoxScript : MonoBehaviour
{
    /// <summary>
    /// The name of the varriable of the field in the SimulationSettings
    /// </summary>
    public string Type;

    /// <summary>
    /// The checkbox which this script was binded
    /// </summary>
    public UnityEngine.UI.Toggle checkbox;

    //--------------------------------------------------------------------------------
    // Start is called before the first frame update
    private void Start()
    {
        checkbox.isOn = (bool)Options.checkBoxOptions[Type].GetValue(Options.checkBoxOptions[Type]);
    }

    //--------------------------------------------------------------------------------
    //Called when the checkbox is checked
    public void OnCheck(bool state)
    {
        Options.checkBoxOptions[Type].SetValue(Options.checkBoxOptions[Type], state);
    }
}
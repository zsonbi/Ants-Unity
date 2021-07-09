using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBoxScript : MonoBehaviour
{
    public string Type;
    public UnityEngine.UI.Toggle checkbox;

    private void Start()
    {
        checkbox.isOn = (bool)Options.checkBoxOptions[Type].GetValue(Options.checkBoxOptions[Type]);
    }

    public void OnCheck(bool state)
    {
        Options.checkBoxOptions[Type].SetValue(Options.checkBoxOptions[Type], state);
    }
}
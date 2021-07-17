using UnityEngine;
using AntSimulation;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// This object handles the options for the simulation
/// </summary>
public class Options : MonoBehaviour
{
    public static Dictionary<string, FieldInfo> sliderOptions = new Dictionary<string, FieldInfo>();
    public static Dictionary<string, FieldInfo> checkBoxOptions = new Dictionary<string, FieldInfo>();

    //Runs when the script is loaded
    private void Awake()
    {
        if (sliderOptions.Count == 0)
        {
            //Loads in the fields into dictionaries
            foreach (var field in typeof(SimulationOptions).GetRuntimeFields())
            {
                foreach (var customAttribute in field.CustomAttributes)
                {
                    if (customAttribute.AttributeType == typeof(SliderAttribute))
                    {
                        sliderOptions.Add(field.Name, field);
                    }
                    else if (customAttribute.AttributeType == typeof(CheckBoxAttribute))
                    {
                        checkBoxOptions.Add(field.Name, field);
                    }
                    else
                    {
                        System.Console.WriteLine("-----WARNING-----");
                        System.Console.WriteLine("No attribute set in simulation options this may lead to errors");
                    }
                }
            }
        }
        this.gameObject.SetActive(false);
    }

    //------------------------------------------------------------------------------------
    /// <summary>
    /// Deactivates the object
    /// </summary>
    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }

    //-------------------------------------------------------------------------------------
    /// <summary>
    /// Activates the object
    /// </summary>
    public void Activate()
    {
        this.gameObject.SetActive(true);
    }
}
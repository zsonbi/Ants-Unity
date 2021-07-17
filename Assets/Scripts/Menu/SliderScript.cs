using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    /// <summary>
    /// Gets the value of the slider
    /// </summary>
    public float sliderValue { get => this.GetComponentInParent<Slider>().value; }

    /// <summary>
    /// The slider which the script was binded
    /// </summary>
    public Text sliderLabelText;

    /// <summary>
    /// The name of the varriable of the field in the SimulationSettings
    /// </summary>
    public string optionType;

    // Start is called before the first frame update
    private void Start()
    {
        this.GetComponentInParent<Slider>().value = (float)System.Convert.ToDouble(Options.sliderOptions[optionType].GetValue(Options.sliderOptions[optionType]));

        sliderLabelText.text = sliderValue.ToString();
    }

    //-------------------------------------------------------------------
    //Called when the slider value is changed
    public void SliderOnChanged(float parameter)
    {
        sliderLabelText.text = parameter.ToString();
        Options.sliderOptions[optionType].SetValue(Options.sliderOptions[optionType], System.Convert.ChangeType(parameter, Options.sliderOptions[optionType].FieldType));
    }
}
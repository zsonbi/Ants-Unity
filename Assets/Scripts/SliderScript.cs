using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    public float sliderValue { get => this.GetComponentInParent<Slider>().value; }
    public Text sliderLabelText;
    public string optionType;

    // Start is called before the first frame update
    private void Start()
    {
        this.GetComponentInParent<Slider>().value = (float)System.Convert.ToDouble(Options.sliderOptions[optionType].GetValue(Options.sliderOptions[optionType]));

        sliderLabelText.text = sliderValue.ToString();
    }

    public void SliderOnChanged(float parameter)
    {
        sliderLabelText.text = parameter.ToString();
        Options.sliderOptions[optionType].SetValue(Options.sliderOptions[optionType], System.Convert.ChangeType(parameter, Options.sliderOptions[optionType].FieldType));
    }
}
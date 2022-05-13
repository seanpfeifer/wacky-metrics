using UnityEngine;
using TMPro;

public class MetricWaver : MonoBehaviour
{
  public TMP_Text textLabel;
  public TMP_Text textValue;
  public WackyWavingCloth cloth;

  // Update is called once per frame
  public void SetValue(long value)
  {
    textValue.text = value.ToString("N0");
    // TODO: Calculate this based on average or something
    // 2 is decent
    cloth.pulseAccel = ((float)value) / 500.0f;
    // 12 is decent
    cloth.randomForce = ((float)value) / 100.0f;
  }
}

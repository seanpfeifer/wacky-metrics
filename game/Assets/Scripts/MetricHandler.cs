using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MetricHandler : MonoBehaviour
{
  public TMP_Text indexLabel;
  public MetricWaver waverPrefab;
  private List<MetricWaver> wavers = new List<MetricWaver>();
  private MetricsLoader.Metrics metrics;
  public Transform spawnStart; // Where the wavers should start spawning
  public float spawnDeltaX = 1; // How much offset in the X direction should each waver spawn?
  public float spawnYRot = 180;

  // Start is called before the first frame update
  void Start()
  {
    metrics = MetricsLoader.ParseMetrics(sampleData);
    SpawnMetrics();
    SetDataIndex(0);
  }

  private void SpawnMetrics()
  {
    Vector3 deltaVector = new Vector3();
    for (int i = 0; i < metrics.Headers.Length; i++)
    {
      var oneWaver = Instantiate<MetricWaver>(waverPrefab, spawnStart.position + deltaVector, Quaternion.AngleAxis(spawnYRot, Vector3.up));
      oneWaver.textLabel.text = metrics.Headers[i];
      oneWaver.SetValue(metrics.DataPoints[0].Values[i]);
      wavers.Add(oneWaver);

      deltaVector.x += spawnDeltaX;
    }
  }

  public void SetDataIndex(int dataIndex)
  {
    indexLabel.text = metrics.DataPoints[dataIndex].Label;

    for (int col = 0; col < wavers.Count; col++)
    {
      wavers[col].SetValue(metrics.DataPoints[dataIndex].Values[col]);
    }
  }


  private const string sampleData = @"""Time"";""Metric A"";""Metric B"";""Metric C"";""Metric D""
""2022-05-11T20:11:00-07:00"";10;500;5,000;10,000
""2022-05-11T20:12:00-07:00"";20;700;7,500;15,000
";
}

using System; // For Action
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AOT; // For MonoPInvokeCallback
using System.Runtime.InteropServices; // For DllImport

public class MetricHandler : MonoBehaviour
{
  // This is a hacky solution to our file callback needing to use a static func
  private static MetricHandler instance;
  public GameObject[] hideOnFileOpen;
  public TMP_Text indexLabel;
  public TMP_Text dataCountLabel;
  public MetricWaver waverPrefab;
  private List<MetricWaver> wavers = new List<MetricWaver>();
  private static MetricsLoader.Metrics metrics;
  public Transform spawnStart; // Where the wavers should start spawning
  public float spawnDeltaX = 1; // How much offset in the X direction should each waver spawn?
  public float spawnYRot = 180;
  // The max index viewable
  public int maxDataIndex
  {
    get
    {
      return metrics.DataPoints.Count - 1;
    }
  }

  [DllImport("__Internal")]
  private static extern void LoadFileFromBrowser(Action<string> fileCallback);

  // Start is called before the first frame update
  void Start()
  {
    instance = this;
    metrics = new MetricsLoader.Metrics("", new List<MetricsLoader.MetricColumn>());
  }

  // This callback needs to be public + static
  [MonoPInvokeCallback(typeof(Action<string>))]
  public static void LoadData(string data)
  {
    metrics = MetricsLoader.ParseMetrics(data);
    instance.SpawnMetrics();
    instance.SetDataIndex(0);
  }

  public void OnClickLoadFile()
  {
    LoadFileFromBrowser(LoadData);
  }

  public void OnClickLoadSample()
  {
    LoadData(sampleData);
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

    dataCountLabel.text = metrics.DataPoints.Count.ToString("N0");
    foreach (var obj in hideOnFileOpen)
    {
      obj.SetActive(false);
    }
  }

  public void SetDataIndex(int dataIndex)
  {
    dataIndex = Mathf.Clamp(dataIndex, 0, maxDataIndex);
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

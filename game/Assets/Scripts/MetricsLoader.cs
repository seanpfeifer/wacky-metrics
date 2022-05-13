using System;
using System.Collections.Generic;

/*
Format is
```
"Time";"Metric A";"Metric B";"Metric C";"Metric D"
"2022-05-11T20:11:00-07:00";500;4,500;450;1,500
"2022-05-11T20:12:00-07:00";1,500;5,500;1,100;3,005
```
*/
public class MetricsLoader
{
  private static readonly string[] NEWLINE_SEPARATORS = new string[] { "\r\n", "\r", "\n" };

  public struct MetricColumn
  {
    public readonly string Label;
    public readonly long[] Values;
    public MetricColumn(string line)
    {
      string[] parts = line.Split(';');
      Label = parts[0].Trim('"');
      Values = new long[parts.Length - 1];
      for (int i = 1; i < parts.Length; i++)
      {
        // Need to subtract 1 to get back to zero index for Values assignment
        Values[i - 1] = long.Parse(parts[i], System.Globalization.NumberStyles.AllowThousands);
      }
    }
  }

  public struct Metrics
  {
    public readonly string[] Headers;
    public readonly List<MetricColumn> DataPoints;
    public Metrics(string headerLine, List<MetricColumn> dataPoints)
    {
      DataPoints = dataPoints;

      string[] parts = headerLine.Split(';');
      // Ignore parts[0], as it just says "Time"
      Headers = new string[parts.Length - 1];
      for (int i = 1; i < parts.Length; i++)
      {
        // Need to subtract 1 to get back to zero index for assignment
        Headers[i - 1] = parts[i].Trim('"');
      }
    }
  }

  public static Metrics ParseMetrics(string data)
  {
    List<MetricColumn> values = new List<MetricColumn>();
    var lines = data.Split(NEWLINE_SEPARATORS, StringSplitOptions.RemoveEmptyEntries);

    // lines[0] will be used as a header, passed into Metrics(), so skip it here
    for (int i = 1; i < lines.Length; i++)
    {
      values.Add(new MetricColumn(lines[i]));
    }

    return new Metrics(lines[0], values);
  }
}

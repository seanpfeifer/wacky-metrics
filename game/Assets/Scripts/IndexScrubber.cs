using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IndexScrubber : MonoBehaviour
{
  public MetricHandler metricHandler;
  public RawImage scrubImage;
  private int prevIndex = 0;

  public void OnDrag(BaseEventData data)
  {
    PointerEventData pointerData = (PointerEventData)data;

    // Calculate where we're scrubbing to based on the number of indexes and the
    // actual width of the scrubImage.
    // Add one to the maxDataIndex so we're using a count of the number of values.
    // eg, for 2 values, and width of 100: 100/2  (NOT 100/1)
    var pixelsPerIndex = scrubImage.rectTransform.rect.width / (metricHandler.maxDataIndex + 1);
    var selectedIndex = (int)(pointerData.position.x / pixelsPerIndex);

    if (selectedIndex != prevIndex)
    {
      prevIndex = selectedIndex;
      metricHandler.SetDataIndex(selectedIndex);
    }
  }
}

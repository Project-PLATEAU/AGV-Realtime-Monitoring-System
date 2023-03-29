/*         INFINITY CODE         */
/*   https://infinity-code.com   */

using UnityEngine;

public class CreateMarkerOnClick:MonoBehaviour
{
    private void Start()
    {
        // Subscribe to the click event.
        OnlineMapsControlBase.instance.OnMapClick += OnMapClick;
    }

    private void OnMapClick()
    {
        // Get the coordinates under the cursor.
        double lng, lat;
        OnlineMapsControlBase.instance.GetCoords(out lng, out lat);

        // Create a label for the marker.
        string label = "Marker " + (OnlineMapsMarkerManager.CountItems + 1);

        // Create a new marker.
        OnlineMapsMarkerManager.CreateItem(lng, lat, label);
    }
}
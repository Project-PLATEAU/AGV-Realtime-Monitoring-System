/*         INFINITY CODE         */
/*   https://infinity-code.com   */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CreateMarkerNLineOnClick:MonoBehaviour
{
    private List<OnlineMapsMarker3D> WayPointList;
    public GameObject WayPointMarkerPrefab;

    LineRenderer linerend;

    private Button WayPointGenerateBtn;

    private bool isMapExpanded = false;

    public Camera cam;

    private void Start()
    {
        // Subscribe to the click event.
        // OnlineMapsControlBase.instance.OnMapClick += OnMapClick;
        WayPointList = new List<OnlineMapsMarker3D>();

        GameObject btn = GameObject.Find("Btn_LeftMenu_GenerateWayPoints");

        WayPointGenerateBtn = btn.GetComponent<Button>();


        WayPointGenerateBtn.onClick.AddListener(OnWayPointGenerateClick);
        linerend = gameObject.AddComponent<LineRenderer>();
    }

    private void OnWayPointGenerateClick(){
        
        isMapExpanded = !isMapExpanded;

        if(isMapExpanded) {
            cam.rect = new Rect(0, 0, 1f, 1f);
        } else {
            cam.rect = new Rect(0, 0, 0.4f, 0.4f);
        }
    }

    private void OnMapClick()
    {
        // Get the coordinates under the cursor.
        double lng, lat;
        OnlineMapsControlBase.instance.GetCoords(out lng, out lat);

        // Create a label for the marker.
        string label = "Marker " + (OnlineMapsMarkerManager.CountItems + 1);

        // Create a new marker.
        OnlineMapsMarker3D obj = OnlineMapsMarker3DManager.CreateItem(lng, lat, WayPointMarkerPrefab);
        obj.altitude = 1.5f;    

        WayPointList.Add(obj);
    }

    private void Update() {
        linerend.positionCount = WayPointList.Count;
        Vector3[] positions = new Vector3[100];
        for(var i = 0; i < WayPointList.Count; i++) {
            Vector3 pos = new Vector3(WayPointList[i].transform.position.x, WayPointList[i].transform.position.y, WayPointList[i].transform.position.z);
            positions[i] = pos;
        }
        linerend.material = new Material(Shader.Find("Sprites/Default"));
        linerend.startColor = Color.green;
        linerend.endColor = Color.green;
        linerend.SetPositions(positions);
    }


}
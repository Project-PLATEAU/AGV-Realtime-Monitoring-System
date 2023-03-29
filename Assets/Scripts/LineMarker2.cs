using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMarker2 : MonoBehaviour
{
    private OnlineMaps map;

    private OnlineMapsMarker3D marker1;
    private OnlineMapsMarker3D marker2;
    private OnlineMapsMarker3D marker3;
    private OnlineMapsMarker3D marker4;
    private OnlineMapsMarker3D marker5;
    private OnlineMapsMarker3D marker6;
    private OnlineMapsMarker3D marker7;
    private OnlineMapsMarker3D marker8;

    private OnlineMapsMarker WaypointMarker1;
    private OnlineMapsMarker WaypointMarker2;
    private OnlineMapsMarker WaypointMarker3;
    private OnlineMapsMarker WaypointMarker4;

    public GameObject targetObject;

    LineRenderer linerend;

    private List<OnlineMapsMarker3D> WayPointList;

    private float markerSize = 1.0f;

    public MQTTManager mqttManager;

    private OnlineMapsMarker GoalMarker2;

    void Start()
    {
        WayPointList = new List<OnlineMapsMarker3D>();

        map = OnlineMaps.instance;

        marker1 = OnlineMapsMarker3DManager.CreateItem(135.397658, 34.666935, targetObject);
        marker1.scale = markerSize;
        marker1.altitude = 1.5f;
        WayPointList.Add(marker1);
        WaypointMarker1 = OnlineMapsMarkerManager.CreateItem(135.397658, 34.666935, "WayPoint2-1");
        WaypointMarker1.OnClick += OnWaypoint1Click;

        marker2 = OnlineMapsMarker3DManager.CreateItem(135.395103, 34.666733, targetObject);
        marker2.scale = markerSize;
        marker2.altitude = 2.5f;
        WayPointList.Add(marker2);
        WaypointMarker2 = OnlineMapsMarkerManager.CreateItem(135.395103, 34.666733, "WayPoint2-2");
        WaypointMarker2.OnClick += OnWaypoint2Click;

        marker3 = OnlineMapsMarker3DManager.CreateItem(135.393510, 34.666604, targetObject);
        marker3.scale = markerSize;
        marker3.altitude = 6f;
        WayPointList.Add(marker3);
        WaypointMarker3 = OnlineMapsMarkerManager.CreateItem(135.393510, 34.666604, "WayPoint2-3");
        WaypointMarker3.OnClick += OnWaypoint3Click;

        marker4 = OnlineMapsMarker3DManager.CreateItem(135.392123, 34.666492, targetObject);
        marker4.scale = markerSize;
        marker4.altitude = 9f;
        WayPointList.Add(marker4);

        GoalMarker2 = OnlineMapsMarkerManager.CreateItem(135.392123, 34.666492, "Goal2");
        GoalMarker2.OnClick += OnGoalMarker2Click;


        linerend = gameObject.AddComponent<LineRenderer>();
    }

    private void OnWaypoint1Click(OnlineMapsMarkerBase marker) {
        mqttManager.SendInitPose(-671.288f, 12.270f, 0.0f, 0.0f, 0.0f, -0.073f, 0.997f);
    }

    private void OnWaypoint2Click(OnlineMapsMarkerBase marker) {
        mqttManager.SendInitPose(-442.490f, -37.242f, 0.0f, 0.0f, 0.0f, -0.093f, 0.9956f);
    }

    private void OnWaypoint3Click(OnlineMapsMarkerBase marker) {
        mqttManager.SendInitPose(-297.935f, -67.984f, 0.0f, 0.0f, 0.0f, -0.109f, 0.993f);
    }

    private void OnGoalMarker2Click(OnlineMapsMarkerBase marker) {
        mqttManager.SendGoalPose(-32.366165f, -123.97429f, 0.0f, 0.0f, 0.0f, -0.0826f, 0.9965f);
    }



    // Update is called once per frame
    void Update()
    {
        linerend.positionCount = WayPointList.Count;
        Vector3[] positions = new Vector3[100];
        for(var i = 0; i < WayPointList.Count; i++) {
            Vector3 pos = new Vector3(WayPointList[i].transform.position.x, WayPointList[i].transform.position.y, WayPointList[i].transform.position.z);
            positions[i] = pos;
        }
        linerend.material = new Material(Shader.Find("Sprites/Default"));
        Color greenAlpha = Color.green;
        greenAlpha.a = 0.4f;
        linerend.startColor = greenAlpha;
        linerend.endColor = greenAlpha;
        linerend.SetPositions(positions);
    }
}

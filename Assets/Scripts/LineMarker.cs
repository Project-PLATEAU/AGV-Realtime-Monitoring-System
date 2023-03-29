using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMarker : MonoBehaviour
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

    public MQTTManager mqttManager;

    private OnlineMapsMarker WaypointMarker1;
    private OnlineMapsMarker WaypointMarker2;
    private OnlineMapsMarker WaypointMarker3;
    private OnlineMapsMarker WaypointMarker4;
    private OnlineMapsMarker WaypointMarker5;
    private OnlineMapsMarker GoalMarker1;

    public GameObject targetObject;

    LineRenderer linerend;

    private List<OnlineMapsMarker3D> WayPointList;

    private float markerSize = 1.0f;

    void Start()
    {
        WayPointList = new List<OnlineMapsMarker3D>();

        map = OnlineMaps.instance;

        marker1 = OnlineMapsMarker3DManager.CreateItem(135.394764, 34.664135, targetObject);
        marker1.scale = markerSize;
        marker1.altitude = 1f;
        WayPointList.Add(marker1);
        WaypointMarker1 = OnlineMapsMarkerManager.CreateItem(135.394764, 34.664135, "WayPoint1-1");
        WaypointMarker1.OnClick += OnWaypoint1Click;

        marker2 = OnlineMapsMarker3DManager.CreateItem(135.395246, 34.664275, targetObject);
        marker2.scale = markerSize;
        marker2.altitude = 1.5f;
        WayPointList.Add(marker2);
        WaypointMarker2 = OnlineMapsMarkerManager.CreateItem(135.395246, 34.664275, "WayPoint1-2");
        WaypointMarker2.OnClick += OnWaypoint2Click;

        marker3 = OnlineMapsMarker3DManager.CreateItem(135.395629, 34.664334, targetObject);
        marker3.scale = markerSize;
        marker3.altitude = 2.5f;
        WayPointList.Add(marker3);
        WaypointMarker3 = OnlineMapsMarkerManager.CreateItem(135.395629, 34.664334, "WayPoint1-3");
        WaypointMarker3.OnClick += OnWaypoint3Click;

        marker4 = OnlineMapsMarker3DManager.CreateItem(135.395871, 34.664332, targetObject);
        marker4.scale = markerSize;
        marker4.altitude = 2.5f;
        WayPointList.Add(marker4);
        WaypointMarker4 = OnlineMapsMarkerManager.CreateItem(135.395871, 34.664332, "WayPoint1-4");
        WaypointMarker4.OnClick += OnWaypoint4Click;

        marker5 = OnlineMapsMarker3DManager.CreateItem(135.396600, 34.664237, targetObject);
        marker5.scale = markerSize;
        marker5.altitude = 3f;
        WayPointList.Add(marker5);
        WaypointMarker5 = OnlineMapsMarkerManager.CreateItem(135.396600, 34.664237, "WayPoint1-5");
        WaypointMarker5.OnClick += OnWaypoint5Click;

        marker6 = OnlineMapsMarker3DManager.CreateItem(135.397924, 34.664031, targetObject);
        marker6.scale = markerSize;
        marker6.altitude = 3f;
        WayPointList.Add(marker6);
        GoalMarker1 = OnlineMapsMarkerManager.CreateItem(135.397924, 34.664031, "Goal1");
        GoalMarker1.OnClick += OnGoal1Click;


        linerend = gameObject.AddComponent<LineRenderer>();
    }

    private void OnWaypoint1Click(OnlineMapsMarkerBase marker) {
        mqttManager.SendInitPose(-320.689f, 231.272f, 0.0f, 0.0f, 0.0f, -0.999f, 0.008f);
    }

    private void OnWaypoint2Click(OnlineMapsMarkerBase marker) {
        mqttManager.SendInitPose(-369.413f, 230.417f, 0.0f, 0.0f, 0.0f, -0.999f, 0.007f);
    }

    private void OnWaypoint3Click(OnlineMapsMarkerBase marker) {
        mqttManager.SendInitPose(-400.148f, 232.606f, 0.0f, 0.0f, 0.0f, 0.997f, 0.075f);
    }

    private void OnWaypoint4Click(OnlineMapsMarkerBase marker) {
        mqttManager.SendInitPose(-400.148f, 232.606f, 0.0f, 0.0f, 0.0f, 0.997f, 0.075f);
    }

    private void OnWaypoint5Click(OnlineMapsMarkerBase marker) {
        mqttManager.SendInitPose(-400.148f, 232.606f, 0.0f, 0.0f, 0.0f, 0.997f, 0.075f);
    }

    private void OnGoal1Click(OnlineMapsMarkerBase marker) {
        mqttManager.SendGoalPose(-484.418f, 266.882f, 0.0f, 0.0f, 0.0f, 0.973f, 0.228f);
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

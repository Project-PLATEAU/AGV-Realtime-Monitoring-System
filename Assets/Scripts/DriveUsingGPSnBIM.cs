/*         INFINITY CODE         */
/*   https://infinity-code.com   */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace InfinityCode.OnlineMapsDemos
{
    [AddComponentMenu("Infinity Code/Online Maps/Demos/DriveUsingGPS")]
    public class DriveUsingGPSnBIM : MonoBehaviour
    {
        public GameObject prefab;
        public GameObject CamPrefab;

        public GameObject AdjustSpherePrefab;
        public GameObject OriginPrefab;
        public GameObject TrajectoryPrefab;

        public float markerScale = 5f;
        public float speed;
        public float maxSpeed = 160;
        public float rotation;
        public bool rotateCamera = true;
        public bool centerOnMarker = true;

        private OnlineMaps map;
        private OnlineMapsMarker3D marker;
        private OnlineMapsTileSetControl control;

        private double lng, lat;

        public MQTTManager mqttManager;

        public GameObject BimPrefab;
        private OnlineMapsMarker3D bimMarker;
        private OnlineMapsMarker3D CamMarker;
        private OnlineMapsMarker3D signMarker;
        private OnlineMapsMarker3D originMarker;
        private OnlineMapsMarker3D trajectoryMarker;
        private OnlineMapsMarker3D lastTrajectoryPoint;

        private double bimLat = 34.667060;
        private double bimLng = 135.399104;
        public float bimScale;
        public float bimRotation;

        private double originLat = 34.667060;
        private double originLng = 135.399104;

        private double signLat = 34.665183;
        private double signLng = 135.390996;

        public List<double[]> latLngList = new List<double[]>();
        private int LatLngListMaxCount = 10;

        private float span = 4f;
        private float currentTime = 0f;

        private float carRotationY = 0f;

        private void Start()
        {
            map = OnlineMaps.instance;
            control = OnlineMapsTileSetControl.instance;

            // control.OnMapClick += OnMapClick;

            map.GetPosition(out lng, out lat);

            marker = OnlineMapsMarker3DManager.CreateItem(lng, lat, prefab);
            // marker = OnlineMapsMarker3DManager.CreateFromExistGameObject(lng, lat, carObject);

            marker.scale = markerScale;
            // marker.rotationY = rotation;

            CamMarker = OnlineMapsMarker3DManager.CreateItem(lng, lat, CamPrefab);

            lastTrajectoryPoint = OnlineMapsMarker3DManager.CreateItem(lng, lat, TrajectoryPrefab);

            bimMarker = OnlineMapsMarker3DManager.CreateItem(bimLng, bimLat, BimPrefab);
            bimMarker.scale = bimScale;
            bimMarker.rotationY = bimRotation;

            originMarker = OnlineMapsMarker3DManager.CreateItem(originLng, originLat, OriginPrefab);
            originMarker.scale = 3;

            signMarker = OnlineMapsMarker3DManager.CreateItem(signLng, signLat, AdjustSpherePrefab);
            signMarker.scale = 3;
        }

        private void OnMapClick()
        {
            control.GetCoords(out bimLng, out bimLat);

            bimMarker.SetPosition(bimLng, bimLat);
            bimMarker.scale = bimScale;
            bimMarker.rotationY = bimRotation;
        }

        private void Update()
        {
            double mqttLat = mqttManager.latitude;
            double mqttLng = mqttManager.longitude;

            Vector3 carVec = (marker.transform.position - lastTrajectoryPoint.transform.position).normalized;
            float angle = Mathf.Atan2(carVec.z, carVec.x) * Mathf.Rad2Deg;;
            if(angle != 0){
                carRotationY = angle;
            }
            marker.rotationY = carRotationY * -1;

            marker.SetPosition(mqttLng, mqttLat);
            CamMarker.SetPosition(mqttLng, mqttLat);

            // marker.rotationY = mqttManager.rotation;
            marker.altitude = (float)(mqttManager.altitude - 37.27 - 5);

            if (centerOnMarker) map.SetPosition(mqttLng, mqttLat);



            

            currentTime += Time.deltaTime;

            if(currentTime > span){
                currentTime = 0f;
                lastTrajectoryPoint.SetPosition(mqttLng, mqttLat);
                lastTrajectoryPoint.altitude = (float)(mqttManager.altitude - 37.27 - 5);
                lastTrajectoryPoint.scale = 0.2f;

                // history points
                trajectoryMarker = OnlineMapsMarker3DManager.CreateItem(mqttLng, mqttLat, TrajectoryPrefab);
                trajectoryMarker.scale = 0.1f;
                trajectoryMarker.altitude = marker.altitude;
            }
        }
    }
}
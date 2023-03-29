using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Text;

using MQTTnet; 
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Receiving;
using MQTTnet.Client.Disconnecting;

using System.Threading;
using System.Threading.Tasks;

public class MQTTManager : MonoBehaviour
{
    IMqttClient mqttClient; 
    public float waitSecondTime = 1f; 
    public string signalingUrl = "test.mosquitto.org";

    public double latitude;
    public double longitude;
    public double altitude;

    public float rotation;

    // UI
    public Text EcuModeText;
    public Text BatteryText;
    public Text SpeedText;
    public Text SteeringText;
    public Text ShowSpeedLimitText;
    public Text PreSpeedLimitText;
    public Button SendSpeedBtn;
    public Button SpeedLimitUpBtn;
    public Button SpeedLimitDownBtn;
    public Button EngageBtn;
    public Button GoalPoseBtn;
    
    public Text TpText;

    public int targetSteering = 0;

    // Kentora data
    public int batteryLevel;
    public float longitudinal_velocity;
    public float kmh; // km/h
    public int ecuMode;
    public float steering;
    public float steeringPercentage;

    public int adjustAngle = 0;

    private int preSpeedLimit = 0;
    private int sentSpeedLimit = 0;
    private float transformProbability = 0.0f;


    // Start is called before the first frame update
    async void Start()
    {
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(signalingUrl)
            .Build(); 

        var factory = new MqttFactory(); 
        mqttClient = factory.CreateMqttClient(); 

        mqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(OnAppMessage);
        mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnConnected);
        mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnDisconnected);

        await mqttClient.ConnectAsync(options);

        SendSpeedBtn.onClick.AddListener(OnSendBtnClick);
        SpeedLimitUpBtn.onClick.AddListener(OnUpBtnClick);
        SpeedLimitDownBtn.onClick.AddListener(OnDownBtnClick);
        EngageBtn.onClick.AddListener(OnEngageBtnClick);
        GoalPoseBtn.onClick.AddListener(OnGoalPoseClick);
    }

    // Update is called once per frame
    void Update()
    {
        if(ecuMode == 1) EcuModeText.text = "自動運転";
        if(ecuMode == 2) EcuModeText.text = "手動運転";
        if(ecuMode == 5) EcuModeText.text = "緊急停止";
        if(ecuMode == 0) EcuModeText.text = "スタンドバイ";

        BatteryText.text = "バッテリー : " + batteryLevel + "%";

        SpeedText.text = kmh.ToString("N1") + " km/h";

        SteeringText.text = steeringPercentage.ToString("F1") + " deg";
        ShowSpeedLimitText.text = "速度制限 " + sentSpeedLimit.ToString() + " km/h";
        PreSpeedLimitText.text = "送信 " + preSpeedLimit + " km/h";

        TpText.text = "TP: " + transformProbability.ToString("F3");
    }

    private async void OnSendBtnClick(){
        sentSpeedLimit = preSpeedLimit;

        VelocityLimit msg = new VelocityLimit();
        msg.max_velocity = sentSpeedLimit / 3.6f;
        msg.use_constraints = false;

        var payload = JsonUtility.ToJson(msg);

        var message = new MqttApplicationMessageBuilder()
           .WithTopic("/planning/scenario_planning/max_velocity_default")
           .WithPayload(payload)
           .WithExactlyOnceQoS()
           .Build();

       await mqttClient.PublishAsync(message, CancellationToken.None);
    }

    private void OnUpBtnClick() {
        preSpeedLimit += 1;
    }

    private void OnDownBtnClick() {
        preSpeedLimit -= 1;
    }

    private async void OnEngageBtnClick() {
        Engage msg = new Engage();
        msg.engage = true;

        var payload = JsonUtility.ToJson(msg);

        var message = new MqttApplicationMessageBuilder()
           .WithTopic("/autoware/engage")
           .WithPayload(payload)
           .WithExactlyOnceQoS()
           .Build();

       await mqttClient.PublishAsync(message, CancellationToken.None);
    }

    private void OnGoalPoseClick() {
        SendGoalPose(-32.366165f, -123.97429f, 0.0f, 0.0f, 0.0f, -0.0826f, 0.9965f);
    }

    public async void SendGoalPose(float x, float y, float z, float qx, float qy, float qz, float qw) {
        Debug.Log("Send Goal Pose");
        PoseStamped msg = new PoseStamped();
        Header h = new Header();
        h.frame_id = "map";

        msg.header = h;


        Pose pose = new Pose();
        Point point = new Point();
        point.x = x;
        point.y = y;
        point.z = z;

        Quaternions quaternion = new Quaternions();
        quaternion.x = qx;
        quaternion.y = qy;
        quaternion.z = qz;
        quaternion.w = qw;

        pose.position = point;
        pose.orientation = quaternion;
        msg.pose = pose;

        var payload = JsonUtility.ToJson(msg);

        var message = new MqttApplicationMessageBuilder()
           .WithTopic("/planning/mission_planning/goal")
           .WithPayload(payload)
           .WithExactlyOnceQoS()
           .Build();

        await mqttClient.PublishAsync(message, CancellationToken.None);
    }

    public async void SendInitPose(float x, float y, float z, float qx, float qy, float qz, float qw) {
        Debug.Log("Send Init Pose");
        PoseWithCovarianceStamped msg = new PoseWithCovarianceStamped();
        Header h = new Header();
        h.frame_id = "map";

        msg.header = h;


        Pose pose = new Pose();
        Point point = new Point();
        point.x = x;
        point.y = y;
        point.z = z;

        Quaternions quaternion = new Quaternions();
        quaternion.x = qx;
        quaternion.y = qy;
        quaternion.z = qz;
        quaternion.w = qw;

        pose.position = point;
        pose.orientation = quaternion;

        PoseWithCovariance poseWithCov = new PoseWithCovariance();
        poseWithCov.pose = pose;
        float[] arr = {0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f};
        poseWithCov.covariance = arr;
        msg.pose = poseWithCov;

        var payload = JsonUtility.ToJson(msg);

        var message = new MqttApplicationMessageBuilder()
           .WithTopic("/initialpose")
           .WithPayload(payload)
           .WithExactlyOnceQoS()
           .Build();

        await mqttClient.PublishAsync(message, CancellationToken.None);
    }

    private async void OnConnected(MqttClientConnectedEventArgs e) {
        Debug.Log("MQTT broker connected");
        await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("fix").Build());
        await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("Imu").Build());
        await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("MagneticField").Build());
        await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("Angle").Build());
        await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("BatteryLevel").Build());
        await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("/vehicle/status/velocity_status").Build());
        await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("EcuMode").Build());
        await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("/vehicle/status/steering_status").Build());
        await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("/localization/pose_estimator/transform_probability").Build());

        Debug.Log("TOPIC Subscribed");
   }

    private void OnAppMessage(MqttApplicationMessageReceivedEventArgs e) {
        string topic = e.ApplicationMessage.Topic;
        
        string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
        if(topic == "fix") {
            NMEA_DATA data = JsonUtility.FromJson<NMEA_DATA>(payload);
            latitude = data.latitude;
            longitude = data.longitude;
            altitude = data.altitude;
            // Debug.Log(altitude);
        }
        if(topic == "Angle") {
            Vector3 data = JsonUtility.FromJson<Vector3>(payload);
            rotation = data.z * -1 + adjustAngle;
        }
        if(topic == "BatteryLevel") {
            Int8 message = JsonUtility.FromJson<Int8>(payload);
            batteryLevel = message.data;
        }
        if(topic == "/vehicle/status/velocity_status") {
            VelocityReport message = JsonUtility.FromJson<VelocityReport>(payload);
            longitudinal_velocity = message.longitudinal_velocity;
            kmh = (float)(longitudinal_velocity * 3.6);
        }
        if(topic == "EcuMode") {
            Int8 message = JsonUtility.FromJson<Int8>(payload);
            ecuMode = message.data;
        }
        if(topic == "/vehicle/status/steering_status") {
            SteeringReport message = JsonUtility.FromJson<SteeringReport>(payload);
            steering = Mathf.Round(message.steering_tire_angle * 1000f) / 1000f;
            steeringPercentage = (float)(steering * 180 / Math.PI);
        }

        if(topic == "/localization/pose_estimator/transform_probability") {
            Float32Stamped message = JsonUtility.FromJson<Float32Stamped>(payload);
            transformProbability = Mathf.Round(message.data * 1000f) / 1000f;
        }
   }

   private async void OnDisconnected(MqttClientDisconnectedEventArgs e) {

       if (mqttClient == null) {
           Debug.Log("MQTT Client is disposed");
           return;
       } else {
           Debug.Log("接続に失敗しました。5秒後に再接続を試みます");
       }

       await Task.Delay(TimeSpan.FromSeconds(5));

       var options = new MqttClientOptionsBuilder()
            .WithTcpServer(signalingUrl)
            .Build(); 

       try {
           Debug.Log("接続を開始します");
           await mqttClient.ConnectAsync(options);
       } catch {
       
       }
   }

   private void OnDestroy() {
       Debug.Log("OnDestroy");
       mqttClient.Dispose();
       mqttClient = null;// 意図的な切断
   }
}

public class NMEA_DATA {
    public string stamp;
    public double latitude;
    public double longitude;
    public double altitude;
}

[Serializable]
public class Header {
    public Stamp stamp;
    public string frame_id;
}

[Serializable]
public class Stamp {
    public int sec;
    public int nanosec;
    public string frame_id;
}

public class Int8 {
    public int data;
}

public class Int16 {
    public int data;
}

public class VelocityReport {
    public Header header;
    public float longitudinal_velocity;
    public float lateral_velocity;
    public float heading_rate;
}

public class SteeringReport {
    public Stamp stamp;
    public float steering_tire_angle;
}

public class VelocityLimit {
    public Stamp stamp;
    public float max_velocity;
    public bool use_constraints;
    public VelocityLimitConstraints constraints;
    public string sender;
}

public class VelocityLimitConstraints {
    public float min_acceleration;
    public float max_jerk;
    public float min_jerk;
}

public class Engage {
    public Stamp stamp;
    public bool engage;
}

[Serializable]
public class PoseStamped {
    public Header header;
    public Pose pose;
}

[Serializable]
public class Pose {
    public Point position;
    public Quaternions orientation;
}

[Serializable]
public class Point {
    public float x;
    public float y;
    public float z;
}

[Serializable]
public class Quaternions {
    public float x;
    public float y;
    public float z;
    public float w;
}

public class Float32Stamped {
    public Stamp stamp;
    public float data;
}

[Serializable]
public class PoseWithCovarianceStamped {
    public Header header;
    public PoseWithCovariance pose;
}

[Serializable]
public class PoseWithCovariance {
    public Pose pose;
    public float[] covariance;
}
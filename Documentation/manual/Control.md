# 車両制御機能について
- 本システムでは、ROSからUnityへメッセージを送信することで、車両のデータを表示しております。
- また、UnityからROSへメッセージを送信することで、車両の制御をしています。（遠隔操作機能はなし）

### 両脇のテキスト表示、ボタン機能について説明します
- ボタン機能は、実際の車両のROSへメッセージを送っています。サンプルデータ受信の場合には動作しません。

![](../resources/unity/ui-buttons.png)  

### テキスト表示部分のソースコード
各UIのテキスト表示のソースコードはこちらに記載しています。

`MQTTManager.cs`
```cs
...
  // UI
  public Text EcuModeText;
  public Text BatteryText;
  public Text SpeedText;
  public Text SteeringText;
  public Text ShowSpeedLimitText;
  public Text PreSpeedLimitText;

...

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
```

## ボタン部分のソースコード
各UIのボタン表示のソースコードはこちらに記載しています。
- ボタン機能は、実際の車両のROSへメッセージを送っています。車両と繋ぎ込む際には、適したトピック、型に変更をしてください。

`MQTTManager.cs`

```cs
...
  public Button SendSpeedBtn;
  public Button SpeedLimitUpBtn;
  public Button SpeedLimitDownBtn;
  public Button EngageBtn;
  public Button GoalPoseBtn;

...
  private void OnSendBtnClick() {...}
  private void OnUpBtnClick() {...}
  private void OnDownBtnClick() {...}
  private void OnEngageBtnClick() {...}
```
# 起動手順（サンプルサーバーを利用）

## シーン起動手順

1. Windows PCに本リポジトリをクローンします
2. UnityHubにて本プロジェクトを起動します
![](../resources/unity/unityhub.png)  

3. Assets/Scenes/CarDriveUsingRTKnGML.unity を開きます
![](../resources/unity/openscene.png)  

4. Unityの再生ボタンを押します
![](../resources/unity/unity-scene-capture.png)  

## サンプルサーバーからのデータ受信

1. インスペクタからMapを選択し、MQTT Managerの項目を確認します。
![](../resources/unity/mqttmanager.png)  

2. サンプルサーバーは `test.mosquitto.org`を利用しています。

3. 再生ボタンを押すと、下記のフローでデータを受信します
    1. ADAWARPのサーバーから `test.mosquitto.org` にサンプルデータを送信
        - サンプルデータはリポジトリに配置しております。[（リンクはこちら）](../resources/rosbag.zip)
        - サンプルデータはROS2で取得したROSBAGファイルとなっています。

    2. UnityのMQTT Managerが `test.mosquitto.org` からサンプルデータを受信
        - ソースコードは `Assets/Scripts/MQTTManager.cs`に配置
    3. Unityの画面上に情報を反映

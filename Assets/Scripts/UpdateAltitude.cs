using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateAltitude : MonoBehaviour
{
    public MQTTManager mqttManager;
    public float altitudeFromMqtt;

    // Start is called before the first frame update
    void Start()
    {
        mqttManager = GameObject.Find("Map").GetComponent<MQTTManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // altitudeFromMqtt = (float)mqttManager.altitude;
        // Transform myTransform = this.transform;

        // Vector3 pos = myTransform.position;
        // pos.y = altitudeFromMqtt / 10;
        // pos.y = 10;

        // myTransform.position = pos;
    }
}

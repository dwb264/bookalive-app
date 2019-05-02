using UnityEngine;
using System.Collections;

public class ScalePingPong : MonoBehaviour
{
    void Update()
    {
        transform.localScale = new Vector3(1 + Mathf.PingPong(Time.time / 5, 0.2f), 1 + Mathf.PingPong(Time.time / 5, 0.2f), transform.localScale.z);
    }
}

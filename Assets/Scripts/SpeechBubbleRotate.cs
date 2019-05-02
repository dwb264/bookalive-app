using UnityEngine;
using System.Collections;

public class SpeechBubbleRotate : MonoBehaviour
{
    Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        Debug.Log("Inside Start of SpeechBubbleRotate");
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
    }
}
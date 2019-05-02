using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCar : MonoBehaviour
{

    GameObject arrow;
    GameObject car;
    GameObject imageTarget;

    // Start is called before the first frame update
    void Start()
    {
        arrow = GameObject.Find("Arrow");
        imageTarget = GameObject.Find("ImageTarget");
        car = GameObject.Find("Car");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            //car = Instantiate(Resources.Load("Car", typeof(GameObject))) as GameObject;
            //car.transform.parent = imageTarget.transform;

            car.transform.Translate(0, 0, 5);
            Rigidbody carBody = car.GetComponent<Rigidbody>();

            //float xVelocity = arrow.transform.rotation.x * -50;
            //carBody.velocity = new Vector3(xVelocity, 6, 5);
            carBody.isKinematic = false;
        }
        /*
        if (car.transform.position.y <= imageTarget.transform.position.y)
        {
            car.GetComponent<Rigidbody>().useGravity = false;
            car.GetComponent<Rigidbody>().isKinematic = true;
        }*/

    }

    //Detect when there is a collision starting
    void OnCollisionEnter(Collision collision)
    {
        //Ouput the Collision to the console
        Debug.Log("Collision : " + collision.gameObject.name);
    }

}

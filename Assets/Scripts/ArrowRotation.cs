using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ArrowRotation : MonoBehaviour
{

    GameObject arrow;
    enum State { Ready, CarMoving }
    State currentState = State.Ready;
    float startTime;
    GameObject car;
    Vector3 initPos;

    public int ratio_left = 0;
    public int ratio_right = 0;
    TextMesh ratioText;
    Rigidbody carBody;

    bool touchedButton = false;

    // Start is called before the first frame update
    void Start()
    {
        arrow = GameObject.Find("Arrow");
        ratioText = GameObject.Find("Ratio").GetComponent<TextMesh>();
        // car = GameObject.Find("Car");
        //carBody = car.GetComponent<Rigidbody>();
        //initPos = car.transform.localPosition;

        SpawnCar();
    }

    // Update is called once per frame
    void Update()
    {

        if (currentState == State.Ready) {
            // Keyboard
            if (Input.GetKeyDown("left"))
            {
                Debug.Log("left");
                arrow.transform.localRotation = Quaternion.Euler(0, 145f, 0);
                car.transform.localRotation = Quaternion.Euler(0, 145f, 0);
            }

            if (Input.GetKeyDown("right"))
            {
                Debug.Log("right");
                arrow.transform.localRotation = Quaternion.Euler(0, 215f, 0);
                car.transform.localRotation = Quaternion.Euler(0, 215f, 0);
            }

            if (Input.GetKeyDown("space"))
            {
                FireCar();
            }

            // Finger

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0); // get first touch since touch count is greater than zero
                // Ignore button presses

                if (touch.phase == TouchPhase.Began)
                {
                    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    {
                        Debug.Log("Touching Button");
                        touchedButton = true;
                    } else
                    {
                        touchedButton = false;
                    }
                }
                if (touch.phase == TouchPhase.Moved &! touchedButton)
                {
                    float pct = touch.position.x / Screen.width;
                    float degrees = (90 * pct) + 135;
                    arrow.transform.localRotation = Quaternion.Euler(0, degrees, 0);
                    car.transform.localRotation = Quaternion.Euler(0, degrees, 0);
                }
                if (touch.phase == TouchPhase.Ended &! touchedButton)
                {
                    FireCar();
                }
                
            }

        } else if (currentState == State.CarMoving ) {
            if (Time.time - startTime > 0.75) {
                //Destroy(car);
                //car.transform.localPosition = initPos;
                carBody.useGravity = false;
                carBody.isKinematic = true;

                SpawnCar();

                currentState = State.Ready;
                ratioText.text = ratio_left + " : " + ratio_right;

                if (ratio_left == 6 && ratio_right == 2) {
                    ParticleSystem fireworks = GameObject.Find("VfxBoomFireworks").GetComponent<ParticleSystem>();
                    fireworks.Play();
                    ratioText.color = Color.green;
                    GameObject.Find("Cheer").GetComponent<AudioSource>().Play();
                }

            }
        }
    }

    void FireCar()
    {
        car.tag = "CarMaterial";
        carBody = car.GetComponent<Rigidbody>();
        carBody.useGravity = true;

        Debug.Log(arrow.transform.localRotation.eulerAngles);
        Quaternion right = new Quaternion(0f, -1.0f, 0f, 0.3f);
        if (arrow.transform.localRotation.eulerAngles.y > 180)
        {
            car.transform.localRotation = Quaternion.Euler(0, 215f, 0);
            carBody.velocity = new Vector3(.2f, .6f, .6f);
            ratio_right += 1;
        }
        else
        {
            car.transform.localRotation = Quaternion.Euler(0, 145, 0);
            carBody.velocity = new Vector3(-.2f, .6f, .6f);
            ratio_left += 1;
        }
        if (ratio_right != 6 && ratio_left != 2 && ratioText.color == Color.green)
        {
            ratioText.color = Color.white;
        }
        carBody.isKinematic = false;
        startTime = Time.time;
        currentState = State.CarMoving;
    }

    void SpawnCar()
    {
        car = Instantiate(Resources.Load("Car", typeof(GameObject))) as GameObject;
        Debug.Log(GameObject.Find("ImageTarget").transform.rotation);
        car.transform.parent = GameObject.Find("ImageTarget").transform;
        car.transform.rotation = GameObject.Find("ImageTarget").transform.rotation;
        car.transform.Rotate(0, 180, 0);
        car.transform.localPosition = new Vector3(0, 0.1f, -0.5f);
    }

}

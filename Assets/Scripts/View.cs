using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{

    public float mouseSpeed = 4.0f;


    float xRotate = 0.0f;

    GameObject cam;
    // Start is called before the first frame update
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        cam = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        MouseRotation();
    }

    void MouseRotation()
    {
        float yRotateSize = Input.GetAxis("Mouse X") * mouseSpeed;

        float yRotate = transform.eulerAngles.y + yRotateSize;

        float xRotateSize = Input.GetAxis("Mouse Y") * mouseSpeed;

        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);

        transform.eulerAngles = new Vector3(0, yRotate, 0);

        cam.transform.eulerAngles = new Vector3(xRotate, yRotate, 0); 

    }
}

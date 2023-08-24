using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CameraLogic : MonoBehaviour
{

    public Transform camera_trans;
    public float camSpeed = 30f;
    public Vector2 camLimit;

    public float scrollSpeed = 20f;
    public float minY = 10f;
    public float maxY = 120f;

    void Start()
    {
        camLimit = new Vector2 ( 450, 460 );    
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 cameraPos = transform.position;

        if(Input.GetKey("w") || Input.mousePosition.y >= Screen.height)
        {
            cameraPos.z += camSpeed * Time.deltaTime;
        }

        if (Input.GetKey("s") || Input.mousePosition.y <= 0)
        {
            cameraPos.z -= camSpeed * Time.deltaTime;
        }

        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width)
        {
            cameraPos.x += camSpeed * Time.deltaTime;
        }

        if (Input.GetKey("a") || Input.mousePosition.x <= 0)
        {
            cameraPos.x -= camSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        cameraPos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

        cameraPos.x = Mathf.Clamp(cameraPos.x, -camLimit.x, camLimit.x);
        cameraPos.y = Mathf.Clamp(cameraPos.y, minY, maxY);
        cameraPos.z = Mathf.Clamp(cameraPos.z, -camLimit.y, camLimit.y);

        transform.position = cameraPos;

    }
}

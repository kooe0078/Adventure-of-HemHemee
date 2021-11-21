using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float m_CameraMoveSpeed = 120.0f;
    public GameObject CameraFollowObj;
    Vector3 m_FollowPOS;
    public float clampAnglehigh = 80.0f;
    public float clampAnglelow = 10.0f;
    public float inputSensitivity = 150.0f;
    public GameObject CameraObj;
    public GameObject PlayerObj;
   // public float camDistanceXToPlayer;
    //public float camDistanceYToPlayer;
    //public float camDistanceZToPlayer;
    public float mouseX;
    public float mouseY;
    //public float smoothX;
    //public float smoothY;
    private float rotX = 0.0f;
    private float rotY = 0.0f;

    private void Start()
    {
        CameraFollowObj = GameObject.FindGameObjectWithTag("CameraFollow");
        Vector3 rot = transform.localRotation.eulerAngles;
        rotX = rot.x;
        rotY = rot.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        rotY -= mouseX * inputSensitivity * Time.deltaTime; /**/
        rotX -= mouseY * inputSensitivity * Time.deltaTime; /*흠흠이가 바꿔놓음*/

        rotX = Mathf.Clamp(rotX, -clampAnglelow, clampAnglehigh);
        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;
    }

    private void LateUpdate()
    {
        CameraUpdater();
    }
    void CameraUpdater()
    {
        // 따라갈 오브젝트 생성
        Transform target = CameraFollowObj.transform;

        // 타겟을 향해 
        float step = m_CameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}

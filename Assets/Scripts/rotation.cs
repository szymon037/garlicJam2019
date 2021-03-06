﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour
{
    public bool isRotating;
    private Rigidbody playerBody = null;
    public float speed = 0f;
    public float gravity = 2f;
    public Vector3 gravityVector = Vector3.zero;

    public Quaternion q1;
    public Quaternion q2;

    public Vector3 hitPoint = Vector3.zero;
    public Vector3 fromV = Vector3.zero;
    public Vector3 toV = Vector3.zero;
    public Vector3 rightVector = Vector3.zero;
    public Vector3 lastMove = Vector3.zero;
    public Vector3 directionVector = Vector3.zero;
    public Vector3 directionVectorRotated = Vector3.zero;

    public float oldAngleZ = 0f;
    public float newAngleZ = 0f;
    public float angleForChangingTransform = 0f;
    public float rotationTimer = 0f;
    public float lerpTimer = 0f;
    public float angleX = 0f;
    public float angleY = 0f;


    public bool movingForward;
    public bool movingBackward;
    public bool didCheckPosition;
    public bool topDownZ;
    public bool rightLeftZ;

    public bool destroyArrow = false;


    void Start()
    {
        playerBody = gameObject.GetComponent<Rigidbody>();
        isRotating = false;
        speed = 5f;
        gravityVector = new Vector3(0f, -gravity, 0f);


        movingForward = false;
        movingBackward = false;
        rotationTimer = 99f;
        lerpTimer = 99f;
        didCheckPosition = false;
        topDownZ = true;
        rightLeftZ = false;
        lastMove = transform.position;
        rightVector = Vector3.right;
        destroyArrow = false;
    }

    void FixedUpdate()
    {

        playerBody.velocity += gravityVector;

        if ((topDownZ && (Vector3.Scale(rightVector, transform.position).x - Vector3.Scale(rightVector, lastMove).x > 0f)) ||
            (rightLeftZ && (Vector3.Scale(rightVector, transform.position).y - Vector3.Scale(rightVector, lastMove).y > 0f)))
        {
            movingForward = true;
            movingBackward = false;
        }
        else if ((topDownZ && (Vector3.Scale(rightVector, transform.position).x - Vector3.Scale(rightVector, lastMove).x < 0f)) ||
                (rightLeftZ && (Vector3.Scale(rightVector, transform.position).y - Vector3.Scale(rightVector, lastMove).y < 0f)))
        {
            movingForward = false;
            movingBackward = true;
        }
        directionVector = transform.position - lastMove;
        directionVector = directionVector.normalized;

        lastMove = transform.position;


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "rotate")
        {
            if (movingForward)
            {
                oldAngleZ = newAngleZ;
                newAngleZ -= 90f;
                angleForChangingTransform = -90f;
            }
            else if (movingBackward)
            {
                oldAngleZ = newAngleZ;
                newAngleZ += 90f;
                angleForChangingTransform = 90f;
            }
            Debug.Log("ENTER");

            directionVectorRotated = Quaternion.Euler(0f, 0f, angleForChangingTransform) * directionVector;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "rotate")
        {
            if (!didCheckPosition)
            {
                if (movingForward)
                {
                    if ((topDownZ && (Vector3.Scale(rightVector, transform.position).x - Vector3.Scale(rightVector, other.transform.position).x) > 0f) ||
                        (rightLeftZ && (Vector3.Scale(rightVector, transform.position).y - Vector3.Scale(rightVector, other.transform.position).y) > 0f))
                    {
                        didCheckPosition = true;
                    }

                }
                else if (movingBackward)
                {

                    if ((topDownZ && (Vector3.Scale(rightVector, transform.position).x - Vector3.Scale(rightVector, other.transform.position).x) < 0f) ||
                        (rightLeftZ && (Vector3.Scale(rightVector, transform.position).y - Vector3.Scale(rightVector, other.transform.position).y) < 0f))
                    {
                        didCheckPosition = true;
                    }
                }
                if (newAngleZ <= -360f)
                    newAngleZ += 360f;

                else if (newAngleZ >= 360f)
                    newAngleZ -= 360f;

                if (didCheckPosition)
                {
                    if (topDownZ)
                        fromV = new Vector3(other.transform.position.x, transform.position.y, transform.position.z);
                    else if (rightLeftZ)
                        fromV = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);

                    this.transform.position = fromV;

                    hitPoint = other.transform.position;
                    rotationTimer = Time.deltaTime;
                    isRotating = true;
                }

            }

            if (rotationTimer <= 0.5001f && isRotating)
            {
                transform.Rotate(new Vector3(0f, 0f, angleForChangingTransform) * Time.deltaTime * 2, Space.World);

                //q1 = Quaternion.Slerp(Quaternion.Euler(new Vector3(0f, 0f, oldAngleZ)), Quaternion.Euler(new Vector3(0f, 0f, newAngleZ)), rotationTimer * 2);
                q2 = Quaternion.Slerp(Quaternion.Euler(new Vector3(0f, 0f, 0f)), Quaternion.Euler(new Vector3(0f, 0f, angleForChangingTransform)), rotationTimer * 2);
                //transform.rotation = q1;

                this.transform.position = new Vector3((fromV.x - hitPoint.x) * Mathf.Cos((q2.eulerAngles.z) / 180f * Mathf.PI) - (fromV.y - hitPoint.y) * Mathf.Sin((q2.eulerAngles.z) / 180f * Mathf.PI) + fromV.x - (fromV.x - hitPoint.x),
                                                      (fromV.x - hitPoint.x) * Mathf.Sin((q2.eulerAngles.z) / 180f * Mathf.PI) + (fromV.y - hitPoint.y) * Mathf.Cos((q2.eulerAngles.z) / 180f * Mathf.PI) + fromV.y - (fromV.y - hitPoint.y),
                                                      this.transform.position.z);

                rotationTimer += Time.deltaTime;
            }

            if (isRotating)
                gravityVector = Vector3.zero;

            if (rotationTimer >= 0.5001f && isRotating)
            {
                isRotating = false;
                destroyArrow = true;
            }

            if (rotationTimer >= 0.5001f && !isRotating && didCheckPosition)
            {
                transform.position += directionVectorRotated;
                // transform.position += transform.right * 0.01f;

                //isRotating = false;
                //lerpTimer = 0f;
                //fromV = transform.position;
                //toV = transform.position + transform.right * 0.5f;
                //transform.rotation = Quaternion.Euler(angleX, angleY, this.transform.rotation.eulerAngles.z);
                //Debug.Log("<color=red>LERP" + " fromV: " + fromV + " toV:" + toV + " </color>");

            }
            /*if (lerpTimer <= 0.2f && !isRotating)
            {
                //transform.position = Vector3.Lerp(fromV, toV, lerpTimer*2f);
                lerpTimer += Time.deltaTime;
              //  playerBody.AddForce(transform.right, ForceMode.Impulse);
                //Debug.Log("<color=red>LERP" + " fromV: " + fromV + " toV:" + toV + " </color>");
            }*/
            //Debug.Log("STAY");


        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "rotate")
        {
            gravityVector = -transform.up * gravity;
            didCheckPosition = false;
            changeVectorRight();
            lerpTimer = 99f;
            Debug.Log("EXIT");

        }
    }

    void changeVectorRight()
    {
        Debug.Log("changeVectorRight()");
        Debug.Log("transform.rotation.z: " + newAngleZ);
        if (/*transform.rotation.z*/newAngleZ >= -0.5f && /*transform.rotation.z*/newAngleZ <= 0.5f)
        {
            rightVector = Vector3.right;
            topDownZ = true;
            rightLeftZ = false;
        }
        else if (/*transform.rotation.z*/newAngleZ >= -90.5f && /*transform.rotation.z*/newAngleZ <= -89.5f || /*transform.rotation.z*/newAngleZ >= 269.5 &&/* transform.rotation.z*/newAngleZ <= 270.5)
        {
            rightVector = Vector3.down;
            topDownZ = false;
            rightLeftZ = true;
        }
        else if (/*transform.rotation.z*/newAngleZ >= -180.5f && /*transform.rotation.z*/newAngleZ <= -179.5f || /*transform.rotation.z*/newAngleZ >= 179.5f && /*transform.rotation.z*/newAngleZ <= 180.5f)
        {
            rightVector = Vector3.left;
            topDownZ = true;
            rightLeftZ = false;
        }
        else if (/*transform.rotation.z*/newAngleZ >= -270.5f && /*transform.rotation.z*/newAngleZ <= -269.5f || /*transform.rotation.z*/newAngleZ >= 89.5f && /*transform.rotation.z*/newAngleZ <= 90.5f)
        {
            rightVector = Vector3.up;
            topDownZ = false;
            rightLeftZ = true;
        }
    }
}

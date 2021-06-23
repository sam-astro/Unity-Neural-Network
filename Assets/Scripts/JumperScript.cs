using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class JumperScript : MonoBehaviour
{
    private NeuralNetwork net;
    private Rigidbody2D rb;

    private Transform target;
    public bool failed;

    public Transform sensorA, sensorB, sensorC, sensorD, sensorE, sensorF, sensorG, sensorH;
    float distanceFromSensorA, distanceFromSensorB, distanceFromSensorC, distanceFromSensorD, distanceFromSensorE, distanceFromSensorF, distanceFromSensorG, distanceFromSensorH;

    public Transform topLeftLegA;
    public Transform topLeftLegB;
    public Transform bottomLeftLegA;
    public Transform bottomLeftLegB;
    public Transform topRightLegA;
    public Transform topRightLegB;
    public Transform bottomRightLegA;
    public Transform bottomRightLegB;

    float distanceTopLeftLegA;
    float distanceTopLeftLegB;
    float distanceBottomLeftLegA;
    float distanceBottomLeftLegB;
    float distanceTopRightLegA;
    float distanceTopRightLegB;
    float distanceBottomRightLegA;
    float distanceBottomRightLegB;

    public Transform groundSensor;
    public Transform wallSensor;

    public Transform footOne;
    public Transform footTwo;

    float distanceFootOne;
    float distanceFootTwo;

    float distanceFromGround;
    float distanceFromWall;

    public LayerMask groundLayer;

    float countDown = 60;

    public float distanceTravelled;

    float direction;

    public GameObject body, topLeftLeg, bottomLeftLeg, topRightLeg, bottomRightLeg;
    int bodyColliding, topLeftLegColliding, bottomLeftLegColliding, topRightLegColliding, bottomRightLegColliding;

    float timeToComplete;
    Color randomColor;

    public TMP_Text distanceTravelledText;

    float motorSpeedMultiplier = 250;

    float internalTimer = 1f;

    void Awake()
    {
        failed = false;
        randomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 0.9f);
        body.GetComponent<SpriteRenderer>().color = randomColor;
        topLeftLeg.GetComponent<SpriteRenderer>().color = randomColor;
        bottomLeftLeg.GetComponent<SpriteRenderer>().color = randomColor;
        topRightLeg.GetComponent<SpriteRenderer>().color = randomColor;
        bottomRightLeg.GetComponent<SpriteRenderer>().color = randomColor;
    }

    void FixedUpdate()
    {
        #region Sensors
        direction = body.transform.rotation.z;

        #region Directional Sensors
        if (Physics.Linecast(body.transform.position, sensorA.position, out RaycastHit sensedInfoA, groundLayer))
        {
            distanceFromSensorA = Vector3.Distance(sensedInfoA.point, body.transform.position);
        }
        else
        {
            distanceFromSensorA = 15f;
        }

        if (Physics.Linecast(body.transform.position, sensorB.position, out RaycastHit sensedInfoB, groundLayer))
        {
            distanceFromSensorB = Vector3.Distance(sensedInfoB.point, body.transform.position);
        }
        else
        {
            distanceFromSensorB = 15f;
        }

        if (Physics.Linecast(body.transform.position, sensorC.position, out RaycastHit sensedInfoC, groundLayer))
        {
            distanceFromSensorC = Vector3.Distance(sensedInfoC.point, body.transform.position);
        }
        else
        {
            distanceFromSensorC = 15f;
        }

        if (Physics.Linecast(body.transform.position, sensorD.position, out RaycastHit sensedInfoD, groundLayer))
        {
            distanceFromSensorD = Vector3.Distance(sensedInfoD.point, body.transform.position);
        }
        else
        {
            distanceFromSensorD = 15f;
        }

        if (Physics.Linecast(body.transform.position, sensorE.position, out RaycastHit sensedInfoE, groundLayer))
        {
            distanceFromSensorE = Vector3.Distance(sensedInfoE.point, body.transform.position);
        }
        else
        {
            distanceFromSensorE = 15f;
        }

        if (Physics.Linecast(body.transform.position, sensorF.position, out RaycastHit sensedInfoF, groundLayer))
        {
            distanceFromSensorF = Vector3.Distance(sensedInfoF.point, body.transform.position);
        }
        else
        {
            distanceFromSensorF = 15f;
        }

        if (Physics.Linecast(body.transform.position, sensorG.position, out RaycastHit sensedInfoG, groundLayer))
        {
            distanceFromSensorG = Vector3.Distance(sensedInfoG.point, body.transform.position);
        }
        else
        {
            distanceFromSensorG = 15f;
        }

        if (Physics.Linecast(body.transform.position, sensorH.position, out RaycastHit sensedInfoH, groundLayer))
        {
            distanceFromSensorH = Vector3.Distance(sensedInfoH.point, body.transform.position);
        }
        else
        {
            distanceFromSensorH = 15f;
        }
        #endregion

        //if (direction < -180 || direction > 180)
        //{
        //	failed = true;
        //}

        if (Physics.Linecast(body.transform.position, groundSensor.position, out RaycastHit sensedInfoI, groundLayer))
        {
            distanceFromGround = Vector3.Distance(sensedInfoI.point, body.transform.position);
        }
        else
        {
            distanceFromGround = 15f;
        }

        #region Limb Distance From Ground Sensors
        if (Physics.Linecast(bottomLeftLeg.transform.position, footOne.position, out RaycastHit sensedInfoK, groundLayer))
        {
            distanceFootOne = Vector3.Distance(sensedInfoK.point, bottomLeftLeg.transform.position);
        }
        else
        {
            distanceFootOne = 5;
        }

        if (Physics.Linecast(bottomRightLeg.transform.position, footTwo.position, out RaycastHit sensedInfoL, groundLayer))
        {
            distanceFootTwo = Vector3.Distance(sensedInfoL.point, bottomRightLeg.transform.position);
        }
        else
        {
            distanceFootTwo = 5;
        }


        if (Physics.Linecast(topLeftLeg.transform.position, topLeftLegA.position, out RaycastHit sensedInfoM, groundLayer))
        {
            distanceTopLeftLegA = Vector3.Distance(sensedInfoK.point, topLeftLeg.transform.position);
        }
        else
        {
            distanceTopLeftLegA = 40;
        }

        if (Physics.Linecast(topLeftLeg.transform.position, topLeftLegB.position, out RaycastHit sensedInfoN, groundLayer))
        {
            distanceTopLeftLegB = Vector3.Distance(sensedInfoL.point, topLeftLeg.transform.position);
        }
        else
        {
            distanceTopLeftLegB = 40;
        }

        if (Physics.Linecast(bottomLeftLeg.transform.position, bottomLeftLegA.position, out RaycastHit sensedInfoO, groundLayer))
        {
            distanceBottomLeftLegA = Vector3.Distance(sensedInfoM.point, bottomLeftLeg.transform.position);
        }
        else
        {
            distanceBottomLeftLegA = 40;
        }

        if (Physics.Linecast(bottomLeftLeg.transform.position, bottomLeftLegB.position, out RaycastHit sensedInfoP, groundLayer))
        {
            distanceBottomLeftLegB = Vector3.Distance(sensedInfoN.point, bottomLeftLeg.transform.position);
        }
        else
        {
            distanceBottomLeftLegB = 40;
        }


        if (Physics.Linecast(topRightLeg.transform.position, topRightLegA.position, out RaycastHit sensedInfoQ, groundLayer))
        {
            distanceTopRightLegA = Vector3.Distance(sensedInfoO.point, topRightLeg.transform.position);
        }
        else
        {
            distanceTopRightLegA = 40;
        }

        if (Physics.Linecast(topRightLeg.transform.position, topRightLegB.position, out RaycastHit sensedInfoR, groundLayer))
        {
            distanceTopRightLegB = Vector3.Distance(sensedInfoP.point, topRightLeg.transform.position);
        }
        else
        {
            distanceTopRightLegB = 40;
        }

        if (Physics.Linecast(bottomRightLeg.transform.position, bottomRightLegA.position, out RaycastHit sensedInfoS, groundLayer))
        {
            distanceBottomRightLegA = Vector3.Distance(sensedInfoQ.point, bottomRightLeg.transform.position);
        }
        else
        {
            distanceBottomRightLegA = 40;
        }

        if (Physics.Linecast(bottomRightLeg.transform.position, bottomRightLegB.position, out RaycastHit sensedInfoT, groundLayer))
        {
            distanceBottomRightLegB = Vector3.Distance(sensedInfoR.point, bottomRightLeg.transform.position);
        }
        else
        {
            distanceBottomRightLegB = 40;
        }

        #endregion

        #region Colliding Sensors
        if (body.GetComponent<CheckIfColliding>().isColliding) //This is for if you want the AI to stay upright
        {
            bodyColliding = 1;
            //failed = true;
        }
        else
        {
            bodyColliding = 0;
        }

        if (topLeftLeg.GetComponent<CheckIfColliding>().isColliding)
        {
            topLeftLegColliding = 1;
            //failed = true;
        }
        else
        {
            topLeftLegColliding = 0;
        }

        if (bottomLeftLeg.GetComponent<CheckIfColliding>().isColliding)
        {
            bottomLeftLegColliding = 1;
        }
        else
        {
            bottomLeftLegColliding = 0;
        }

        if (topRightLeg.GetComponent<CheckIfColliding>().isColliding)
        {
            topRightLegColliding = 1;
            //failed = true;
        }
        else
        {
            topRightLegColliding = 0;
        }

        if (bottomRightLeg.GetComponent<CheckIfColliding>().isColliding)
        {
            bottomRightLegColliding = 1;
        }
        else
        {
            bottomRightLegColliding = 0;
        }

        if (bottomRightLeg.GetComponent<CheckIfColliding>().failed || topRightLeg.GetComponent<CheckIfColliding>().failed || bottomLeftLeg.GetComponent<CheckIfColliding>().failed || body.GetComponent<CheckIfColliding>().failed || topLeftLeg.GetComponent<CheckIfColliding>().failed)
        {
            failed = true;
        }
        #endregion

        timeToComplete += Time.deltaTime;

        #endregion

        //distanceTravelled = timeToComplete * body.GetComponent<Rigidbody2D>().velocity.x;

        if(body.transform.position.y > distanceTravelled)
		{
            distanceTravelled = body.transform.position.y;
		}

        if (!failed)
        {
            if (internalTimer > 0)
            {
                internalTimer -= Time.deltaTime;
            }
            else
            {
                internalTimer = 1;
            }

            distanceTravelled = body.transform.position.x;
            distanceTravelledText.text = Mathf.Round(body.transform.position.x).ToString();

            body.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(body.transform.position.y);
            topLeftLeg.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(body.transform.position.y);
            bottomLeftLeg.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(body.transform.position.y);
            topRightLeg.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(body.transform.position.y);
            bottomRightLeg.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(body.transform.position.y);
            distanceTravelledText.GetComponent<MeshRenderer>().sortingOrder = Mathf.RoundToInt(body.transform.position.y) + 1;

            float[] inputs = new float[34];

            inputs[0] = distanceFromGround / 15;

            inputs[1] = bodyColliding;
            inputs[2] = topLeftLegColliding;
            inputs[3] = bottomLeftLegColliding;
            inputs[4] = topRightLegColliding;
            inputs[5] = bottomRightLegColliding;

            inputs[6] = Mathf.Abs(direction) / 360;

            inputs[7] = distanceFootOne / 5;
            inputs[8] = distanceFootTwo / 5;
            inputs[9] = topLeftLeg.transform.rotation.z;
            inputs[10] = bottomLeftLeg.transform.rotation.z;
            inputs[11] = topRightLeg.transform.rotation.z;
            inputs[12] = bottomRightLeg.transform.rotation.z;

            JointMotor2D motorA = topLeftLeg.GetComponent<HingeJoint2D>().motor;
            JointMotor2D motorB = bottomLeftLeg.GetComponent<HingeJoint2D>().motor;
            JointMotor2D motorC = topRightLeg.GetComponent<HingeJoint2D>().motor;
            JointMotor2D motorD = bottomRightLeg.GetComponent<HingeJoint2D>().motor;

            inputs[13] = motorA.motorSpeed;
            inputs[14] = motorB.motorSpeed;
            inputs[15] = motorC.motorSpeed;
            inputs[16] = motorD.motorSpeed;

            inputs[17] = distanceFromSensorA / 15;
            inputs[18] = distanceFromSensorB / 15;
            inputs[19] = distanceFromSensorC / 15;
            inputs[20] = distanceFromSensorD / 15;
            inputs[21] = distanceFromSensorE / 15;
            inputs[22] = distanceFromSensorF / 15;
            inputs[23] = distanceFromSensorG / 15;
            inputs[24] = distanceFromSensorH / 15;

            inputs[25] = distanceTopLeftLegA / 5;
            inputs[26] = distanceTopLeftLegB / 5;
            inputs[27] = distanceBottomLeftLegA / 5;
            inputs[28] = distanceBottomLeftLegB / 5;
            inputs[29] = distanceTopRightLegA / 5;
            inputs[30] = distanceTopRightLegB / 5;
            inputs[31] = distanceBottomRightLegA / 5;
            inputs[32] = distanceBottomRightLegB / 5;
            inputs[33] = internalTimer;

            float[] output = net.FeedForward(inputs);

            if (output[4] > 1)
            {
                motorA.motorSpeed = output[0] * motorSpeedMultiplier;
            }
            else
            {
                motorA.motorSpeed = -output[0] * motorSpeedMultiplier;
            }
            topLeftLeg.GetComponent<HingeJoint2D>().motor = motorA;

            if (output[5] > 1)
            {
                motorB.motorSpeed = output[1] * motorSpeedMultiplier;
            }
            else
            {
                motorB.motorSpeed = -output[1] * motorSpeedMultiplier;
            }
            bottomLeftLeg.GetComponent<HingeJoint2D>().motor = motorB;

            if (output[6] > 1)
            {
                motorC.motorSpeed = output[2] * motorSpeedMultiplier;
            }
            else
            {
                motorC.motorSpeed = -output[2] * motorSpeedMultiplier;
            }
            topRightLeg.GetComponent<HingeJoint2D>().motor = motorC;

            if (output[7] > 1)
            {
                motorD.motorSpeed = output[3] * motorSpeedMultiplier;
            }
            else
            {
                motorD.motorSpeed = -output[3] * motorSpeedMultiplier;
            }
            bottomRightLeg.GetComponent<HingeJoint2D>().motor = motorD;

            //topLeftLeg.GetComponent<Rigidbody2D>().rotation += output[0] * 50;
            //bottomLeftLeg.GetComponent<Rigidbody2D>().rotation += output[1] * 50;
            //topRightLeg.GetComponent<Rigidbody2D>().rotation += output[2] * 50;
            //bottomRightLeg.GetComponent<Rigidbody2D>().rotation += output[3] * 50;

            net.SetFitness(distanceTravelled);
            //net.SetFitness(timeToComplete);
        }
        else
        {
            body.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            topLeftLeg.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            bottomLeftLeg.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            topRightLeg.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            bottomRightLeg.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            Destroy(distanceTravelledText);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag == "Danger")
        //{
        //    failed = true;
        //    net.AddFitness((net.GetFitness() / 2) * -1);
        //}
    }

    public void Init(NeuralNetwork net, Transform target)
    {
        this.net = net;
        this.target = target;
        //net.SetFitness(10f);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawLine(wallSensor.position, body.transform.position);
    }
}


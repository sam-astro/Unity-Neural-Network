using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ThreeDWalkerScript : MonoBehaviour
{
    [HideInInspector]
    public NeuralNetwork net;
    private Rigidbody2D rb;

    private Transform target;
    float lowestDistanceFromTarget = 100000;
    public bool failed;

    public LayerMask groundLayer;

    float countDown = 60;

    public float distanceTravelled;

    float direction;

    public GameObject head, upperBody, rUpperArm, rLowerArm, rHand, lUpperArm, lLowerArm, lHand, midBodyA, midBodyB, lowerBody, topLeftLeg, bottomLeftLeg, leftFoot, topRightLeg, bottomRightLeg, rightFoot;
    public GameObject lShoulderJoint, rShoulderJoint, lElbowJoint, rElbowJoint, lWristJoint, rWristJoint, uTorsoJoint, mTorsoJoint, lTorsoJoint, lLegJoint, rLegJoint, lKneeJoint, rKneeJoint, lAnkleJoint, rAnkleJoint;

    float timeToComplete;

    float motorSpeedMultiplier = 300;

    public bool stayUpright;
    float uprightAmount;

    public Material failedMaterial;
    public Material normalMaterial;

    float internalTimer = 1f;

    void Awake()
    {
        failed = false;
        lowestDistanceFromTarget = 100000;

        head.GetComponent<MeshRenderer>().material = normalMaterial;
        upperBody.GetComponent<MeshRenderer>().material = normalMaterial;
        rUpperArm.GetComponent<MeshRenderer>().material = normalMaterial;
        rLowerArm.GetComponent<MeshRenderer>().material = normalMaterial;
        rHand.GetComponent<MeshRenderer>().material = normalMaterial;
        lUpperArm.GetComponent<MeshRenderer>().material = normalMaterial;
        lLowerArm.GetComponent<MeshRenderer>().material = normalMaterial;
        lHand.GetComponent<MeshRenderer>().material = normalMaterial;
        midBodyA.GetComponent<MeshRenderer>().material = normalMaterial;
        midBodyB.GetComponent<MeshRenderer>().material = normalMaterial;
        lowerBody.GetComponent<MeshRenderer>().material = normalMaterial;
        topLeftLeg.GetComponent<MeshRenderer>().material = normalMaterial;
        bottomLeftLeg.GetComponent<MeshRenderer>().material = normalMaterial;
        leftFoot.GetComponent<MeshRenderer>().material = normalMaterial;
        topRightLeg.GetComponent<MeshRenderer>().material = normalMaterial;
        bottomRightLeg.GetComponent<MeshRenderer>().material = normalMaterial;
        rightFoot.GetComponent<MeshRenderer>().material = normalMaterial;
        lShoulderJoint.GetComponent<MeshRenderer>().material = normalMaterial;
        rShoulderJoint.GetComponent<MeshRenderer>().material = normalMaterial;
        lElbowJoint.GetComponent<MeshRenderer>().material = normalMaterial;
        rElbowJoint.GetComponent<MeshRenderer>().material = normalMaterial;
        lWristJoint.GetComponent<MeshRenderer>().material = normalMaterial;
        rWristJoint.GetComponent<MeshRenderer>().material = normalMaterial;
        uTorsoJoint.GetComponent<MeshRenderer>().material = normalMaterial;
        mTorsoJoint.GetComponent<MeshRenderer>().material = normalMaterial;
        lTorsoJoint.GetComponent<MeshRenderer>().material = normalMaterial;
        lLegJoint.GetComponent<MeshRenderer>().material = normalMaterial;
        rLegJoint.GetComponent<MeshRenderer>().material = normalMaterial;
        lKneeJoint.GetComponent<MeshRenderer>().material = normalMaterial;
        rKneeJoint.GetComponent<MeshRenderer>().material = normalMaterial;
        lAnkleJoint.GetComponent<MeshRenderer>().material = normalMaterial;
        rAnkleJoint.GetComponent<MeshRenderer>().material = normalMaterial;
    }

    void FixedUpdate()
    {
		if (stayUpright)
        {
            if (upperBody.GetComponent<LimbsCollidingOrNot>().collidingInt == 1)
            {
                failed = true;
            }
            if (head.GetComponent<LimbsCollidingOrNot>().collidingInt == 1)
            {
                failed = true;
            }
            if (midBodyA.GetComponent<LimbsCollidingOrNot>().collidingInt == 1)
            {
                failed = true;
            }
            if (midBodyB.GetComponent<LimbsCollidingOrNot>().collidingInt == 1)
            {
                failed = true;
            }
            if (lowerBody.GetComponent<LimbsCollidingOrNot>().collidingInt == 1)
            {
                failed = true;
            }
            if (rUpperArm.GetComponent<LimbsCollidingOrNot>().collidingInt == 1)
            {
                failed = true;
            }
            if (lUpperArm.GetComponent<LimbsCollidingOrNot>().collidingInt == 1)
            {
                failed = true;
            }
            if (rLowerArm.GetComponent<LimbsCollidingOrNot>().collidingInt == 1)
            {
                failed = true;
            }
            if (lLowerArm.GetComponent<LimbsCollidingOrNot>().collidingInt == 1)
            {
                failed = true;
            }
            if (rHand.GetComponent<LimbsCollidingOrNot>().collidingInt == 1)
            {
                failed = true;
            }
            if (lHand.GetComponent<LimbsCollidingOrNot>().collidingInt == 1)
            {
                failed = true;
            }
            if (topRightLeg.GetComponent<LimbsCollidingOrNot>().collidingInt == 1)
            {
                failed = true;
            }
            if (topLeftLeg.GetComponent<LimbsCollidingOrNot>().collidingInt == 1)
            {
                failed = true;
            }
            if (bottomRightLeg.GetComponent<LimbsCollidingOrNot>().collidingInt == 1)
            {
                failed = true;
            }
            if (bottomLeftLeg.GetComponent<LimbsCollidingOrNot>().collidingInt == 1)
            {
                failed = true;
            }
        }

        //distanceTravelled = timeToComplete * body.GetComponent<Rigidbody2D>().velocity.x;

        if (!failed)
        {
            if(internalTimer < 1)
            {
                internalTimer += Time.deltaTime;
            }
			else
			{
                internalTimer = 0;
			}

            if(Vector3.Distance(upperBody.transform.position, target.position) < lowestDistanceFromTarget)
			{
                lowestDistanceFromTarget = Vector3.Distance(upperBody.transform.position, target.position);
            }

            timeToComplete += Time.deltaTime;

            float[] inputs = new float[90];

            inputs[0] = lShoulderJoint.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[1] = lShoulderJoint.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[2] = lShoulderJoint.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[3] = rShoulderJoint.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[4] = rShoulderJoint.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[5] = rShoulderJoint.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[6] = lElbowJoint.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[7] = lElbowJoint.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[8] = lElbowJoint.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[9] = rElbowJoint.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[10] = rElbowJoint.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[11] = rElbowJoint.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[12] = lWristJoint.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[13] = lWristJoint.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[14] = lWristJoint.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[15] = rWristJoint.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[16] = rWristJoint.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[17] = rWristJoint.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[18] = lLegJoint.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[19] = lLegJoint.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[20] = lLegJoint.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[21] = rLegJoint.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[22] = rLegJoint.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[23] = rLegJoint.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[24] = lKneeJoint.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[25] = lKneeJoint.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[26] = lKneeJoint.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[27] = rKneeJoint.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[28] = rKneeJoint.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[29] = rKneeJoint.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[30] = lAnkleJoint.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[31] = lAnkleJoint.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[32] = lAnkleJoint.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[33] = rAnkleJoint.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[34] = rAnkleJoint.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[35] = rAnkleJoint.GetComponent<Rigidbody>().rotation.z / 360;

            //inputs[55] = (upperBody.transform.position.z * -1) / 100;

            inputs[36] = upperBody.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[37] = rUpperArm.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[38] = rLowerArm.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[39] = rHand.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[40] = lUpperArm.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[41] = lLowerArm.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[42] = lHand.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[43] = midBodyA.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[44] = midBodyB.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[45] = lowerBody.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[46] = topLeftLeg.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[47] = bottomLeftLeg.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[48] = leftFoot.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[49] = topRightLeg.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[50] = bottomRightLeg.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[51] = rightFoot.GetComponent<LimbsCollidingOrNot>().collidingInt;

            inputs[52] = rUpperArm.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[53] = rUpperArm.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[54] = rUpperArm.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[55] = lUpperArm.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[56] = lUpperArm.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[57] = lUpperArm.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[58] = rLowerArm.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[59] = rLowerArm.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[60] = rLowerArm.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[61] = lLowerArm.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[62] = lLowerArm.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[63] = lLowerArm.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[64] = rHand.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[65] = rHand.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[66] = rHand.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[67] = lHand.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[68] = lHand.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[69] = lHand.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[70] = topRightLeg.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[71] = topRightLeg.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[72] = topRightLeg.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[73] = topLeftLeg.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[74] = topLeftLeg.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[75] = topLeftLeg.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[76] = bottomRightLeg.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[77] = bottomRightLeg.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[78] = bottomRightLeg.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[79] = bottomLeftLeg.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[80] = bottomLeftLeg.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[81] = bottomLeftLeg.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[82] = rightFoot.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[83] = rightFoot.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[84] = rightFoot.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[85] = leftFoot.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[86] = leftFoot.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[87] = leftFoot.GetComponent<Rigidbody>().rotation.z / 360;

            inputs[88] = Mathf.Clamp(upperBody.transform.position.y / 10, 0.01f, 1);

            inputs[89] = Mathf.Clamp(internalTimer, 0.01f, 1);

            float[] output = net.FeedForward(inputs);

			lShoulderJoint.GetComponent<Rigidbody>().MoveRotation(lShoulderJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[0] * motorSpeedMultiplier / 2, output[1] * motorSpeedMultiplier / 2, output[2] * motorSpeedMultiplier / 2) * Time.deltaTime));
			rShoulderJoint.GetComponent<Rigidbody>().MoveRotation(rShoulderJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[3] * motorSpeedMultiplier / 2, output[4] * motorSpeedMultiplier / 2, output[5] * motorSpeedMultiplier / 2) * Time.deltaTime));
			lElbowJoint.GetComponent<Rigidbody>().MoveRotation(lElbowJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[6] * motorSpeedMultiplier / 2, output[7] * motorSpeedMultiplier / 2, output[8] * motorSpeedMultiplier / 2) * Time.deltaTime));
			rElbowJoint.GetComponent<Rigidbody>().MoveRotation(rElbowJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[9] * motorSpeedMultiplier / 2, output[10] * motorSpeedMultiplier / 2, output[11] * motorSpeedMultiplier / 2) * Time.deltaTime));
			lWristJoint.GetComponent<Rigidbody>().MoveRotation(lWristJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[12] * motorSpeedMultiplier / 2, output[13] * motorSpeedMultiplier / 2, output[14] * motorSpeedMultiplier / 2) * Time.deltaTime));
			rWristJoint.GetComponent<Rigidbody>().MoveRotation(rWristJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[15] * motorSpeedMultiplier / 2, output[16] * motorSpeedMultiplier / 2, output[17] * motorSpeedMultiplier / 2) * Time.deltaTime));
			//uTorsoJoint.GetComponent<Rigidbody>().MoveRotation(uTorsoJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[18] * motorSpeedMultiplier, output[19] * motorSpeedMultiplier, output[20] * motorSpeedMultiplier) * Time.deltaTime));
			//mTorsoJoint.GetComponent<Rigidbody>().MoveRotation(mTorsoJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[21] * motorSpeedMultiplier, output[22] * motorSpeedMultiplier, output[23] * motorSpeedMultiplier) * Time.deltaTime));
			//lTorsoJoint.GetComponent<Rigidbody>().MoveRotation(lTorsoJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[24] * motorSpeedMultiplier, output[25] * motorSpeedMultiplier, output[26] * motorSpeedMultiplier) * Time.deltaTime));
			lLegJoint.GetComponent<Rigidbody>().MoveRotation(lLegJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[18] * motorSpeedMultiplier, output[19] * motorSpeedMultiplier, output[20] * motorSpeedMultiplier) * Time.deltaTime));
			rLegJoint.GetComponent<Rigidbody>().MoveRotation(rLegJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[21] * motorSpeedMultiplier, output[22] * motorSpeedMultiplier, output[23] * motorSpeedMultiplier) * Time.deltaTime));
			lKneeJoint.GetComponent<Rigidbody>().MoveRotation(lKneeJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[24] * motorSpeedMultiplier, output[25] * motorSpeedMultiplier, output[26] * motorSpeedMultiplier) * Time.deltaTime));
			rKneeJoint.GetComponent<Rigidbody>().MoveRotation(rKneeJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[27] * motorSpeedMultiplier, output[28] * motorSpeedMultiplier, output[29] * motorSpeedMultiplier) * Time.deltaTime));
			lAnkleJoint.GetComponent<Rigidbody>().MoveRotation(lAnkleJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[30] * motorSpeedMultiplier, output[31] * motorSpeedMultiplier, output[32] * motorSpeedMultiplier) * Time.deltaTime));
			rAnkleJoint.GetComponent<Rigidbody>().MoveRotation(rAnkleJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[33] * motorSpeedMultiplier, output[34] * motorSpeedMultiplier, output[35] * motorSpeedMultiplier) * Time.deltaTime));


            //net.SetFitness(timeToComplete);
            net.AddFitness(upperBody.transform.position.y + 10);
			//net.SetFitness(distanceTravelled + uprightAmount * 10);
			uprightAmount += 180 - ((upperBody.transform.rotation.x + 180) / 2);
            //distanceTravelled = upperBody.transform.position.z * -1;
            distanceTravelled = net.GetFitness();

            //net.SetFitness(200 - Vector3.Distance(upperBody.transform.position, target.position) + timeToComplete * 10 + uprightAmount * 10);
            //net.SetFitness(100 - Vector3.Distance(upperBody.transform.position, target.position));
        }
		else
		{
			head.GetComponent<MeshRenderer>().material = failedMaterial;
			upperBody.GetComponent<MeshRenderer>().material = failedMaterial;
			rUpperArm.GetComponent<MeshRenderer>().material = failedMaterial;
			rLowerArm.GetComponent<MeshRenderer>().material = failedMaterial;
			rHand.GetComponent<MeshRenderer>().material = failedMaterial;
			lUpperArm.GetComponent<MeshRenderer>().material = failedMaterial;
			lLowerArm.GetComponent<MeshRenderer>().material = failedMaterial;
			lHand.GetComponent<MeshRenderer>().material = failedMaterial;
			midBodyA.GetComponent<MeshRenderer>().material = failedMaterial;
			midBodyB.GetComponent<MeshRenderer>().material = failedMaterial;
			lowerBody.GetComponent<MeshRenderer>().material = failedMaterial;
			topLeftLeg.GetComponent<MeshRenderer>().material = failedMaterial;
			bottomLeftLeg.GetComponent<MeshRenderer>().material = failedMaterial;
			leftFoot.GetComponent<MeshRenderer>().material = failedMaterial;
			topRightLeg.GetComponent<MeshRenderer>().material = failedMaterial;
			bottomRightLeg.GetComponent<MeshRenderer>().material = failedMaterial;
			rightFoot.GetComponent<MeshRenderer>().material = failedMaterial;
			lShoulderJoint.GetComponent<MeshRenderer>().material = failedMaterial;
			rShoulderJoint.GetComponent<MeshRenderer>().material = failedMaterial;
			lElbowJoint.GetComponent<MeshRenderer>().material = failedMaterial;
			rElbowJoint.GetComponent<MeshRenderer>().material = failedMaterial;
			lWristJoint.GetComponent<MeshRenderer>().material = failedMaterial;
			rWristJoint.GetComponent<MeshRenderer>().material = failedMaterial;
			uTorsoJoint.GetComponent<MeshRenderer>().material = failedMaterial;
			mTorsoJoint.GetComponent<MeshRenderer>().material = failedMaterial;
			lTorsoJoint.GetComponent<MeshRenderer>().material = failedMaterial;
			lLegJoint.GetComponent<MeshRenderer>().material = failedMaterial;
			rLegJoint.GetComponent<MeshRenderer>().material = failedMaterial;
			lKneeJoint.GetComponent<MeshRenderer>().material = failedMaterial;
			rKneeJoint.GetComponent<MeshRenderer>().material = failedMaterial;
			lAnkleJoint.GetComponent<MeshRenderer>().material = failedMaterial;
			rAnkleJoint.GetComponent<MeshRenderer>().material = failedMaterial;

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

    public void Init(NeuralNetwork net, Transform target, bool stayUpright)
    {
        this.net = net;
        this.target = target;
        this.stayUpright = stayUpright;
    }

    void OnDrawGizmos()
	{
        Gizmos.color = Color.red;
		//Gizmos.DrawLine(wallSensor.position, body.transform.position);
	}
}


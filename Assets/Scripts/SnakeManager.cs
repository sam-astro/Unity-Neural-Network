
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SnakeManager : MonoBehaviour
{
    public GameObject entityPrefab;
    public GameObject target;

    private bool isTraning = false;
    public int populationSize = 1;
    private int generationNumber = 0;
    public int[] layers = new int[] { 7, 10, 10, 6 }; //No. of inputs and No. of outputs
    private List<NeuralNetwork> nets;
    private bool leftMouseDown = false;
    [HideInInspector]
    public List<SnakeScript> entityList = null;

    public int amntLeft;

    public TMP_Text generationText;

    [Range(0.5f, 8)]
    public float timeScale = 0.5f;

    public float topDistance;

    public Material[] placingMaterials;

    public float timer;

    public bool runEffectiveLearning = false;

    public GameObject cameraObj;

    public Transform[][][] weightLines;

    public Image[] weightLinesColA;
    public Image[] weightLinesColB;
    public Image[] weightLinesColC;
    public Image[] weightLinesColD;
    public Image[] weightLinesColE;
    public Image[] weightLinesColF;
    public Image[] weightLinesColG;
    public Image[] weightLinesColH;
    public Image[] weightLinesColI;
    public Image[] weightLinesColJ;
    public Image[] weightLinesColK;
    public Image[] weightLinesColL;
    public Image[] weightLinesColM;
    public Image[] weightLinesColN;
    public Image[] weightLinesColO;
    public Image[] weightLinesColP;
    public Image[] weightLinesColQ;
    public Image[] weightLinesColR;
    public Image[] weightLinesColS;
    public Image[] weightLinesColT;

    public Transform canvas;

    public int multiplyAmnt;
    public float addAmnt;

    public TMP_Text creatureNameText;

    public int applesLeft;

    public Vector2 boardSize;

    public GameObject applePrefab;

    void Update()
    {
        generationText.text = generationNumber.ToString();

        Time.timeScale = timeScale;

        if(entityList != null)
        {
			for (int i = 0; i < entityList.Count; i++)
            {
                if (entityList[i].timeElapsed > topDistance && !entityList[i].failed && entityList[i] != null)
                {
                    //topDistance = entityList[i].timeElapsed;
                    //creatureNameText.text = "'" + entityList[i].gameObject.name + "' " + "Score: " + Mathf.Round(entityList[i].timeElapsed);

                    //outputNode.color = new Color(entityList[i].pressingButton, entityList[i].pressingButton, entityList[i].pressingButton, 1);

     //               #region Color Setters

     //               /// Weights are written [what layer (l to r)] [what node (b to t)] [what branch(b to t)]

     //               for (int j = 0; j < weightLinesColA.Length; j++)
					//{
					//	if (nets[i].weights[0][0][j] > 0)
					//	{
     //                       weightLinesColA[j].color = Color.red;
     //                   }
					//	else
					//	{
     //                       weightLinesColA[j].color = Color.blue;
     //                   }

     //                   weightLinesColA[j].rectTransform.sizeDelta = new Vector2(weightLinesColA[j].rectTransform.sizeDelta.x, Mathf.Abs(nets[i].weights[0][0][j] * multiplyAmnt) + addAmnt);
     //               }

					//for (int j = 0; j < weightLinesColB.Length; j++)
					//{
     //                   if (nets[i].weights[0][1][j] > 0)
     //                   {
     //                       weightLinesColB[j].color = Color.red;
     //                   }
     //                   else
     //                   {
     //                       weightLinesColB[j].color = Color.blue;
     //                   }

     //                   weightLinesColB[j].rectTransform.sizeDelta = new Vector2(weightLinesColB[j].rectTransform.sizeDelta.x, Mathf.Abs(nets[i].weights[0][1][j] * multiplyAmnt) + addAmnt);
     //               }
                    
					//for (int j = 0; j < weightLinesColC.Length; j++)
					//{
     //                   if (nets[i].weights[0][2][j] > 0)
     //                   {
     //                       weightLinesColC[j].color = Color.red;
     //                   }
     //                   else
     //                   {
     //                       weightLinesColC[j].color = Color.blue;
     //                   }

     //                   weightLinesColC[j].rectTransform.sizeDelta = new Vector2(weightLinesColC[j].rectTransform.sizeDelta.x, Mathf.Abs(nets[i].weights[0][2][j] * multiplyAmnt) + addAmnt);
     //               }
                    
					//for (int j = 0; j < weightLinesColD.Length; j++)
					//{
     //                   if (nets[i].weights[0][3][j] > 0)
     //                   {
     //                       weightLinesColD[j].color = Color.red;
     //                   }
     //                   else
     //                   {
     //                       weightLinesColD[j].color = Color.blue;
     //                   }

     //                   weightLinesColD[j].rectTransform.sizeDelta = new Vector2(weightLinesColD[j].rectTransform.sizeDelta.x, Mathf.Abs(nets[i].weights[0][3][j] * multiplyAmnt) + addAmnt);
     //               }
                    
					//for (int j = 0; j < weightLinesColE.Length; j++)
					//{
     //                   if (nets[i].weights[0][4][j] > 0)
     //                   {
     //                       weightLinesColE[j].color = Color.red;
     //                   }
     //                   else
     //                   {
     //                       weightLinesColE[j].color = Color.blue;
     //                   }

     //                   weightLinesColE[j].rectTransform.sizeDelta = new Vector2(weightLinesColE[j].rectTransform.sizeDelta.x, Mathf.Abs(nets[i].weights[0][4][j] * multiplyAmnt) + addAmnt);
     //               }
                    
					//for (int j = 0; j < weightLinesColF.Length; j++)
					//{
     //                   if (nets[i].weights[0][5][j] > 0)
     //                   {
     //                       weightLinesColF[j].color = Color.red;
     //                   }
     //                   else
     //                   {
     //                       weightLinesColF[j].color = Color.blue;
     //                   }

     //                   weightLinesColF[j].rectTransform.sizeDelta = new Vector2(weightLinesColF[j].rectTransform.sizeDelta.x, Mathf.Abs(nets[i].weights[0][5][j] * multiplyAmnt) + addAmnt);
     //               }
                    
					//for (int j = 0; j < weightLinesColG.Length; j++)
					//{
     //                   if (nets[i].weights[0][5][j] > 0)
     //                   {
     //                       weightLinesColG[j].color = Color.red;
     //                   }
     //                   else
     //                   {
     //                       weightLinesColG[j].color = Color.blue;
     //                   }

     //                   weightLinesColG[j].rectTransform.sizeDelta = new Vector2(weightLinesColG[j].rectTransform.sizeDelta.x, Mathf.Abs(nets[i].weights[0][5][j] * multiplyAmnt) + addAmnt);
     //               }
                    
					//for (int j = 0; j < weightLinesColH.Length; j++)
					//{
     //                   if (nets[i].weights[1][0][j] > 0)
     //                   {
     //                       weightLinesColH[j].color = Color.red;
     //                   }
     //                   else
     //                   {
     //                       weightLinesColH[j].color = Color.blue;
     //                   }

     //                   weightLinesColH[j].rectTransform.sizeDelta = new Vector2(weightLinesColH[j].rectTransform.sizeDelta.x, Mathf.Abs(nets[i].weights[1][0][j] * multiplyAmnt) + addAmnt);
     //               }
                    
					//for (int j = 0; j < weightLinesColI.Length; j++)
					//{
     //                   if (nets[i].weights[1][1][j] > 0)
     //                   {
     //                       weightLinesColI[j].color = Color.red;
     //                   }
     //                   else
     //                   {
     //                       weightLinesColI[j].color = Color.blue;
     //                   }

     //                   weightLinesColI[j].rectTransform.sizeDelta = new Vector2(weightLinesColI[j].rectTransform.sizeDelta.x, Mathf.Abs(nets[i].weights[1][1][j] * multiplyAmnt) + addAmnt);
     //               }
                    
					//for (int j = 0; j < weightLinesColJ.Length; j++)
					//{
     //                   if (nets[i].weights[1][2][j] > 0)
     //                   {
     //                       weightLinesColJ[j].color = Color.red;
     //                   }
     //                   else
     //                   {
     //                       weightLinesColJ[j].color = Color.blue;
     //                   }

     //                   weightLinesColJ[j].rectTransform.sizeDelta = new Vector2(weightLinesColJ[j].rectTransform.sizeDelta.x, Mathf.Abs(nets[i].weights[1][2][j] * multiplyAmnt) + addAmnt);
     //               }
                    
					//for (int j = 0; j < weightLinesColK.Length; j++)
					//{
     //                   if (nets[i].weights[1][3][j] > 0)
     //                   {
     //                       weightLinesColK[j].color = Color.red;
     //                   }
     //                   else
     //                   {
     //                       weightLinesColK[j].color = Color.blue;
     //                   }

     //                   weightLinesColK[j].rectTransform.sizeDelta = new Vector2(weightLinesColK[j].rectTransform.sizeDelta.x, Mathf.Abs(nets[i].weights[1][3][j] * multiplyAmnt) + addAmnt);
     //               }
                    
					//for (int j = 0; j < weightLinesColL.Length; j++)
					//{
     //                   if (nets[i].weights[1][4][j] > 0)
     //                   {
     //                       weightLinesColL[j].color = Color.red;
     //                   }
     //                   else
     //                   {
     //                       weightLinesColL[j].color = Color.blue;
     //                   }

     //                   weightLinesColL[j].rectTransform.sizeDelta = new Vector2(weightLinesColL[j].rectTransform.sizeDelta.x, Mathf.Abs(nets[i].weights[1][4][j] * multiplyAmnt) + addAmnt);
     //               }
                    
					//for (int j = 0; j < weightLinesColM.Length; j++)
					//{
     //                   if (nets[i].weights[1][5][j] > 0)
     //                   {
     //                       weightLinesColM[j].color = Color.red;
     //                   }
     //                   else
     //                   {
     //                       weightLinesColM[j].color = Color.blue;
     //                   }

     //                   weightLinesColM[j].rectTransform.sizeDelta = new Vector2(weightLinesColM[j].rectTransform.sizeDelta.x, Mathf.Abs(nets[i].weights[1][5][j] * multiplyAmnt) + addAmnt);
     //               }
                    
					//for (int j = 0; j < weightLinesColN.Length; j++)
					//{
     //                   if (nets[i].weights[2][0][j] > 0)
     //                   {
     //                       weightLinesColN[j].color = Color.red;
     //                   }
     //                   else
     //                   {
     //                       weightLinesColN[j].color = Color.blue;
     //                   }

     //                   weightLinesColN[j].rectTransform.sizeDelta = new Vector2(weightLinesColN[j].rectTransform.sizeDelta.x, Mathf.Abs(nets[i].weights[2][0][j] * multiplyAmnt) + addAmnt);
     //               }
                    
					////for (int j = 0; j < weightLinesColO.Length; j++)
					////{
     ////                   if (nets[i].weights[2][0][j] > 0)
     ////                   {
     ////                       weightLinesColO[j].color = Color.red;
     ////                   }
     ////                   else
     ////                   {
     ////                       weightLinesColO[j].color = Color.blue;
     ////                   }
     ////               }
                    
					////for (int j = 0; j < weightLinesColP.Length; j++)
					////{
     ////                   if (nets[i].weights[2][0][j] > 0)
     ////                   {
     ////                       weightLinesColP[j].color = Color.red;
     ////                   }
     ////                   else
     ////                   {
     ////                       weightLinesColP[j].color = Color.blue;
     ////                   }
     ////               }
                    
					////for (int j = 0; j < weightLinesColQ.Length; j++)
					////{
     ////                   if (nets[i].weights[2][0][j] > 0)
     ////                   {
     ////                       weightLinesColQ[j].color = Color.red;
     ////                   }
     ////                   else
     ////                   {
     ////                       weightLinesColQ[j].color = Color.blue;
     ////                   }
     ////               }
                    
					////for (int j = 0; j < weightLinesColR.Length; j++)
					////{
     ////                   if (nets[i].weights[2][0][j] > 0)
     ////                   {
     ////                       weightLinesColR[j].color = Color.red;
     ////                   }
     ////                   else
     ////                   {
     ////                       weightLinesColR[j].color = Color.blue;
     ////                   }
     ////               }
                    
					////for (int j = 0; j < weightLinesColS.Length; j++)
					////{
     ////                   if (nets[i].weights[2][0][j] > 0)
     ////                   {
     ////                       weightLinesColS[j].color = Color.red;
     ////                   }
     ////                   else
     ////                   {
     ////                       weightLinesColS[j].color = Color.blue;
     ////                   }
     ////               }
                    
					////for (int j = 0; j < weightLinesColT.Length; j++)
					////{
     ////                   if (nets[i].weights[2][0][j] > 0)
     ////                   {
     ////                       weightLinesColT[j].color = Color.red;
     ////                   }
     ////                   else
     ////                   {
     ////                       weightLinesColT[j].color = Color.blue;
     ////                   }
     ////               }
					//#endregion
				}
			}
        }

        if (isTraning == false)
        {
            amntLeft = populationSize;

            if (generationNumber == 0)
            {
                InitEntityNeuralNetworks();
            }
            else
            {
                nets.Sort();
                if (!runEffectiveLearning)
                {
                    //nets[0].topFitness = true;
                    for (int i = populationSize / 2; i < populationSize - 1; i++)
                    {
                        //nets[i] = new NeuralNetwork(nets[i + (populationSize / 2)]);
                        nets[i] = nets[populationSize - 1];
                        //nets[i].Mutate();
                        nets[i - populationSize / 2].Mutate();

                        //nets[i + (populationSize / 2)] = new NeuralNetwork(nets[i + (populationSize / 2)]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
                    }
                    //nets[0] = nets[populationSize - 1];
                }

                if (runEffectiveLearning)
				{
                    for (int i = 0; i < populationSize / 2; i++)
                    {
                        nets[i] = new NeuralNetwork(nets[i + (populationSize / 2)]);
                        nets[i + (populationSize / 2)].Mutate();                                  //Mutates best entities

                        nets[i + (populationSize / 2)] = new NeuralNetwork(nets[i + (populationSize / 2)]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
                    }
                }

				for (int i = 0; i < populationSize; i++)
                {
                    nets[i].SetFitness(0f);
                }
            }

            generationNumber++;
            topDistance = 0;
            isTraning = true;
            Invoke("Timer", 20);
            timer = 20;
            CreateEntityBodies();
		}

        amntLeft = populationSize;
		foreach (SnakeScript emt in entityList)
		{
            if (emt.failed)
            {
                amntLeft--;
            }

        }

        if (amntLeft <= 0)
		{
            isTraning = false;
            amntLeft = populationSize;
            timer = 20;
        }

        if (Input.GetMouseButtonDown(0))
        {
            leftMouseDown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            leftMouseDown = false;
        }

        if (leftMouseDown == true)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.transform.position = mousePosition;
        }

        if(timer > 0)
		{
            timer -= Time.deltaTime;
		}
    }

    void Timer()
    {
        if(timer <= 0)
        {
            isTraning = false;
        }
		else
		{
            Invoke("Timer", 20);
		}
    }

    private void CreateEntityBodies()
    {
        if (entityList != null)
        {
            for (int i = 0; i < entityList.Count; i++)
            {
                GameObject.Destroy(entityList[i].gameObject);
            }
        }

        entityList = new List<SnakeScript>();

        for (int i = 0; i < populationSize; i++)
        {
            SnakeScript snakeScript = ((GameObject)Instantiate(entityPrefab, new Vector3(0, 0.27f, 0), entityPrefab.transform.rotation)).GetComponent<SnakeScript>();
            snakeScript.Init(nets[i], target.transform);
            snakeScript.gameObject.name = "Creature " + i;
            entityList.Add(snakeScript);
        }

    }

    void InitEntityNeuralNetworks()
    {
        //population must be even, just setting it to 20 incase it's not
        if (populationSize % 2 != 0)
        {
            populationSize = populationSize - 1;
        }

        nets = new List<NeuralNetwork>();

        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers);
            net.Mutate();
            nets.Add(net);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Camera.main.transform.position + new Vector3(0, 0, 10), new Vector3(boardSize.x, boardSize.y, 1));
    }
}

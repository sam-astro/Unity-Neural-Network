using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetManagerConvo : MonoBehaviour
{
    private bool isTraining = false;
    public int populationSize = 10;
    private int generationNumber = 0;
	public int[] layers = new int[] { 50, 25, 25, 50 }; // Default No. of inputs and No. of outputs
	private List<NeuralNetwork> nets;
    [HideInInspector]
    public List<ConvoBot> entityList = null;

    public int amntLeft;

    public TMP_Text generationText;

    [HideInInspector]
    public float timer = 5;
    public float startTimer;

    public bool runEffectiveLearning;

    public Toggle learnMethodToggle;

    ConvoBot lastBest;

    public GameObject entityPrefab;

    void Update()
    {
        generationText.text = generationNumber.ToString();

        if (isTraining == false)
        {
            amntLeft = populationSize;

            if (generationNumber == 0)
            {
                InitEntityNeuralNetworks();
            }
            else
            {
                nets.Sort();
                GameObject.Find("Window_Graph").GetComponent<WindowGraph>().valueList.Add(nets[populationSize - 1].fitness);
                GameObject.Find("Window_Graph").GetComponent<WindowGraph>().NewEntry();
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
                    //for (int i = populationSize / 2; i < populationSize - 4; i++) //Gathers all but best 2 nets
                    //{
                    //    nets[i] = new NeuralNetwork(nets[populationSize - 1]);
                    //    nets[i].Mutate();
                    //}

                    for (int i = 2; i < (populationSize - 2) / 2; i++) //Gathers all but best 2 nets
                    {
                        nets[i] = new NeuralNetwork(nets[populationSize - 2]);
                        nets[i].Mutate();                                                    //Mutates new entities

                        nets[populationSize - 1] = new NeuralNetwork(nets[populationSize - 1]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
						nets[populationSize - 2] = new NeuralNetwork(nets[populationSize - 2]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
                        nets[populationSize - 2].Mutate();
                    }
                }

                for (int i = 0; i < populationSize; i++)
                {
                    nets[i].SetFitness(0f);
                }
            }

            //cameraObj.GetComponent<CameraFollow>().target = theWall.transform;
            generationNumber++;
            isTraining = true;
            Invoke("Timer", startTimer);
            timer = startTimer;
            CreateEntityBodies();
		}

        amntLeft = populationSize;
		foreach (ConvoBot emt in entityList)
		{
            if (emt.failed)
            {
                amntLeft--;
            }
        }

        if (amntLeft <= 0)
		{
            isTraining = false;
            amntLeft = populationSize;
            timer = startTimer;
            //cameraObj.GetComponent<CameraFollow>().target = theWall.transform;
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
            isTraining = false;
        }
		else
		{
            Invoke("Timer", startTimer);
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

        entityList = new List<ConvoBot>();

        for (int i = 0; i < populationSize; i++)
        {
            ConvoBot walkerScript = ((GameObject)Instantiate(entityPrefab, transform.position, entityPrefab.transform.rotation)).GetComponent<ConvoBot>();
            walkerScript.Init(nets[i]);
            entityList.Add(walkerScript);
        }

    }

    void InitEntityNeuralNetworks()
    {
        if (populationSize % 2 != 0)
        {
            populationSize++;
        }

        nets = new List<NeuralNetwork>();

        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers);
            net.Mutate();
            nets.Add(net);
        }
    }
}

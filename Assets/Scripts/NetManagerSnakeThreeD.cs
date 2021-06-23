using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetManagerSnakeThreeD : MonoBehaviour
{
    public GameObject entityPrefab;
    public GameObject target;
    private Vector3 wallOriginalPosition;

    private bool isTraning = false;
    public int populationSize = 10;
    private int generationNumber = 0;
	public int[] layers = new int[] { 7, 10, 10, 4 }; //No. of inputs and No. of outputs
	private List<NeuralNetwork> nets;
    [HideInInspector]
    public List<ThreeDSnake> entityList = null;

    public int amntLeft;

    public TMP_Text generationText;

    [Range(1f, 8)]
    public float timeScale = 1;

    public float topDistance;

    public Material[] placingMaterials;

    [HideInInspector]
    public float timer = 5;
    public float startTimer;

    public bool runEffectiveLearning;

    public Slider populationSlider;
    public Toggle learnMethodToggle;

    public GameObject cameraObj;
    public bool bodyStayUpright;

    ThreeDSnake lastBest;

    void Update()
    {
        generationText.text = generationNumber.ToString();
        Time.timeScale = timeScale;

        if (entityList != null)
        {
            foreach (ThreeDSnake scrpt in entityList)
            {
                if (scrpt.distanceTravelled > topDistance && !scrpt.failed && scrpt != null)
                {
                    topDistance = scrpt.distanceTravelled;
					cameraObj.GetComponent<CameraFollow>().target = scrpt.gameObject.transform.GetChild(0).transform;
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
                    for (int i = 0; i < (populationSize - 2) / 2; i++) //Gathers all but best 2 nets
                    {
                        nets[i] = new NeuralNetwork(nets[i + (populationSize - 2) / 2]);     //Copies weight values from top half networks to worst half
                        nets[i].Mutate();                                                    //Mutates new entities

                        nets[i + (populationSize - 2) / 2] = new NeuralNetwork(nets[populationSize - 1]);
                        nets[i + (populationSize - 2) / 2].Mutate();

						nets[populationSize - 1] = new NeuralNetwork(nets[populationSize - 1]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
						nets[populationSize - 2] = new NeuralNetwork(nets[populationSize - 2]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
					}
                }

                for (int i = 0; i < populationSize; i++)
                {
                    nets[i].SetFitness(0f);
                }
            }

            //cameraObj.GetComponent<CameraFollow>().target = theWall.transform;
            generationNumber++;
            topDistance = 0;
            isTraning = true;
            Invoke("Timer", startTimer);
            timer = startTimer;
            CreateEntityBodies();
		}

        amntLeft = populationSize;
		foreach (ThreeDSnake emt in entityList)
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
            isTraning = false;
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

        entityList = new List<ThreeDSnake>();

        for (int i = 0; i < populationSize; i++)
        {
            ThreeDSnake walkerScript = ((GameObject)Instantiate(entityPrefab, transform.position, entityPrefab.transform.rotation)).GetComponent<ThreeDSnake>();
            walkerScript.Init(nets[i], target.transform, bodyStayUpright);
            entityList.Add(walkerScript);
        }

    }

    void InitEntityNeuralNetworks()
    {
        if (populationSize % 2 != 0)
        {
            populationSize = populationSize + 1;
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

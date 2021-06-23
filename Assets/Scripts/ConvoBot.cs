using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ConvoBot : MonoBehaviour
{
	[HideInInspector]
	public NeuralNetwork net;

	public bool failed;

	string prompt;
	string answer;

	Prompt promptObject;

	char[] alphabet = { ' ', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', ' ' };

	GameObject outText;

	private void Start()
	{
		outText = GameObject.Find("NetOutText");
		promptObject = GameObject.Find("PromptObject").GetComponent<Prompt>();
	}

	void Awake()
	{
		failed = false;
	}

	void FixedUpdate()
	{
		if (!failed)
		{
			for (int l = 0; l < promptObject.AmountOfPrompts(); l++)
			{
				prompt = null;
				answer = null;

				if (outText.GetComponent<TMP_Text>().text.Contains(promptObject.GetPrompt(promptObject.AmountOfPrompts() - 1)))
					outText.GetComponent<TMP_Text>().text = "";
				prompt = promptObject.GetPrompt(l);

				float[] inputs = new float[50];
				for (int i = 0; i < prompt.Length && i < 50; i++)
				{
					inputs[i] = GetAlphabetNum(prompt.ToCharArray()[i]);
				}


				float[] outputs = net.FeedForward(inputs);
				for (int i = 0; i < 20; i++)
				{
					answer += alphabet[Mathf.Clamp(Mathf.RoundToInt(Mathf.Abs((1f + outputs[i]) * 27)), 0, 27)];
				}

				answer = answer.Replace(" ", "");
				int score = (35 - promptObject.StringSimilarity(answer));

				if (answer == prompt)
				{
					score += 100;
					Debug.Log("Answer acheived " + answer + " : " + prompt);
				}

				net.AddFitness(score);

				//Debug.Log(score + " : " + answer + " : " + prompt);
				if (!outText.GetComponent<TMP_Text>().text.Contains(prompt))
					outText.GetComponent<TMP_Text>().text = outText.GetComponent<TMP_Text>().text + "\n" + score + " : " + answer + " : " + prompt;

				failed = true;
			}
		}
	}

	int GetAlphabetNum(char checkStr)
	{
		for (int j = 0; j < alphabet.Length; j++)
		{
			if (checkStr == alphabet[j])
				return j;
		}
		return 0;
	}

	public void Init(NeuralNetwork net)
	{
		this.net = net;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		//Gizmos.DrawLine(wallSensor.position, body.transform.position);
	}
}


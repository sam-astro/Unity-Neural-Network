using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Prompt : MonoBehaviour
{
    string prompt;
	string correctAnswer;

	public bool CheckPrompt(string answer)
	{
		ReadList();
		if (answer == correctAnswer)
		{
			return true;
		}
		return false;
	}

	public string GetPrompt(int l)
	{
		StreamReader sr = File.OpenText("Assets/promptlist.txt");
		string[] fullFile = sr.ReadToEnd().Split('\n');

		prompt = fullFile[l].Split('#')[0];
		correctAnswer = fullFile[l].Split('#')[1];

		return prompt;
	}

	void ReadList()
	{
		StreamReader sr = File.OpenText("Assets/promptlist.txt");
		string[] fullFile = sr.ReadToEnd().Split('\n');

		int randomPromptNumber = Random.Range(0, fullFile.Length);

		prompt = fullFile[randomPromptNumber].Split('#')[0];
		correctAnswer = fullFile[randomPromptNumber].Split('#')[1];
	}

	public int AmountOfPrompts()
	{
		StreamReader sr = File.OpenText("Assets/promptlist.txt");
		string[] fullFile = sr.ReadToEnd().Split('\n');

		return fullFile.Length;
	}

	public int StringSimilarity(string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			if (string.IsNullOrEmpty(correctAnswer))
				return 0;
			return correctAnswer.Length;
		}

		if (string.IsNullOrEmpty(correctAnswer))
		{
			return s.Length;
		}

		int n = s.Length;
		int m = correctAnswer.Length;
		int[,] d = new int[n + 1, m + 1];

		// initialize the top and right of the table to 0, 1, 2, ...
		for (int i = 0; i <= n; d[i, 0] = i++) ;
		for (int j = 1; j <= m; d[0, j] = j++) ;

		for (int i = 1; i <= n; i++)
		{
			for (int j = 1; j <= m; j++)
			{
				int cost = (correctAnswer[j - 1] == s[i - 1]) ? 0 : 1;
				int min1 = d[i - 1, j] + 1;
				int min2 = d[i, j - 1] + 1;
				int min3 = d[i - 1, j - 1] + cost;
				d[i, j] = Mathf.Min(Mathf.Min(min1, min2), min3);
			}
		}
		return d[n, m];
	}
}

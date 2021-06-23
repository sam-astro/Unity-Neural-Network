using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
	[SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;

	public List<float> valueList;

	void Awake()
	{
		graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
	}

	public void NewEntry()
	{
		circleSprite = null;
		graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
		for (int i = 0; i < GameObject.Find("graphingObjects").transform.childCount - 1; i++)
		{
			Destroy(GameObject.Find("graphingObjects").transform.GetChild(i).gameObject);
		}
		if (valueList.Count >= 100)
		{
			valueList.RemoveAt(0);
		}
		ShowGraph(valueList);
	}

	GameObject CreateCircle(Vector2 anchoredPosition)
	{
		GameObject gameObject = new GameObject("point", typeof(Image));
		gameObject.transform.SetParent(graphContainer, false);
		gameObject.GetComponent<Image>().sprite = circleSprite;
		RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
		rectTransform.anchoredPosition = anchoredPosition;
		rectTransform.sizeDelta = new Vector2(11, 11);
		rectTransform.anchorMin = new Vector2(0, 0);
		rectTransform.anchorMax = new Vector2(0, 0);

		return gameObject;
	}

	private void ShowGraph(List<float> valueList)
	{
		RectTransform lastCircleGameObject = null;
		float graphHeight = graphContainer.sizeDelta.y;
		float graphWidth = graphContainer.sizeDelta.x;
		float highestNum = 0;
		foreach (float num in valueList)
		{
			if(num > highestNum)
			{
				highestNum = num;
			}
		}
		float yMaximum = highestNum * 4f;
		float xSize = graphWidth / valueList.Count;

		for (int i = 0; i < valueList.Count; i++)
		{
			float xPosition = xSize + i * xSize;
			float yPosition = (valueList[i] / yMaximum) * graphHeight;
			GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
			circleGameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0f);
			if (lastCircleGameObject != null)
			{
				CreateDotConnection(lastCircleGameObject.anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
			}
			lastCircleGameObject = circleGameObject.GetComponent<RectTransform>();
			circleGameObject.transform.SetParent(GameObject.Find("graphingObjects").transform, false);
		}
	}

	private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
	{
		GameObject gameObject = new GameObject("line", typeof(Image));
		gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.6f);
		RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
		Vector2 dir = (dotPositionB - dotPositionA).normalized;
		float distance = Vector2.Distance(dotPositionA, dotPositionB);
		rectTransform.anchorMin = new Vector2(0, 0);
		rectTransform.anchorMax = new Vector2(0, 0);
		rectTransform.sizeDelta = new Vector2(distance, 3);
		rectTransform.anchoredPosition = dotPositionA + dir * distance * 0.5f;
		rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
		gameObject.transform.SetParent(GameObject.Find("graphingObjects").transform, false);
	}
}

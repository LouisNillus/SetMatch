using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NS_Rank;

public class WindowGraph : MonoBehaviour
{
    public RankingSystem rankingSystem;

    [SerializeField]
    private Sprite circleSprite;
    [SerializeField, Range(0,20)]
    private float circleSize = 7f;

    private RectTransform graphContainer;
    private RectTransform labelXtemplate;
    private RectTransform labelYtemplate;
    private RectTransform xDashTemplate;
    private RectTransform yDashTemplate;

    public List<float> valueList = new List<float>();
    public List<GameObject> poubelle = new List<GameObject>();

    [SerializeField, Range(0, 100)]
    int gamesPlayed = 10;
    [SerializeField, Range(0, 100)]
    float xDistance = 20f;




    //Awake
    private void Awake()
    {
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
        labelXtemplate = graphContainer.Find("xLabel").GetComponent<RectTransform>();
        labelYtemplate = graphContainer.Find("yLabel").GetComponent<RectTransform>();
        xDashTemplate = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        yDashTemplate = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();
    }

    // Start
    void Start()
    {
        for(int i = 0; i< gamesPlayed; i++)
        {
            int j = UnityEngine.Random.Range(0, 100);
            valueList.Add(j);
        }
    }

    // Update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UpdateGraph();
        }
    }

    public void UpdateGraph()
    {
        foreach (GameObject go in poubelle)
        {
            Destroy(go);
        }
        valueList = rankingSystem.historyELO;
        ShowGraph(valueList);
    }

    public GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject go = new GameObject("Circle", typeof(Image));
        poubelle.Add(go);
        go.transform.SetParent(graphContainer, false);
        go.GetComponent<Image>().sprite = circleSprite;
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchoredPosition = anchoredPosition;
        rt.sizeDelta = new Vector2(circleSize, circleSize);
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(0, 0);
        return go;
    }

    public void ShowGraph(List<float> valueList)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 100f;

        GameObject lastGO = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = xDistance + i * xDistance;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject circleGO = CreateCircle(new Vector2(xPosition, yPosition));

            if(lastGO != null)
            {
                CreateDotConnection(lastGO.GetComponent<RectTransform>().anchoredPosition, circleGO.GetComponent<RectTransform>().anchoredPosition);
            }

            lastGO = circleGO;

            RectTransform labelX = Instantiate(labelXtemplate);
            poubelle.Add(labelX.gameObject);
            labelX.SetParent(graphContainer);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, - 7f);
            labelX.GetComponent<Text>().text = i.ToString();

            RectTransform dashX = Instantiate(yDashTemplate);
            poubelle.Add(dashX.gameObject);
            dashX.SetParent(graphContainer);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, -3f);
        }

        int ySeperators = 10;
        for(int i = 0; i <= ySeperators; i++)
        {
            RectTransform labelY = Instantiate(labelYtemplate);
            poubelle.Add(labelY.gameObject);
            labelY.SetParent(graphContainer);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / ySeperators;
            labelY.anchoredPosition = new Vector2(-7f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = Mathf.RoundToInt(normalizedValue * yMaximum).ToString();

            RectTransform dashY = Instantiate(xDashTemplate);
            poubelle.Add(dashY.gameObject);
            dashY.SetParent(graphContainer);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(-2.6f, normalizedValue * graphHeight);
        }
    }

    public void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject go = new GameObject("DotConnection", typeof(Image));
        poubelle.Add(go);
        go.transform.SetParent(graphContainer, false);
        go.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        RectTransform rt = go.GetComponent<RectTransform>();
        Vector2 direction = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rt.sizeDelta = new Vector2(distance, 3f);
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(0, 0);
        rt.anchoredPosition = dotPositionA + direction * distance * 0.5f;
        rt.localEulerAngles = new Vector3(0, 0, CodeMonkey.Utils.UtilsClass.GetAngleFromVectorFloat(direction));

    }


    /*  
        Truc sympa à faire :
        GameObject go = new GameObject("DotConnection", typeof(Image), typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(RankingSystem));
        UnityEditorInternal.ComponentUtility.CopyComponent(rk);
        UnityEditorInternal.ComponentUtility.PasteComponentValues(go.GetComponent<RankingSystem>());
    */
}

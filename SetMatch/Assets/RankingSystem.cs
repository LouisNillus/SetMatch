using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingSystem : MonoBehaviour
{
    public InputField rankA;
    public InputField rankB;
    public Toggle winA;
    public Button bakeButton;

    public Text ELOA;
    public Text ELOB;


    private int result;

    [Range(0, 10), SerializeField]
    private float k;

    [Range(0, 400), SerializeField]
    private int scale = 21;

    public float finalELOA;
    public float finalELOB;

    // Start
    void Start()
    {

    }

    // Update
    void Update()
    {
        if(finalELOA != 0f && finalELOB != 0f)
        {
            ELOA.text = finalELOA.ToString();
            ELOB.text = finalELOB.ToString();
        }


        if (winA.isOn == true)
        {
            winA.GetComponentInChildren<Text>().text = "Winner";
        }
        else
        {
            winA.GetComponentInChildren<Text>().text = "Loser";
        }

        Ranking();
    }

    public void Ranking()
    {
        if(float.TryParse(rankA.text , out float valueRankA) && float.TryParse(rankB.text, out float valueRankB))
        {
            valueRankA = Mathf.Clamp(valueRankA, 0f, 100f);
            if(valueRankA == 100f)
            {
                rankA.text = "100";
            }

            valueRankB = Mathf.Clamp(valueRankB, 0f, 100f);
            if (valueRankB == 100f)
            {
                rankB.text = "100";
            }

            if(bakeButton.IsActive() == true)
            {
                Bake(valueRankA, valueRankB);
            }
        }
    }

    public void Bake(float vA, float vB)
    {
            switch(winA.isOn)
            {
                case true:
                    finalELOA = vA + k * (1f - (1f /(1f + Mathf.Pow(10f, ((vB - vA) / scale)))));
                    finalELOB = vB + k * (0f- (1f / (1f + Mathf.Pow(10f, ((vA - vB) / scale)))));
                    break;

                case false:
                    finalELOA = vA + k * (0f - (1f / (1f + Mathf.Pow(10f, ((vB - vA) / scale)))));
                    finalELOB = vB + k * (1f - (1f / (1f + Mathf.Pow(10f, ((vA - vB) / scale)))));
                    break;
            }
    }
}

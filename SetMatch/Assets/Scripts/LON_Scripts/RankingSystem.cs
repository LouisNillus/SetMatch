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
    private bool canBake;

    public Text rankNameA;
    public Text rankNameB;

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

            if(canBake == true)
            {
                Bake(valueRankA, valueRankB);
                canBake = false;
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
                    finalELOA = Mathf.Round(finalELOA * 10) / 10;
                    finalELOB = Mathf.Round(finalELOB * 10) / 10;
                    rankA.text = finalELOA.ToString();
                    rankB.text = finalELOB.ToString();
                    SetRankNames(finalELOA, rankNameA);
                    SetRankNames(finalELOB, rankNameB);
                    break;

                case false:
                    finalELOA = vA + k * (0f - (1f / (1f + Mathf.Pow(10f, ((vB - vA) / scale)))));
                    finalELOB = vB + k * (1f - (1f / (1f + Mathf.Pow(10f, ((vA - vB) / scale)))));
                    finalELOA = Mathf.Round(finalELOA * 10) / 10;
                    finalELOB = Mathf.Round(finalELOB * 10) / 10;
                    rankA.text = finalELOA.ToString();
                    rankB.text = finalELOB.ToString();
                    SetRankNames(finalELOA, rankNameA);
                    SetRankNames(finalELOB, rankNameB);
                    break;
            }
    }

    public void SetRankNames(float eloToModify, Text textToModify)
    {
        if(eloToModify >= 0 && eloToModify < 20f)
        {
            textToModify.text = "Amateur";
        }
        else if(eloToModify >= 20f && eloToModify < 30f)
        {
            textToModify.text = "Professional";
        }
        else if (eloToModify >= 30f && eloToModify < 40f)
        {
            textToModify.text = "5th League";
        }
        else if (eloToModify >= 40f && eloToModify < 50f)
        {
            textToModify.text = "4th League";
        }
        else if (eloToModify >= 50f && eloToModify < 60f)
        {
            textToModify.text = "3rd League";
        }
        else if (eloToModify >= 60 && eloToModify < 70f)
        {
            textToModify.text = "2nd League";
        }
        else if (eloToModify >= 70f && eloToModify < 80f)
        {
            textToModify.text = "1st League";
        }
        else if (eloToModify >= 80f && eloToModify < 90f)
        {
            textToModify.text = "Master";
        }
        else if (eloToModify >= 90f && eloToModify <= 100f)
        {
            textToModify.text = "Legend";
        }
    }

    public void CanBake()
    {
        canBake = true;
    }
}
enum RankNames {Amateur, FifthLeague, FourthLeague, ThirdLeague, SecondLeague, FirstLeague, Master, Legend}

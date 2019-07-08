using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{

    [SerializeField]
    private int points = 50;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Joueur")
        {
            Controller.score += points;

            Destroy(this.gameObject);
        }
    }



}

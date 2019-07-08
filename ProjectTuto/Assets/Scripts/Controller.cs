using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    Transform playerPosition;

    [SerializeField]
    private float vitesse;

    public static int score;

    // Start is called before the first frame update
    void Start()
    {
        playerPosition = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            playerPosition.Translate(Vector3.right * Time.deltaTime * vitesse);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerPosition.Translate(Vector3.left * Time.deltaTime * vitesse);
        }
    }


}

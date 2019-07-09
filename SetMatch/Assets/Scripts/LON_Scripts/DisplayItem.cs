using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayItem : MonoBehaviour
{

    public CustomizableItem myItem;
    public Text itemName;

    Image myImage;


    // Start is called before the first frame update
    void Start()
    {
        myImage = this.GetComponent<Image>();
        myImage.sprite = myItem.sprite;
        myImage.color = myItem.color;
        itemName.text = myItem.itemName;
        itemName.color = myItem.nameColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

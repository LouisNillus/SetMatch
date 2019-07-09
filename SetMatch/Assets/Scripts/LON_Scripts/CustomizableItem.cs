using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class CustomizableItem : ScriptableObject
{
    public string itemName;
    public Color32 nameColor;

    public Sprite sprite;
    public Color32 color;

    public ItemType itemType;
    public ItemAccess itemAccess;

}
public enum ItemType {Ball, Lines, Ground}
public enum ItemAccess {Free, Event, Rank, Achievement}
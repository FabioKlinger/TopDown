using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject
{
    public string itemId;
    public string itemName;
    [TextArea] 
    public string itemDescription;
    public Sprite itemSprite;
    

}

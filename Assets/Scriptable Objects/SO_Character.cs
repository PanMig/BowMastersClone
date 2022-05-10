using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "character", menuName = "Characters")]
public class SO_Character : ScriptableObject
{
    public Sprite characterIcon;
    public GameObject projectilePrefab;
}

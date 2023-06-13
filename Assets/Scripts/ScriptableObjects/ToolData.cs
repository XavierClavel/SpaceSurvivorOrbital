using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToolData", menuName = "Space Survivor 2D/ToolData", order = 0)]
public class ToolData : ScriptableObject
{
    [Header("Tool parameters")]
    public int power = 15;
    public float reloadTime = 1f;
    public float range = 1f;

    [Header("Attractor parameters")]
    public float attractorRange = 1f;
    public float attractorForce = 5f;

    public Tool tool;

}

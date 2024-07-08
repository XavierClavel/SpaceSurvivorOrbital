using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RbLbItem : MonoBehaviour
{
    public Button LB;
    public Button RB;

    public RbLbObject ToRbLbObject() => new RbLbObject(LB, RB);
}

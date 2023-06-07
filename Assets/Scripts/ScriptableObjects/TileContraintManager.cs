using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/tileConstraintManager", order = 1)]
public class DistanceConstraintsManager : ScriptableObject
{
    public List<DistanceConstraint> distanceConstraints = new List<DistanceConstraint>();
    public List<DistanceConstraintGroup> distanceConstraintGroups = new List<DistanceConstraintGroup>();
    public List<SelfDistanceConstraint> selfDistanceConstraints = new List<SelfDistanceConstraint>();

}

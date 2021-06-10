using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KineticRotationDB", menuName = "ScriptableObjects/KineticRotationDB", order = 1)]
public class KineticRotationDB : ScriptableObject
{
    public float _maxVelocity = 700.0f;
    public float _deceleration = 950.0f;
    public float _filter = 0.5f;
}

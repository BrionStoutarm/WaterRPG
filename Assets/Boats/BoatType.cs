using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoatType", menuName = "ScriptableObjects/BoatType")]
public class BoatType : ScriptableObject
{
    public string boatTypeName;
    public Transform boatPrefab;
    public Transform[] deckPrefabs;
}

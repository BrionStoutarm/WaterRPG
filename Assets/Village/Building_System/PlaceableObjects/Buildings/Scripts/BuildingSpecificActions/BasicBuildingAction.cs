using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBuildingAction : PlaceableAction
{
    public BasicBuildingAction() { }
    public BasicBuildingAction(PlacedObject owner) { Owner = owner; }

    public BasicBuildingAction(PlacedObject owner, string actionName) {
        Owner = owner;
        ActionName = actionName;
    }

    public override int ActionActivate() {
        Debug.Log(Owner.gameObject.name + " activated!");
        return 1;
    }
}

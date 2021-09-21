using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableAction {
    public PlacedObject Owner {get; set;}

    public PlaceableAction() { }
    public PlaceableAction(PlacedObject owner) {
        this.Owner = owner;
    }
    public  PlaceableAction(PlacedObject owner, string actionName) { 
        this.Owner = owner;
        ActionName = actionName;
    }
    public string ActionName { get; set; }

    public virtual int ActionActivate() {
        return -1;
    }
}

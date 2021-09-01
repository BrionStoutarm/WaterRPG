using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="BuildingScriptableObject", menuName ="ScriptableObjects/BuildingType")]
public class PlaceableScriptableObject : ScriptableObject
{
    public static Dir GetNextDir(Dir dir) {
        switch(dir) {
            default:
            case Dir.Down:  return Dir.Left;
            case Dir.Left:  return Dir.Up;
            case Dir.Up:    return Dir.Right;
            case Dir.Right: return Dir.Down;
        }
    }
    public enum Dir {
        Down,
        Up,
        Left,
        Right
    }

    public string nameString;
    public Transform prefab;
    public Transform visual;
    public int width;
    public int height;

    public int GetRotationAngle(Dir dir) {
        switch(dir) {
            default:
            case Dir.Down:  return 0;
            case Dir.Left:  return 90;
            case Dir.Up:    return 180;
            case Dir.Right: return 270;
        }
    }

    public Vector2Int GetRotationOffset(Dir dir, int cellScale) {
        switch(dir) {
            default:
            case Dir.Down:  return new Vector2Int(0, 0);
            case Dir.Left:  return new Vector2Int(0, width * cellScale);
            case Dir.Up:    return new Vector2Int(width * cellScale, height *cellScale);
            case Dir.Right: return new Vector2Int(height * cellScale, 0);
        }
    }

    public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir, int cellScale) {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        switch (dir) {
            default:
            case Dir.Down:
            case Dir.Up:
                for (int x = 0; x < width * cellScale; x++) {
                    for (int y = 0; y < height * cellScale; y++) {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
            case Dir.Left:
            case Dir.Right:
                for (int x = 0; x < height * cellScale; x++) {
                    for (int y = 0; y < width * cellScale; y++) {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
        }
        return gridPositionList;
    }
}

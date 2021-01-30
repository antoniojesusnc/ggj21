using System;
using UnityEngine;

[System.Serializable]
public class CellInfo: IEquatable<CellInfo>
{
    public int PositionX => _positionX;
    public int _positionX;
    
    public int PositionY => _positionY;
    public int _positionY;
    
    public ECellType Type{ get; private set; }

    public CellInfo( Vector2Int cellPosition, ECellType newType)
    {
        _positionX = cellPosition.x;
        _positionY = cellPosition.y;
        Type = newType;
    }

    public void SetType(ECellType newType)
    {
        Type = newType;
    }
    
    public bool Equals(CellInfo other)
    {
        if (PositionX == other.PositionX && 
            PositionY == other.PositionY)
        {
            return true;
        }

        return false;
    }

    public bool IsWalkable
    {
        get
        {
            switch (Type)
            {
                case ECellType.None:
                case ECellType.Collectable:
                case ECellType.SafeArea:
                    return true;
                default:
                    return false;
            }
        }
    }

    public bool IsCollectable => Type == ECellType.Collectable;
}

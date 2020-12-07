using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomTypes
{
    LeftRight,
    LeftRightBottom,
    LeftRightTop,
    LeftRightTopBottom
};

public class RoomType : MonoBehaviour
{
    public int type;
    public RoomTypes rType;

    public void RoomDestruction()
    {
        Destroy(gameObject);
    }
}

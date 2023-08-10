using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static float Speed
    {
        get
        {
            return GameManager.instance.playerID == 0 ? 1.1f : 1.0f;
        }
    }

    public static float WeaponRate
    {
        get
        {
            return GameManager.instance.playerID == 1 ? 0.9f : 1.0f;
        }
    }

    public static float WeaponSpeed
    {
        get
        {
            return GameManager.instance.playerID == 2 ? 1.1f : 1.0f;
        }
    }

    public static float WeaponDamage
    {
        get
        {
            return GameManager.instance.playerID == 3 ? 10.0f : 0.0f;
        }
    }
}

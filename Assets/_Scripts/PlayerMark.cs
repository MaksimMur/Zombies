using Opsive.Shared.Input;
using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Traits;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMark : MonoBehaviour
{
    public static PlayerMark MARK { private set; get; }

    public void MakeDeathBehaviour()
    {
        this.GetComponent<Health>().Invincible = true;
        this.GetComponent<UnityInput>().DisableCursor = false;

    }
    private void Awake()
    {
        MARK = this;
    }
}

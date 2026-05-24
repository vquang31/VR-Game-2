using PDollarGestureRecognizer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DisplayInventory : NewMonoBehaviour
{
    [Header("Input")]
    public InputActionProperty triggerAction;

    [SerializeField]
    private GameObject inventoryUI;

    void OnEnable()
    {
        triggerAction.action.Enable();
    }

    void OnDisable()
    {
        triggerAction.action.Disable();
    }

    void Update()
    {
        float triggerValue = triggerAction.action.ReadValue<float>();
        if (triggerValue > 0.1f)
        {
            inventoryUI.transform.localScale = Vector3.one;
            Player.Instance.canDraw = false;
        }
        else
        {
            inventoryUI.transform.localScale = Vector3.zero;
            Player.Instance.canDraw = true;
        }
    }

}

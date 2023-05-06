using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
   [SerializeField] private PlayerController pc;
   [SerializeField] private CombatManager cm;
   [SerializeField] private CameraController cc;
   private Vector2 moveInput;
   private Vector2 lookInput;
   public bool grappleInput;

   public void OnMove(InputValue value)
   {
        pc.moveInput = value.Get<Vector2>();
   }

   public void OnJump()
   {
        pc.TryJump();
   }

   public void OnGrind()
   {
        pc.Grind();
   }

   public void OnGrapple(InputValue value)
   {
        if (value.isPressed)
            pc.Grapple();
        else 
            pc.EndGrapple();
        
   }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
        cc.Look(lookInput);
    }

    public void OnAttack()
    {
        cm.Attack();
    }

    public void OnParry()
    {
        cm.Parry();
    }

}

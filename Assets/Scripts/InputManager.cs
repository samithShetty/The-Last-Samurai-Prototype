using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
   [SerializeField] private PlayerController pc;
   [SerializeField] private CameraController cc;
   private Vector2 moveInput;
   private Vector2 lookInput;

   public void OnMove(InputValue value)
   {
        pc.moveInput = value.Get<Vector2>();
   }

   public void OnJump()
   {
        pc.TryJump();
   }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
        cc.Look(lookInput);
    }

    public void OnAttack()
    {
        pc.Attack();
    }

    public void OnParry()
    {
        pc.Parry();
    }

}

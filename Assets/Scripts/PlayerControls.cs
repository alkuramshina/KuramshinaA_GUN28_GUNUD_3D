using System;
using UnityEngine;

public class PlayerControls: MonoBehaviour
{
    [SerializeField] private Transform ballStartPlace;
    public Vector3 BallStartPosition => ballStartPlace.position;
    public float ThrowForce { get; private set; }
    public Vector3 ThrowAngle { get; private set; }
    
    private void Update()
    {
        switch (State)
        {
            case ControlState.Ready:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    State = ControlState.IsAiming;
                }
                break;
            case ControlState.IsAiming:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ThrowAngle = ballStartPlace.forward;
                    State = ControlState.IsForcing;
                }
                break;
            case ControlState.IsForcing:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ThrowForce = 5f;
                    State = ControlState.Thrown;
                }
                break;
            case ControlState.Thrown:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        Debug.Log(State);
        
        //Step 1: aim
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     _holdDownStartTime = Time.time;
        // }
        //
        // if (Input.GetKeyUp(KeyCode.Space))
        // {
        //     var holdDownTime = Time.time - _holdDownStartTime;
        //     _ball.Throw(ballStartPlace.forward * CalculateHoldDownForce(holdDownTime));
        // }
        
        //Step 2: throw
        
    }
    
    public ControlState State { get; private set; }
    public void SetReady() => State = ControlState.Ready;
    public enum ControlState
    {
        Ready,
        IsAiming,
        IsForcing,
        Thrown
    }
}
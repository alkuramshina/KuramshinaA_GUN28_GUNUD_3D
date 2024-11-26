using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private Transform ballStartPlace;
    [SerializeField] private Image arrow;

    public Vector3 BallStartPosition => ballStartPlace.position;
    private float _throwForce;
    private Vector3 _throwDirection;

    public Vector3 ChosenVelocity => _throwDirection * _throwForce;

    private void Update()
    {
        switch (State)
        {
            case ControlState.Ready:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    State = ControlState.IsAiming;
                    StartCoroutine(Aiming());
                }

                break;
            case ControlState.IsAiming:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _throwDirection = arrow.transform.rotation * Vector3.up;

                    State = ControlState.IsForcing;
                    StartCoroutine(Forcing());
                }

                break;
            case ControlState.IsForcing:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _throwForce = Mathf.Clamp(throwForceMax * arrow.fillAmount, throwForceMin, throwForceMax);
                    State = ControlState.Thrown;
                }

                break;
            case ControlState.Thrown:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private int _arrowRotationDirection = 1;
    [SerializeField] private float rotationSpeed = 50f;

    private IEnumerator Aiming()
    {
        while (State == ControlState.IsAiming)
        {
            if (arrow.transform.rotation.eulerAngles.y is > 59f and < 60f or > 300f and < 301f)
            {
                _arrowRotationDirection *= -1;
            }

            arrow.transform.RotateAround(ballStartPlace.position, Vector3.up,
                _arrowRotationDirection * rotationSpeed * Time.deltaTime);

            yield return null;
        }
    }

    [SerializeField] private float throwForceMin = 10f;
    [SerializeField] private float throwForceMax = 50f;
    private int _arrowFillingDirection = 1;

    private IEnumerator Forcing()
    {
        while (State == ControlState.IsForcing)
        {
            if (arrow.fillAmount is < .01f or > .99f)
            {
                _arrowFillingDirection *= -1;
            }

            arrow.fillAmount += _arrowFillingDirection * Time.deltaTime;
            yield return null;
        }
    }

    public ControlState State { get; private set; }

    public void SetReady()
    {
        State = ControlState.Ready;
        StopAllCoroutines();

        arrow.fillAmount = 1;
        arrow.transform.localPosition = Vector3.zero;
        arrow.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    public enum ControlState
    {
        Ready,
        IsAiming,
        IsForcing,
        Thrown
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.ARFoundation;

public class GameManager : MonoBehaviour
{
    public DroneController _DroneController;

    public Button _FlyButton;
    public Button _LandButton;
    public Joystick _Joystick;
    public Joystick _Joystick1;

    public GameObject _Controls;

    //AR
    public ARRaycastManager _RaycastManager;
    public ARPlaneManager _PlaneManager;
    List<ARRaycastHit> _HitResult = new List<ARRaycastHit>();

    public GameObject _Drone;
    public GameObject _ARobjects;
    struct DroneAnimationControls
    {
        public bool _moving;
        public bool _interpolatingAsc;
        public bool _interpolationgDesc;
        public float _axis;
        public float _direction;
    }

    DroneAnimationControls _MovingLeft;
    DroneAnimationControls _MovingBack;
    DroneAnimationControls _MovingUp;
    void Start()
    {
        _FlyButton.onClick.AddListener(EventOnClickFlyButton);
        _LandButton.onClick.AddListener(EventOnClickLandButton);
    }

    // Update is called once per frame
    void Update()
    {

        float speedX = _Joystick.Horizontal;
        float speedZ = _Joystick.Vertical;
        float speedY = _Joystick1.Vertical;


        _Drone.transform.rotation = _Drone.transform.rotation * Quaternion.AngleAxis(_Joystick1.Horizontal * 8, Vector3.up * Time.deltaTime);

        UpdateControls(ref _MovingUp);


        _DroneController.Move(speedX, speedZ, speedY);

        if (_DroneController.IsIdle())
        {
            UpdateAR();
        }

    }

    void UpdateAR()
    {
        Vector2 positionScreenSpace = Camera.current.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));

        _RaycastManager.Raycast(positionScreenSpace, _HitResult, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinBounds);

        if (_HitResult.Count > 0)
        {
            if (_PlaneManager.GetPlane(_HitResult[0].trackableId).alignment == UnityEngine.XR.ARSubsystems.PlaneAlignment.HorizontalUp)
            {
                Pose pose = _HitResult[0].pose;
                _Drone.transform.position = pose.position;
                _Drone.SetActive(true);
                
                _ARobjects.transform.position = pose.position;
                _ARobjects.SetActive(true);
            }
        }
    }

    void UpdateControls(ref DroneAnimationControls _controls)
    {
        if (_controls._moving || _controls._interpolatingAsc || _controls._interpolationgDesc)
        {
            if (_controls._interpolatingAsc)
            {
                _controls._axis += 0.05f;
                if (_controls._axis >= 1.0f)
                {
                    _controls._axis = 1.0f;
                    _controls._interpolatingAsc = false;
                    _controls._interpolationgDesc = true;
                }
            }
            else if (!_controls._moving)
            {
                _controls._axis -= 0.05f;

                if (_controls._axis <= 0.0f)
                {
                    _controls._axis = 0.0f;
                    _controls._interpolationgDesc = false;
                }
            }
        }
    }

    void EventOnClickFlyButton()
    {
        if (_DroneController.IsIdle())
        {
            _DroneController.TakeOff();
            _FlyButton.gameObject.SetActive(false);
            _LandButton.gameObject.SetActive(true);
            _Controls.SetActive(true);
        }
    }

    void EventOnClickLandButton()
    {
        if (_DroneController.IsFlying())
        {
            _DroneController.Land();
            _FlyButton.gameObject.SetActive(true);
            _LandButton.gameObject.SetActive(false);
            _Controls.SetActive(false);
        }
    }

    // Up Button
    public void EventOnUpButtonPressed()
    {
        _MovingUp._moving = true;
        _MovingUp._interpolatingAsc = true;
        _MovingUp._direction = 0.1f;
    }
    public void EventOnUpButtonReleased()
    {
        _MovingUp._moving = false;
    }
    // Down Button
    public void EventOnDownButtonPressed()
    {
        _MovingUp._moving = true;
        _MovingUp._interpolatingAsc = true;
        _MovingUp._direction = -0.1f;
    }
    public void EventOnDownButtonReleased()
    {
        _MovingUp._moving = false;
    }
}
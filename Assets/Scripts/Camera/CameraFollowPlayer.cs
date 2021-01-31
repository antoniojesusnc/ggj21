using UnityEngine;
using System.Collections;

namespace SpyGame
{
    public class CameraFollowPlayer : MonoBehaviour
    {
        public enum Axis
        {
            X,
            Y,
        }
        /// <summary>
        /// The camera dont move until the player go to the viewportborder, 
        /// when this happen the camera will move until the player in in the middle scren again;
        /// 
        /// The default Pos is some angle and distance from the player
        /// </summary>

        [Header("Camera Standard Properties")]
        public Vector3 _cameraAngle;
        public float _cameraDefaultDistance;
        public Vector3 _cameraOffset;

        [Header("Camera Configuration")]
        public float _timeInertia;

        [Range(0, 1)]
        public float _viewportBorderX;
        [Range(0, 1)]
        public float _viewportBorderY;

        private Transform _player;
        private Camera _camera;
        private Vector3 _viewportPlayerPos;

        private Vector3 _cameraSpeed;
        private Vector3 _playerSpeed;


        private Vector3 _debugPlayerPosition;
        private Vector3 _debugPlayerSpeedNormalized;

        // inertia Var
        private bool[] _doingInertia;
        private float[] _inertiaTimeStamp;
        private float[] _initialInerciaSpeed;

        private bool _enable = true;

        void Start()
        {
            Init();
        }
        
        public void Init()
        {
            _player = GameObject.FindWithTag("Player").transform;

            _camera = Camera.main;

            _camera.transform.rotation = CalulateStandardCameraRotation();
            _camera.transform.position = CalulateStandardCameraPosition(_player.transform.position);

            // init inertia vars
            _doingInertia = new bool[2];
            _inertiaTimeStamp = new float[2];
            _initialInerciaSpeed = new float[2];
            _debugPlayerPosition = _player.position;
        } // Init

        public Quaternion CalulateStandardCameraRotation()
        {
            return Quaternion.Euler(_cameraAngle);
        } // CalulateStandardCameraRotation

        public void ResetCameraInPlayer()
        {
            Vector3 result = _player.position - _camera.transform.forward * _cameraDefaultDistance;
            transform.position = result + _cameraOffset;
        }

        public Vector3 CalulateStandardCameraPosition(Vector3 relativeTo)
        {
            //Vector3 result = _player.transform.position - _camera.transform.forward * _cameraDefaultDistance;
            Vector3 result = relativeTo - _camera.transform.forward * _cameraDefaultDistance;
            result += _cameraOffset;

            return result;
        } // CalulateStandardCameraPosition

        void LateUpdate()
        {
            if (!_enable)
                return;

            _viewportPlayerPos = _camera.WorldToViewportPoint(_player.position);

            // taking the player speed
            UpdatePlayerSpeed();
            float xValue = 0;
            float yValue = 0;
            // first checking Axis X
            if (IsPlayerOutsideBorderX())
            {
                // take the camera Speed for Axis X
                xValue = UpdateCameraSpeed(Axis.X);
                if (_doingInertia[(int)Axis.X])
                    ResetInertia(Axis.X);
            } else
            {
                if (!_doingInertia[(int)Axis.X])
                    InitInertia(Axis.X);
                // taking the inertia value for Axis X
                xValue = UpdateCameraInertia(Time.deltaTime, Axis.X);
            }

            // second checking axis Y
            if (IsPlayerOutsideBorderY())
            {
                // take the camera Speed for Axis Y
                yValue = UpdateCameraSpeed(Axis.Y);
                if (_doingInertia[(int)Axis.Y])
                    ResetInertia(Axis.Y);
            } else
            {
                if (!_doingInertia[(int)Axis.Y])
                    InitInertia(Axis.Y);
                // taking the inertia value for Axis Y
                yValue = UpdateCameraInertia(Time.deltaTime, Axis.Y);
            }

            // updating camera position
            _cameraSpeed.Set(xValue, 0, yValue);
            UpdateCameraPosition();

            _debugPlayerPosition = _player.position;
        } // LateUpdate 


        private void UpdatePlayerSpeed()
        {
            _playerSpeed = _player.position - _debugPlayerPosition;
            _playerSpeed.Set(_playerSpeed.x, 0, _playerSpeed.z);
            _debugPlayerSpeedNormalized = _playerSpeed.normalized;
        } // UpdateCheckBordersViewport

        /// <summary>
        /// Updating the camera speed taking the same that the player
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        private float UpdateCameraSpeed(Axis axis)
        {
            return axis == Axis.X ? _playerSpeed.x : _playerSpeed.z;
        } // UpdateCameraSpeed

        /// <summary>
        /// Reset inertia values
        /// </summary>
        /// <param name="axis"></param>
        private void ResetInertia(Axis axis)
        {
            if (_doingInertia[(int)axis])
            {
                _doingInertia[(int)axis] = false;
            }
        } // ResetInertia

        /// <summary>
        /// Initialize the values for inertia
        /// </summary>
        /// <param name="axis"></param>
        private void InitInertia(Axis axis)
        {
            if (!_doingInertia[(int)axis])
            {
                _doingInertia[(int)axis] = true;
                _initialInerciaSpeed[(int)axis] = axis == Axis.X ? _cameraSpeed.x : _cameraSpeed.z;
                _inertiaTimeStamp[(int)axis] = 0;
            }
        } // InitInertia

        private float UpdateCameraInertia(float dt, Axis axis)
        {
            // if still doing inertia
            if (_inertiaTimeStamp[(int)axis] < _timeInertia)
            {
                _inertiaTimeStamp[(int)axis] += Time.deltaTime;

                return Mathf.Lerp(_initialInerciaSpeed[(int)axis], 0, _inertiaTimeStamp[(int)axis] / _timeInertia);
            } else
            {
                return 0;
            }
        } // UpdateCameraInertiaX

        private void UpdateCameraPosition()
        {
            if (_cameraSpeed.x != 0 || _cameraSpeed.z != 0)
            {
                transform.Translate(_cameraSpeed, Space.World);
            }
        } // UpdateCameraPosition

        private bool IsPlayerOutsideBorderX()
        {
            if (_debugPlayerSpeedNormalized.x < 0 && _viewportPlayerPos.x < _viewportBorderX)
                return true;
            else if (_debugPlayerSpeedNormalized.x > 0 && _viewportPlayerPos.x > 1 - _viewportBorderX)
                return true;

            return false;
        } // IsPlayerOutsideBorderX

        private bool IsPlayerOutsideBorderY()
        {
            if (_debugPlayerSpeedNormalized.z < 0 && _viewportPlayerPos.y < _viewportBorderY)
                return true;
            else if (_debugPlayerSpeedNormalized.z > 0 && _viewportPlayerPos.y > 1 - _viewportBorderY)
                return true;

            return false;
        } // IsPlayerOutsideBorderY

        public void SetEnable(bool enable)
        {
            _enable = enable;
            enabled = enabled;
        }
    }
}
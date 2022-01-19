using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;


    public class TelepathDrawing : MonoBehaviour
    {
        [Header("Hands")]
        [SerializeField]
        private GameObject _rightHand;

        [SerializeField]
        private GameObject _leftHand;

        [SerializeField]
        private LayerMask _layerMask;

        [SerializeField]
        private LineRenderer _line;

        [Header("PaintMarker and Cursor")]
        [SerializeField]
        private GameObject _paintMarkerPrefab;

        [SerializeField]
        private GameObject _paintMarker;

        [SerializeField]
        private GameObject _cursor;

        [SerializeField]
        private Vector3 _cursorVelocity = Vector3.zero;

        [SerializeField]
        private float _cursorSmoothTime = 1f;

        [SerializeField]
        private List<Vector3> _points;

        [SerializeField]
        private List<GameObject> _pointMarkers;

        [SerializeField]
        private LineRenderer _pathLine;

        [Header("Adjustable values")]
        [SerializeField]
        private float moveSpeed;


        [SerializeField]
        private float moveSpeedMultiplier;


        [SerializeField]
        private int index = 0;

        [SerializeField]
        private float _pointDistance = 1f;


        [SerializeField]
        public bool primaryValue = false;

        [SerializeField]
        public bool secondaryValue = false;

        [SerializeField]
        public bool triggerValue = false;

        [SerializeField]
        public Vector2 stick = new Vector2(0, 0);

        [SerializeField]
        private float _rotationSpeed = 0.5f;

        [SerializeField]
        private float _lineHeight = 0.02f;

        private bool _isHitting = false;

        [SerializeField]
        private float _maxRayDistance = 1;
        // Start is called before the first frame update
        void Start()
        {
            _points = new List<Vector3>();
            _pointMarkers = new List<GameObject>();
            _pathLine.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            //CheckAndStartMovement();
            var inputDevices = new List<UnityEngine.XR.InputDevice>();
            UnityEngine.XR.InputDevices.GetDevices(inputDevices);



            foreach (var device in inputDevices)
            {
                device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out primaryValue);
                device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out secondaryValue);
                device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue);
                device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out stick);
            }

            CalculateMaxRayDistance();
            EvaluateInput();
            CheckAndStartMovement();
            CalculateLine();

        }

        void CalculateMaxRayDistance()
        {
            if (_points.Count > 1)
            {
                _maxRayDistance = Vector3.Magnitude(this.transform.position - _points[_points.Count - 1]) + 5f;
            }
            else
            {
                _maxRayDistance = 2f;
            }
        }

        public void ResetLine()
        {
        Debug.Log("Resetting Line");
            if (_points.Count > 0)
            {
                foreach (var pointMarker in _pointMarkers)
                {
                    Destroy(pointMarker);
                }

                _pointMarkers = new List<GameObject>();
                _points = new List<Vector3>();

            }

            _pathLine.gameObject.SetActive(false);
        }

        private void EvaluateInput()
        {
            _cursor.SetActive(primaryValue && triggerValue);



            if (secondaryValue)
            {
                ResetLine();
            }
            if (primaryValue)
            {

                RaycastHit hit;
                if (Physics.Raycast(_rightHand.transform.position, _rightHand.transform.TransformDirection(Vector3.forward), out hit, _maxRayDistance, _layerMask))
                {
                    _line.SetPosition(0, _line.gameObject.transform.position);
                    _line.SetPosition(1, hit.point);
                    _line.gameObject.SetActive(true);
                    _cursor.SetActive(true);
                    _isHitting = true;
                }
                else
                {
                    _line.gameObject.SetActive(false);
                    if (_points.Count > 0)
                    {
                        _cursor.transform.position = _points[_points.Count - 1];
                    }
                    else
                    {
                        _cursor.transform.position = this.transform.position;
                    }
                    _isHitting = false;
                }

                if (triggerValue)
                {
                    if (_isHitting)
                    {
                        if (hit.transform.gameObject.layer == 3)
                        {


                            //Make cursor go behind ray
                            _cursor.transform.position = Vector3.SmoothDamp(_cursor.transform.position, hit.point, ref _cursorVelocity, _cursorSmoothTime);
                            if (_points.Count == 0)
                            {

                                Vector3 directionInCursor = (_cursor.transform.position - this.transform.position).normalized * 0.2f;
                                AddPointsToList(this.transform.position + directionInCursor);
                            }

                            if (Vector3.Distance(_cursor.transform.position, _points[_points.Count - 1]) >= _pointDistance)
                            {
                                AddPointsToList(_cursor.transform.position);
                            }
                        }
                    }

                }
                else
                {
                    _cursor.transform.position = hit.point;

                }

            }
            else
            {
                _line.gameObject.SetActive(false);
                if (_paintMarker != null)
                {
                    Destroy(_paintMarker);
                    _paintMarker = null;
                }
            }
        }

        private void CheckAndStartMovement()
        {
            if (stick.y > 0)
            {
                moveSpeedMultiplier = stick.y * 10;
            }
            else
            {
                moveSpeedMultiplier = 1;
            }
            if (_points.Count > 0 && !triggerValue)
            {
                if (this.transform.position != _points[index])
                {
                    float step = moveSpeed * Time.deltaTime * moveSpeedMultiplier;
                    this.transform.position = Vector3.MoveTowards(this.transform.position, _points[index], step);


                }
                else
                {

                    _points.Remove(_points[index]);
                    var todestroy = _pointMarkers[index];
                    _pointMarkers.Remove(_pointMarkers[index]);
                    Destroy(todestroy);

                    //index++;
                }
            }
            else
            {
                index = 0;
            }
        }
        private void AddPointsToList(Vector3 currentPos)
        {
            if (_points.Count == 0)
            {
                _points.Add(currentPos);
                GameObject paintMarker = Instantiate(_paintMarkerPrefab);
                paintMarker.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
                paintMarker.transform.position = currentPos;

                _pointMarkers.Add(paintMarker);
            }
            else
            {

                if (Vector3.Distance(currentPos, _points[_points.Count - 1]) >= 0.2f)
                {
                    _points.Add(currentPos);

                    GameObject paintMarker = Instantiate(_paintMarkerPrefab);
                    paintMarker.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
                    paintMarker.transform.position = currentPos;

                    _pointMarkers.Add(paintMarker);
                    //_pathLine.SetPosition(_points.FindIndex(x => x.Equals(currentPos)), currentPos);
                    _pathLine.positionCount = _points.Count + 1;
                    _pathLine.SetPosition(_points.IndexOf(currentPos), currentPos);
                }


            }
        }

        private void CalculateLine()
        {

            if (_points.Count > 0)
            {
                Vector3[] linePoints = new Vector3[_points.Count];
                for (int i = 0; i < linePoints.Length; i++)
                {
                    linePoints[i] = _points[i] + new Vector3(0, _lineHeight, 0);
                }
                _pathLine.gameObject.SetActive(true);
                _pathLine.positionCount = _points.Count;
                _pathLine.SetPositions(linePoints);
            }
            else
            {
                _pathLine.gameObject.SetActive(false);

            }

        }


    }


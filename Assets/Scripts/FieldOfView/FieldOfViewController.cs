using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FieldOfViewController : MonoBehaviour
{

	[SerializeField] private LayerMask _targetMask;
	[SerializeField] private LayerMask _obstacleMask;

	private Rigidbody _rigidbody;
	private Camera _camera;

	[SerializeField] private Vector2 _lockDirection;
	bool _isChangingLookDirection = true;
	float _timeStamp = 0;
	private Vector2 _nextLookDirection;

	public List<Transform> Targets => _targets;
	private List<Transform> _targets = new List<Transform>();

	private float _maskCutawayDst = .1f;
	[SerializeField] private MeshFilter _meshFilterCone;
	private Mesh _meshCone;
	[SerializeField] private MeshFilter _meshFilterCircle;
	private Mesh _meshCircle;

	private LevelController _levelController;
	private EnemyController _enemyController;

	public FieldOfViewData FieldOfViewData => _fieldOfViewData;

	[Header("Only use for debug proposal")] [SerializeField]
	private FieldOfViewData _fieldOfViewData;

	void Start()
	{
		_enemyController = GetComponentInParent<EnemyController>();
		_levelController = FindObjectOfType<LevelController>();

		_rigidbody = GetComponent<Rigidbody>();
		_camera = Camera.main;


		_meshCone = new Mesh();
		_meshCone.name = "FOVCone";
		_meshFilterCone.mesh = _meshCone;

		_meshCircle = new Mesh();
		_meshCircle.name = "FOVCircle";
		_meshFilterCircle.mesh = _meshCircle;

		StartCoroutine(FindTargetsEachTimeCo(0.1f));
	}

	void LateUpdate()
	{
		DrawAllFieldOfView();

		if (_isChangingLookDirection)
		{
			UpdateChangeLookDirection();
		}
	}

	private void UpdateChangeLookDirection()
	{
		_timeStamp += Time.deltaTime;

		Vector3 lookDirection3D = new Vector3(_lockDirection.x, 0, _lockDirection.y);
		Vector3 nextLookDirection3D = new Vector3(_nextLookDirection.x, 0, _nextLookDirection.y);

		Quaternion quaternionLookDirection3D = Quaternion.identity;
		if (lookDirection3D != Vector3.zero)
		{
			quaternionLookDirection3D = Quaternion.LookRotation(lookDirection3D, Vector3.up);
		}

		Quaternion quaternionNextLookDirection3D = Quaternion.identity;
		if (nextLookDirection3D != Vector3.zero)
		{
			quaternionNextLookDirection3D = Quaternion.LookRotation(nextLookDirection3D, Vector3.up);
		}

		var rotation = Quaternion.Lerp(
			quaternionLookDirection3D,
			quaternionNextLookDirection3D,
			_timeStamp /
			_enemyController.FieldOfViewData.timeToChangeLookDirection);

		transform.rotation = rotation;

		if (_timeStamp > _enemyController.FieldOfViewData.timeToChangeLookDirection)
		{
			FinishChangeLookDirection();
		}
	}

	public Vector3 DirectionFromAngle(float angleInDegrees, bool isGlobalAngle)
	{
		if (!isGlobalAngle)
		{
			angleInDegrees += transform.eulerAngles.y;
		}

		return new Vector3(
			Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),
			0,
			Mathf.Cos(angleInDegrees * Mathf.Deg2Rad)
		);
	}

	IEnumerator FindTargetsEachTimeCo(float delay)
	{
		while (true)
		{
			yield return new WaitForSeconds(delay);
			// cone
			FindVisibleTargets(_enemyController.FieldOfViewData.coneDistanceToDetect,
				_enemyController.FieldOfViewData.coneAngleToDetect);
			// circle around
			FindVisibleTargets(_enemyController.FieldOfViewData.circleDistanceToDetect,
				_enemyController.FieldOfViewData.circleAngleToDetect);
		}
	}

	private void FindVisibleTargets(float distance, float angle)
	{
		_targets.Clear();
		Collider[] targetsInViewRadius =
			Physics.OverlapSphere(transform.position, distance, _targetMask);

		for (int i = 0; i < targetsInViewRadius.Length; i++)
		{
			Transform target = targetsInViewRadius[i].transform;
			Vector3 directionToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
			{
				float distanceToTarget = Vector3.Distance(transform.position, target.position);
				if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstacleMask))
				{
					_levelController.CharacterDetected(distanceToTarget);
					_targets.Add(target);
				}
			}
		}
	}

	void DrawAllFieldOfView()
	{
		DrawFieldOfView(_enemyController.FieldOfViewData.coneAngleToShow,
			_enemyController.FieldOfViewData.coneDistanceToShow, _meshCone);

		DrawFieldOfView(_enemyController.FieldOfViewData.circleAngleToShow,
			_enemyController.FieldOfViewData.circleDistanceToShow, _meshCircle);

	}

	void DrawFieldOfView(float angle, float distance, Mesh mesh)
	{

		int stepCount = Mathf.RoundToInt(angle *
		                                 _enemyController.FieldOfViewData.accuracy);
		float stepAngleSize = angle / stepCount;
		List<Vector3> viewPoints = new List<Vector3>();
		ViewCastInfo oldViewCast = new ViewCastInfo();
		for (int i = 0; i <= stepCount; i++)
		{
			float viewAngle = transform.eulerAngles.y - (angle / 2) +
			                  (stepAngleSize * i);
			ViewCastInfo newViewCast = ViewCast(viewAngle, distance);

			if (i > 0)
			{
				bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.Distance - newViewCast.Distance) >
				                                _enemyController.FieldOfViewData.edgeDistTreshold;
				if (oldViewCast.Hit != newViewCast.Hit ||
				    (oldViewCast.Hit && newViewCast.Hit && edgeDstThresholdExceeded))
				{
					EdgeInfo edge = FindEdge(oldViewCast, newViewCast, distance);
					if (edge.PointA != Vector3.zero)
					{
						viewPoints.Add(edge.PointA);
					}

					if (edge.PointB != Vector3.zero)
					{
						viewPoints.Add(edge.PointB);
					}
				}

			}


			viewPoints.Add(newViewCast.Point);
			oldViewCast = newViewCast;
		}

		int vertexCount = viewPoints.Count + 1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount - 2) * 3];

		vertices[0] = Vector3.zero;
		for (int i = 0; i < vertexCount - 1; i++)
		{
			vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]) + Vector3.forward * _maskCutawayDst;

			if (i < vertexCount - 2)
			{
				triangles[i * 3] = 0;
				triangles[i * 3 + 1] = i + 1;
				triangles[i * 3 + 2] = i + 2;
			}
		}

		mesh.Clear();

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
	}

	EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast, float distance)
	{
		float minAngle = minViewCast.Angle;
		float maxAngle = maxViewCast.Angle;
		Vector3 minPoint = Vector3.zero;
		Vector3 maxPoint = Vector3.zero;

		for (int i = 0; i < _enemyController.FieldOfViewData.edgeResolveIterations; i++)
		{
			float angle = (minAngle + maxAngle) / 2;
			ViewCastInfo newViewCast = ViewCast(angle, distance);

			bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.Distance - newViewCast.Distance) >
			                                _enemyController.FieldOfViewData.edgeDistTreshold;
			if (newViewCast.Hit == minViewCast.Hit && !edgeDstThresholdExceeded)
			{
				minAngle = angle;
				minPoint = newViewCast.Point;
			}
			else
			{
				maxAngle = angle;
				maxPoint = newViewCast.Point;
			}
		}

		return new EdgeInfo(minPoint, maxPoint);
	}


	ViewCastInfo ViewCast(float globalAngle, float distance)
	{
		Vector3 dir = DirFromAngle(globalAngle, true);
		RaycastHit hit;

		if (Physics.Raycast(transform.position, dir, out hit, distance, _obstacleMask))
		{
			return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
		}
		else
		{
			return new ViewCastInfo(false, transform.position + dir * distance, distance, globalAngle);
		}
	}

	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal)
		{
			angleInDegrees += transform.eulerAngles.y;
		}

		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

	public struct ViewCastInfo
	{
		public bool Hit { get; private set; }
		public Vector3 Point { get; private set; }
		public float Distance { get; private set; }
		public float Angle { get; private set; }

		public ViewCastInfo(bool newHit, Vector3 newPoint, float newDistance, float newAngle)
		{
			Hit = newHit;
			Point = newPoint;
			Distance = newDistance;
			Angle = newAngle;
		}
	}

	public struct EdgeInfo
	{
		public Vector3 PointA { get; private set; }
		public Vector3 PointB { get; private set; }

		public EdgeInfo(Vector3 newPointA, Vector3 newPointB)
		{
			PointA = newPointA;
			PointB = newPointB;
		}
	}

	public void SetLookDirection(Vector2 newLockDirection)
	{
		if (_isChangingLookDirection || _lockDirection == newLockDirection)
		{
			return;
		}

		StartChangeLookDirection(newLockDirection);
	}

	private void StartChangeLookDirection(Vector3 newLockDirection)
	{
		_nextLookDirection = newLockDirection;
		_isChangingLookDirection = true;
		_timeStamp = 0;
	}

	void FinishChangeLookDirection()
	{
		_isChangingLookDirection = false;
		_lockDirection = _nextLookDirection;
	}
}
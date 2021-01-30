using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class FieldOfViewController : MonoBehaviour
{

	[SerializeField] private LayerMask _targetMask;
	[SerializeField] private LayerMask _obstacleMask;

	private Rigidbody _rigidbody;
	private Camera _camera;

	private Vector2Int _lockDirection;

	public List<Transform> Targets => _targets;
	private List<Transform> _targets = new List<Transform>();

	private float _maskCutawayDst = .1f;
	private MeshFilter _meshFilter;
	private Mesh _mesh;

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


		_meshFilter = GetComponent<MeshFilter>();
		_mesh = new Mesh();
		_mesh.name = "FOV";
		_meshFilter.mesh = _mesh;

		StartCoroutine(FindTargetsEachTimeCo(0.1f));
	}

	void LateUpdate()
	{
		DrawFieldOfView();
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
			FindVisibleTargets();
		}
	}

	private void FindVisibleTargets()
	{
		_targets.Clear();
		Collider[] targetsInViewRadius =
			Physics.OverlapSphere(transform.position, _enemyController.FieldOfViewData.distanceToDetect, _targetMask);

		for (int i = 0; i < targetsInViewRadius.Length; i++)
		{
			Transform target = targetsInViewRadius[i].transform;
			Vector3 directionToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle(transform.forward, directionToTarget) < _enemyController.FieldOfViewData.angleToDetect / 2)
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

	void DrawFieldOfView()
	{
		int stepCount = Mathf.RoundToInt(_enemyController.FieldOfViewData.angleToShow *
		                                 _enemyController.FieldOfViewData.presition);
		float stepAngleSize = _enemyController.FieldOfViewData.angleToShow / stepCount;
		List<Vector3> viewPoints = new List<Vector3>();
		ViewCastInfo oldViewCast = new ViewCastInfo();
		for (int i = 0; i <= stepCount; i++)
		{
			float angle = transform.eulerAngles.y - _enemyController.FieldOfViewData.angleToShow / 2 +
			              stepAngleSize * i;
			ViewCastInfo newViewCast = ViewCast(angle);

			if (i > 0)
			{
				bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.Distance - newViewCast.Distance) >
				                                _enemyController.FieldOfViewData.edgeDistTreshold;
				if (oldViewCast.Hit != newViewCast.Hit ||
				    (oldViewCast.Hit && newViewCast.Hit && edgeDstThresholdExceeded))
				{
					EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
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

		_mesh.Clear();

		_mesh.vertices = vertices;
		_mesh.triangles = triangles;
		_mesh.RecalculateNormals();
	}

	EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
	{
		float minAngle = minViewCast.Angle;
		float maxAngle = maxViewCast.Angle;
		Vector3 minPoint = Vector3.zero;
		Vector3 maxPoint = Vector3.zero;

		for (int i = 0; i < _enemyController.FieldOfViewData.edgeResolveIterations; i++)
		{
			float angle = (minAngle + maxAngle) / 2;
			ViewCastInfo newViewCast = ViewCast(angle);

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


	ViewCastInfo ViewCast(float globalAngle)
	{
		Vector3 dir = DirFromAngle(globalAngle, true);
		RaycastHit hit;

		if (Physics.Raycast(transform.position, dir, out hit, _enemyController.FieldOfViewData.distanceToShow,
			_obstacleMask))
		{
			return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
		}
		else
		{
			return new ViewCastInfo(false, transform.position + dir * _enemyController.FieldOfViewData.distanceToShow,
				_enemyController.FieldOfViewData.distanceToShow, globalAngle);
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
		public bool Hit{ get; private set; }
		public Vector3 Point{ get; private set; }
		public float Distance{ get; private set; }
		public float Angle{ get; private set; }

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
		public Vector3 PointB{ get; private set; }

		public EdgeInfo(Vector3 newPointA, Vector3 newPointB)
		{
			PointA = newPointA;
			PointB = newPointB;
		}
	}
}
using UnityEngine;

[RequireComponent(typeof(SnakePart))]
public class SnakeHead : MonoBehaviour
{
	[SerializeField]
	private SnakePart partPfb;

	[SerializeField]
	private int count;

	[SerializeField]
	private float distance;

	[Space]
	[Header("FollowTarget")]
	[Space]
	[SerializeField]
	private bool followTarget;

	public Transform target;

	public float rotateSpeed;

	[SerializeField]
	private float minDistance;

	[Space]
	[Header("Debug")]
	[Space]
	[SerializeField]
	private bool debug;

	private SnakePart headPart;

	private SnakePart partTemp;

	private SnakePart newPartTemp;

	private RoadLine currentRoadline;

	private RoadLine tempHeadRoad;

	private Vector3 currentStartPoint;

	private RoadLine tempRoad;

	private const float newRoadMinDistance = 0.3f;

	private Vector3 lastRoadPosMark;

	public SnakePart Part => headPart;

	private void Awake()
	{
		lastRoadPosMark = base.transform.position;
		headPart = GetComponent<SnakePart>();
		headPart.ActivePart(this);
	}

	private void Start()
	{
		tempHeadRoad = new RoadLine(base.transform.position, -base.transform.up, (float)count * distance, null);
		for (int i = 0; i < count; i++)
		{
			partTemp = newPartTemp;
			newPartTemp = UnityEngine.Object.Instantiate(partPfb, base.transform.position - (i + 1) * base.transform.forward * distance, base.transform.rotation);
			newPartTemp.ActivePart(this);
			if (i == 0)
			{
				newPartTemp.lastPart = headPart;
				headPart.nextPart = newPartTemp;
			}
			else
			{
				partTemp.nextPart = newPartTemp;
				newPartTemp.lastPart = partTemp;
			}
		}
		tempHeadRoad = new RoadLine(base.transform.position, -base.transform.up, (float)count * distance, null);
		AddRoadByDistance(-1f);
	}

	private void UpdateTempHeadRoad()
	{
		tempHeadRoad.startPoint = base.transform.position;
		tempHeadRoad.direction = -base.transform.up;
		if (tempHeadRoad.nextRoadLine != null)
		{
			tempHeadRoad.length = (base.transform.position - tempHeadRoad.nextRoadLine.startPoint).magnitude;
			tempHeadRoad.length = (tempHeadRoad.startPoint - tempHeadRoad.nextRoadLine.startPoint).magnitude;
		}
	}

	private void Update()
	{
		AddRoadByDistance(0.3f);
		UpdateTempHeadRoad();
		MoveParts();
		if (debug)
		{
			Debuug();
		}
	}

	private void AddRoadByDistance(float _distance)
	{
		if (Vector3.Distance(lastRoadPosMark, base.transform.position) >= _distance)
		{
			lastRoadPosMark = base.transform.position;
			AddRoad();
		}
	}

	private void AddRoad()
	{
		tempRoad = new RoadLine(base.transform.position, -base.transform.up, 0f, tempHeadRoad);
		tempHeadRoad = tempRoad;
	}

	private void Debuug()
	{
		UnityEngine.Debug.Log("RoadCount=" + CountingRoad(tempHeadRoad).ToString());
	}

	private int CountingRoad(RoadLine road, int count = 0)
	{
		count++;
		if (road.nextRoadLine != null)
		{
			return CountingRoad(road.nextRoadLine, count);
		}
		return count;
	}

	private void MoveParts()
	{
		currentRoadline = tempHeadRoad;
		currentStartPoint = base.transform.position;
		SetRemainPartTartget(headPart.nextPart);
	}

	private void SetRemainPartTartget(SnakePart _part)
	{
		if (_part != null)
		{
			_part.SetTarget(FindPositionOnRoad(currentRoadline, currentStartPoint, distance, _part.nextPart == null));
			if (_part.nextPart != null)
			{
				SetRemainPartTartget(_part.nextPart);
			}
		}
	}

	private Vector3 FindPositionOnRoad(RoadLine _road, Vector3 _startPoint, float _remainLength, bool removeRestRoads)
	{
		currentRoadline = _road;
		float num = _remainLength - (_road.length - (_startPoint - _road.startPoint).magnitude);
		if (num > 0f)
		{
			if (_road.nextRoadLine != null)
			{
				currentStartPoint = FindPositionOnRoad(_road.nextRoadLine, _road.nextRoadLine.startPoint, num, removeRestRoads);
			}
			else
			{
				currentStartPoint = _startPoint + _road.direction * _remainLength;
			}
		}
		else
		{
			currentStartPoint = _startPoint + _road.direction * _remainLength;
			if (removeRestRoads)
			{
				_road.nextRoadLine = null;
			}
		}
		return currentStartPoint;
	}

	private Vector3 FindPositionFromHead(RoadLine _road, float _remainLength, bool removeRestRoads)
	{
		if (_remainLength > _road.length)
		{
			if (_road.nextRoadLine == null)
			{
				return _road.startPoint + _road.direction * _remainLength;
			}
			return FindPositionFromHead(_road.nextRoadLine, _remainLength - _road.length, removeRestRoads);
		}
		if (removeRestRoads)
		{
			_road.nextRoadLine = null;
		}
		return _road.startPoint + _road.direction * _remainLength;
	}
}

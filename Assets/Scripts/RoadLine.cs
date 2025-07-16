using UnityEngine;

public class RoadLine
{
	public Vector3 startPoint;

	public Vector3 direction;

	public float length;

	public RoadLine nextRoadLine;

	public Vector3 endPoint => startPoint + direction * length;

	public RoadLine(Vector3 _startPoint, Vector3 _direction, float _length, RoadLine _nextRoadLine)
	{
		startPoint = _startPoint;
		direction = _direction;
		length = _length;
		nextRoadLine = _nextRoadLine;
	}
}

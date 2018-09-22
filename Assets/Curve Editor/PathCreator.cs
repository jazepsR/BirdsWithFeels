using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour {
	[Header("Shift + click to add road segement")]
    [HideInInspector]
    public Path path;

	public Color anchorCol = Color.red;
    public Color controlCol = Color.white;
    public Color segmentCol = Color.green;
    public Color selectedSegmentCol = Color.yellow;
    public float anchorDiameter = .1f;
    public float controlDiameter = .075f;
    public bool displayControlPoints = true;
	Vector3 lastPosition;
    public void CreatePath()
    {
		lastPosition = transform.position;
        path = new Path(transform.position);
    }

    void Reset()
    {
        CreatePath();
    }
	public void UpdatePosition()
	{
		if (lastPosition == null)
			lastPosition = transform.position;
		Vector3 delta = transform.position - lastPosition;
		transform.position = lastPosition;
		path.UpdatePoints(delta);
		lastPosition = transform.position;
	}
}

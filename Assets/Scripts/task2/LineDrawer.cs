using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public GameObject linePrefab;
    private LineRenderer currentLine;
    private Vector2 lastMousePos;
    public ParticleSystem particleEffect;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrawing();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndDrawing();
        }

        if (currentLine != null && Input.GetMouseButton(0))
        {
            UpdateLine();
        }
    }

    private void StartDrawing()
    {
        GameObject lineObj = Instantiate(linePrefab);
        currentLine = lineObj.GetComponent<LineRenderer>();
        lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentLine.positionCount = 1;
        currentLine.SetPosition(0, lastMousePos);
    }

    private void UpdateLine()
    {
        Vector2 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Vector2.Distance(currentMousePos, lastMousePos) > 0.1f)
        {
            currentLine.positionCount++;
            currentLine.SetPosition(currentLine.positionCount - 1, currentMousePos);
        }
    }

    private void EndDrawing()
    {
        if (currentLine != null)
        {
            Destroy(currentLine.gameObject);
            CheckForCircleIntersections();
        }
    }

    private void CheckForCircleIntersections()
    {
        GameObject[] circles = GameObject.FindGameObjectsWithTag("Circle");
        foreach (GameObject circle in circles)
        {
            if (currentLine != null && LineIntersectsCircle(currentLine, circle))
            {
                Destroy(circle);
            }
        }
    }

    private bool LineIntersectsCircle(LineRenderer line, GameObject circle)
    {
        Vector2 circleCenter = circle.transform.position;
        float circleRadius = circle.GetComponent<SpriteRenderer>().bounds.extents.x;
        for (int i = 1; i < line.positionCount; i++)
        {
            Vector2 p1 = line.GetPosition(i - 1);
            Vector2 p2 = line.GetPosition(i);
            if (IsPointInCircle(p1, circleCenter, circleRadius) || IsPointInCircle(p2, circleCenter, circleRadius))
            {
                return true;
            }
            if (IntersectsCircleSegment(p1, p2, circleCenter, circleRadius))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsPointInCircle(Vector2 point, Vector2 circleCenter, float circleRadius)
    {
        return Vector2.Distance(point, circleCenter) <= circleRadius;
    }

    private bool IntersectsCircleSegment(Vector2 p1, Vector2 p2, Vector2 circleCenter, float circleRadius)
    {
        float distToSegment = DistanceToLineSegment(circleCenter, p1, p2);
        return distToSegment <= circleRadius;
    }

    private float DistanceToLineSegment(Vector2 point, Vector2 p1, Vector2 p2)
    {
        Vector2 p1ToP2 = p2 - p1;
        float lineLength = p1ToP2.magnitude;
        Vector2 dir = p1ToP2.normalized;
        float dot = Vector2.Dot(point - p1, dir);
        dot = Mathf.Clamp(dot, 0f, lineLength);
        Vector2 closestPoint = p1 + dir * dot;
        return Vector2.Distance(point, closestPoint);
    }
}

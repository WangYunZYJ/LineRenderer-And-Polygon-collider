using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Private Fields

    private Vector3 mousePos;
    private LineRenderer lineRenderer = null;
    private bool firstClick = false;
    private int index = 0;
    private Vector3 screenPos;
    private GameObject currObj;

    private List<Vector3> touchPos = new List<Vector3>();

    private Vector3 lastTouch;
    private Vector3 currTouch;
    #endregion

    #region Public Fields
    [Tooltip("minTouchDistance Control")]
    public float minTouchDis = 0.1f;
    #endregion

    #region MonoBehavior Callbacks

    private void Awake()
    {
        firstClick = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !firstClick)
        {
            touchPos.Clear();
            firstClick = true;
            mousePos = Input.mousePosition;

            currTouch = Camera.main.ScreenToWorldPoint(mousePos);
            currTouch.z = 0;

            CreateLineRenderer();
            index = 0;
        }
        else if (Input.GetMouseButton(0))
        {
            lastTouch = currTouch;
            currTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currTouch.z = 0;
            if (Vector3.Distance(lastTouch, currTouch) > minTouchDis)
            {
                touchPos.Add(currTouch);
                index++;
                lineRenderer.positionCount = index;
                lineRenderer.SetPosition(index - 1, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 50)));
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {

            currTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currTouch.z = 0;
            index++;
            lineRenderer.positionCount = index;
            lineRenderer.SetPosition(index - 1, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 50)));

            touchPos.Add(currTouch);

            firstClick = false;
            currObj.AddComponent<Rigidbody2D>();
            AddPolygonCollider();
        }
    }

    #endregion

    #region Common Functions

    private void CreateLineRenderer()
    {
        var obj = Resources.Load("Line") as GameObject;
        screenPos = Camera.main.WorldToScreenPoint(obj.transform.position);
        var line = Instantiate(obj, obj.transform.position, obj.transform.rotation);
        currObj = line;
        lineRenderer = line.GetComponent<LineRenderer>();
    }

    private void AddPolygonCollider()
    {
        var collider = currObj.AddComponent<PolygonCollider2D>();
        var colliderPos = GetColliderPos();
        collider.SetPath(0, colliderPos.ToArray());
    }

    private List<Vector2> GetColliderPos()
    {
        float colliderWidth = lineRenderer.startWidth;
        List<Vector2> corrPos = new List<Vector2>();
        List<Vector2> ans = new List<Vector2>();
        foreach(var pos in touchPos)
        {
            corrPos.Add(pos);
        }

        for(int i = 0; i < corrPos.Count - 1; ++i)
        {
            var direction = corrPos[i + 1] - corrPos[i];
            Vector2 normal = Vector3.Cross(direction, Vector3.forward).normalized;
            var pos1 = corrPos[i] + colliderWidth / 2 * normal;
            var pos2 = corrPos[i] - colliderWidth / 2 * normal;
            ans.Insert(0, pos2);
            ans.Add(pos1);
            if(i == corrPos.Count - 2)
            {
                var _pos1 = corrPos[i + 1] + colliderWidth / 2 * normal;
                var _pos2 = corrPos[i + 1] - colliderWidth / 2 * normal;
                ans.Insert(0, _pos2);
                ans.Add(pos1);
            }
        }
        return ans;
    }

    #endregion
}

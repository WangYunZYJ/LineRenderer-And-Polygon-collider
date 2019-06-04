using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Private Fields

    private Vector3 mousePos;
    private LineRenderer lineRenderer =null;
    private bool firstClick = false;
    private int index = 0;
    private Vector3 screenPos;
    private GameObject currObj;
    private static GameManager _instance;
    private List<Vector3> touchPos = new List<Vector3>();

    private Vector3 lastTouch;
    private Vector3 currTouch;

    private bool canDraw = false;

    public List<GameObject> lines = new List<GameObject>();
    #endregion

    #region Public Fields
    [Tooltip("minTouchDistance Control")]
    public float minTouchDis = 0.1f;

    public float width = 0.5f;

    public static GameManager Instance;

    public Button drawBtn;
    public Button clearBtn;
    public Button pauseBtn;
    #endregion

    #region MonoBehavior Callbacks

    IEnumerator CanDraw()
    {
        yield return new WaitForSeconds(0.2f);
        canDraw = true;
    }

    private void Awake()
    {
        Instance = this;
        firstClick = false;

        drawBtn.onClick.AddListener(() =>
        {
            StartCoroutine(CanDraw());
            touchPos.Clear();
        });

        clearBtn.onClick.AddListener(() =>
        {
            for (int i = 0; i < lines.Count; ++i)
                Destroy(lines[i]);
            lines.Clear();
            touchPos.Clear();
        });

        pauseBtn.onClick.AddListener(() =>
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        });
    }

    private void Update()
    {
        if (DemoScene.Instance.currHp <= 0 || touchPos.Count >= DemoScene.Instance.currHp)
        {
            canDraw = false;
            return;
        }
        if (!canDraw) return;
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
        else if (Input.GetMouseButton(0)&&firstClick)
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
                if(touchPos.Count >= DemoScene.Instance.currHp)
                {

                    firstClick = false;

                    canDraw = false;

                    var rb = currObj.AddComponent<Rigidbody2D>();
                    rb.mass = 100;
                    AddPolygonCollider();
                    touchPos.Clear();
                }
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
            var rb = currObj.AddComponent<Rigidbody2D>();
            rb.mass = 100;
            AddPolygonCollider();
            canDraw = false;
        }
    }

    #endregion

    #region Common Functions

    private void CreateLineRenderer()
    {
        var obj = Resources.Load("Line") as GameObject;
        screenPos = Camera.main.WorldToScreenPoint(obj.transform.position);
        var line = Instantiate(obj, obj.transform.position, obj.transform.rotation);
        lines.Add(line);
        currObj = line;
        lineRenderer = line.GetComponent<LineRenderer>();
    }

    private void AddPolygonCollider()
    {
        var collider = currObj.AddComponent<PolygonCollider2D>();
        var colliderPos = GetColliderPos();

        DemoScene.Instance.Decrease(colliderPos.Count / 2);
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

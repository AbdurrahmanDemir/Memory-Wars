using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawController : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject linePrefabs;
    [SerializeField] private GameObject line;


    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private EdgeCollider2D edgeCollider;
    [SerializeField] private List<Vector2> fingerPositionList;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            LineDraw();
        if (Input.GetMouseButton(0))
        {
            Vector2 fingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Vector2.Distance(fingerPos, fingerPositionList[^1]) > .1f)
                LineUpdate(fingerPos);
        }
    }

    void LineDraw()
    {
        line = Instantiate(linePrefabs, Vector2.zero, Quaternion.identity);
        lineRenderer= line.GetComponent<LineRenderer>();
        edgeCollider= line.GetComponent<EdgeCollider2D>();

        fingerPositionList.Clear();
        fingerPositionList.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        fingerPositionList.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        lineRenderer.SetPosition(0, fingerPositionList[0]);
        lineRenderer.SetPosition(1, fingerPositionList[1]);

        edgeCollider.points= fingerPositionList.ToArray();
    }

    void LineUpdate(Vector2 newFingerPos)
    {
        fingerPositionList.Add(newFingerPos);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);
        edgeCollider.points= fingerPositionList.ToArray();
    }
}

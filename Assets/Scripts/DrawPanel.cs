using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Material lineMaterial;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform colliderPrefab;
    [SerializeField] private float colliderCloseness;

    private GameObject lineGameObject;
    private LineRenderer lineGameObjectRenderer;
    private GameObject drawnObject;
    private bool startDrawing = false;
    private int currentIndex;
    private Vector3 mousePosition;
    private Transform lastInstantiatedCollider;
    private MeshRenderer[] lineMesh;

    public void OnPointerDown(PointerEventData eventData)
    {
        CreateNewObject();
        mousePosition = Input.mousePosition;
        startDrawing = true;
        lineGameObjectRenderer = lineGameObject.AddComponent<LineRenderer>();
        lineGameObjectRenderer.material = lineMaterial;
        lineGameObjectRenderer.startWidth = 0.1f;

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        startDrawing = false;
        currentIndex = 0;
        lineGameObject = Instantiate(lineGameObject, new Vector3(0, 0, 10), Quaternion.identity);
        lineMesh = lineGameObject.GetComponentsInChildren<MeshRenderer>();

        foreach(MeshRenderer lineMesh in lineMesh)
        {
            lineMesh.enabled = true;
        }
    }
    private void Update() 
    {
        DrawLineObject();
    }
    private void CreateNewObject()
    {
        lineGameObject = new GameObject("Lined Gameobject");
    }

    private void DrawLineObject()
    {     
        if(startDrawing)
        {
            Vector3 distance = mousePosition - Input.mousePosition;

            if(distance.sqrMagnitude > 5f)
            {
                lineGameObjectRenderer.SetPosition(currentIndex, mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y, Input.mousePosition.z + 5f)));

                if(lastInstantiatedCollider!= null)
                {
                    Vector3 currentLinePosition = lineGameObjectRenderer.GetPosition(currentIndex);

                    lastInstantiatedCollider.LookAt(currentLinePosition);

                    lastInstantiatedCollider.localScale = new Vector3(lastInstantiatedCollider.localScale.x, lastInstantiatedCollider.localScale.y, Vector3.Distance(lastInstantiatedCollider.position, currentLinePosition) * colliderCloseness);
                }

                lastInstantiatedCollider = Instantiate(colliderPrefab, lineGameObjectRenderer.GetPosition(currentIndex),Quaternion.identity, lineGameObject.transform);

                mousePosition = Input.mousePosition;

                currentIndex++;

                lineGameObjectRenderer.positionCount = currentIndex +1;

                lineGameObjectRenderer.SetPosition(currentIndex, mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y, Input.mousePosition.z + 5f)));

                if(currentIndex>100)
                {
                    startDrawing = false;
                }
            }
        }
    }
}

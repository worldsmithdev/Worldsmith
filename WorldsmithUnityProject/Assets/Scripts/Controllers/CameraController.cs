using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Handles mouse movement and zoom. Sets camera location & size through various methods

    public static CameraController Instance { get; protected set; }

    public Camera mainCamera;

    Vector3 lastFramePosition;


    public float maxZoomIn = 2f;
    public float maxZoomOut = 50f;
    public float worldCameraDefaultSize = 25;
    public float locationCameraDefaultSize = 25;
    public float characterCameraDefaultSize = 25;

    public float worldCameraX = -25f;
    public float worldCameraY = 0f;
    public float locationCameraX = 0f;
    public float locationCameraY = 0f;
    public float characterCameraX = 0f;
    public float characterCameraY = 0f;
 

    private void Awake()
    {
        Instance = this;
    }

    public void SetWorldSectionView()
    {
        if (mainCamera != null)
        {
            mainCamera.orthographicSize = worldCameraDefaultSize;
            mainCamera.transform.localPosition = new Vector3(worldCameraX, worldCameraY, -10f);
        }
    }
    public void SetLocationSectionView()
    {
        if (mainCamera != null)
        {
            mainCamera.orthographicSize = locationCameraDefaultSize;
            mainCamera.transform.localPosition = new Vector3(locationCameraX, locationCameraY, -10f);
        }
    }
    public void SetCharacterSectionView()
    {
        if (mainCamera != null)
        {
            mainCamera.orthographicSize = characterCameraDefaultSize;
            mainCamera.transform.localPosition = new Vector3(characterCameraX, characterCameraY, -10f);
        }
    }



    void Update()
    {
        // HANDlE PANNING
        Vector3 currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currFramePosition.z = 0;

        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            Vector3 diff = lastFramePosition - currFramePosition;
            Camera.main.transform.Translate(diff);
        }
        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;

        //HANDLE ZOOM
        Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, maxZoomIn, maxZoomOut); 

    }

    public void CenterCameraOnSelectedLocation()
    {
        // Called from Places - Lookup search, or the locate button in Explore Screen.
        // Setting camera to size 25 because size alters correct positioning and math is hard
        mainCamera.orthographicSize = 25f;
        LocationContainer cont = ContainerController.Instance.GetContainerFromLocation(LocationController.Instance.GetSelectedLocation());
        Vector3 newvec = new Vector3(cont.transform.localPosition.x - 3f, cont.transform.localPosition.y + 15f, 0);
        mainCamera.transform.localPosition = new Vector3(newvec.x, newvec.y, -10);
    }
}

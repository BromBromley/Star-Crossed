using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Minigame1B : MonoBehaviour
{
    // this script manages the minigame "fixing the outer hull"

    // screens for the different parts of the minigame
    private int screenCounter;
    [SerializeField] private GameObject screen01;
    [SerializeField] private GameObject screen02;
    [SerializeField] private GameObject screen03;
    private GameObject[] screenList = new GameObject[3];
    [SerializeField] private GameObject nextScreenButton;

    private List<GameObject> scratchList = new List<GameObject>();
    private List<GameObject> metalPatchList = new List<GameObject>();
    private List<GameObject> boltList = new List<GameObject>();

    // images of the tools following the mouse
    [SerializeField] private GameObject img_welder;
    private bool showingWelder;
    private bool usingWelder;
    [SerializeField] private GameObject img_metalPatch;
    private bool showingMetalPatch;
    [SerializeField] private GameObject img_hammer;
    private bool showingHammer;
    private GameObject currentTool;

    // raycasting when using the tools
    private EventSystem _eventSystem;
    private GraphicRaycaster _raycaster;
    private PointerEventData _pointerEventData;


    private void OnEnable()
    {
        _eventSystem = EventSystem.current;
    }

    void Start()
    {
        _raycaster = GameObject.Find("[Minigames]").GetComponent<GraphicRaycaster>();

        screenList[0] = screen01;
        screenList[1] = screen02;
        screenList[2] = screen03;
        screen01.SetActive(true);
        screen02.SetActive(false);
        screen03.SetActive(false);

        img_welder.SetActive(false);
        img_metalPatch.SetActive(false);
        img_hammer.SetActive(false);

        nextScreenButton.SetActive(false);
        SetUpLists(screenList[screenCounter]);
    }

    void Update()
    {
        // this makes the selected tool follow the mouse
        if (showingWelder || showingMetalPatch || showingHammer)
        {
            currentTool.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y - 30);
        }

        if (Input.GetMouseButton(0))
        {
            if (showingWelder)
            {
                usingWelder = true;
            }
        }
        else
        {
            usingWelder = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (showingMetalPatch)
            {
                _pointerEventData = new PointerEventData(_eventSystem);
                _pointerEventData.position = Input.mousePosition;
                List<RaycastResult> raycastResults = new List<RaycastResult>();
                _raycaster.Raycast(_pointerEventData, raycastResults);
                foreach (RaycastResult result in raycastResults)
                {
                    if (result.gameObject.tag == "MetalPatch")
                    {
                        PlacePatch(result.gameObject);
                    }
                }
            }
            if (showingHammer)
            {
                _pointerEventData = new PointerEventData(_eventSystem);
                _pointerEventData.position = Input.mousePosition;
                List<RaycastResult> raycastResults = new List<RaycastResult>();
                _raycaster.Raycast(_pointerEventData, raycastResults);
                foreach (RaycastResult result in raycastResults)
                {
                    if (result.gameObject.tag == "Bolt")
                    {
                        PlacePatch(result.gameObject);
                    }
                }
            }
        }

        if (usingWelder)
        {
            _pointerEventData = new PointerEventData(_eventSystem);
            _pointerEventData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            _raycaster.Raycast(_pointerEventData, raycastResults);

            foreach (RaycastResult result in raycastResults)
            {
                if (result.gameObject.tag == "Scratch")
                {
                    result.gameObject.SetActive(false);
                    scratchList.Remove(result.gameObject);
                    CheckIfDone();
                    //TestAudioManager.onUsingTool?.Invoke(0);
                }
            }
        }
    }


    public void nextScreen()
    {
        nextScreenButton.SetActive(false);
        screenCounter++;
        SetUpLists(screenList[screenCounter]);

        if (screenCounter == 1)
        {
            screen01.SetActive(false);
            screen02.SetActive(true);
        }
        if (screenCounter == 2)
        {
            screen02.SetActive(false);
            screen03.SetActive(true);
        }
    }

    private void SetUpLists(GameObject screen)
    {
        for (int i = 0; i < screen.transform.childCount; i++)
        {
            if (screen.transform.GetChild(i).tag != "Untagged")
            {
                if (screen.transform.GetChild(i).tag == "MetalPatch")
                {
                    metalPatchList.Add(screen.transform.GetChild(i).gameObject);
                }
                if (screen.transform.GetChild(i).tag == "Bolt")
                {
                    boltList.Add(screen.transform.GetChild(i).gameObject);
                }
            }
            else
            {
                for (int j = 0; j < screen.transform.GetChild(i).transform.childCount; j++)
                {
                    if (screen.transform.GetChild(i).transform.GetChild(j).tag == "Scratch")
                    {
                        scratchList.Add(screen.transform.GetChild(i).transform.GetChild(j).gameObject);
                        screen.transform.GetChild(i).transform.GetChild(j).GetComponent<Image>().alphaHitTestMinimumThreshold = 1f;
                    }
                }
            }
        }
    }


    private void PlacePatch(GameObject patch)
    {
        patch.GetComponent<Image>().color = Color.white;
        if (patch.tag == "MetalPatch")
        {
            metalPatchList.Remove(patch);
            ToggleTools("metalPatch");
        }
        if (patch.tag == "Bolt")
        {
            //TestAudioManager.onUsingTool?.Invoke(1);
            boltList.Remove(patch);
            CheckIfDone();
        }
    }


    // this toggles the different tool bools and images
    public void ToggleTools(string toolName)
    {
        // this toggles the bools
        if (toolName == "welder")
        {
            showingWelder = !showingWelder;
            showingMetalPatch = false;
            showingHammer = false;
        }
        if (toolName == "metalPatch")
        {
            showingMetalPatch = !showingMetalPatch;
            showingWelder = false;
            showingHammer = false;
        }
        if (toolName == "hammer")
        {
            showingHammer = !showingHammer;
            showingWelder = false;
            showingMetalPatch = false;
        }

        // this turns on the right images and assignes them the current tool
        if (showingWelder)
        {
            img_welder.SetActive(true);
            currentTool = img_welder;
        }
        else
        {
            img_welder.SetActive(false);
        }
        if (showingMetalPatch)
        {
            img_metalPatch.SetActive(true);
            currentTool = img_metalPatch;
        }
        else
        {
            img_metalPatch.SetActive(false);
        }
        if (showingHammer)
        {
            img_hammer.SetActive(true);
            currentTool = img_hammer;
        }
        else
        {
            img_hammer.SetActive(false);
        }
    }


    private void CheckIfDone()
    {
        if (screenCounter == 0)
        {
            if (scratchList.Count == 0)
            {
                nextScreenButton.SetActive(true);
            }
        }
        if (screenCounter == 1)
        {
            if (boltList.Count == 0)
            {
                nextScreenButton.SetActive(true);
            }
        }
        if (screenCounter == 2)
        {
            if (scratchList.Count == 0 && boltList.Count == 0)
            {
                Debug.Log("You did it!");
                TestSceneManager.onFinishedTask?.Invoke(1);
            }
        }
    }

}

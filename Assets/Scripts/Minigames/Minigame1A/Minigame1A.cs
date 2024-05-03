using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Minigame1A : MonoBehaviour
{
    // this script manages the minigame "fixing the spacesuit"

    [SerializeField] private GameObject top_01;
    [SerializeField] private GameObject bottom_01;

    [SerializeField] private GameObject img_cleaner;
    private bool showingCleaner;
    private bool usingCleaner;
    [SerializeField] private GameObject img_scanner;
    private bool showingScanner;
    private bool usingScanner;
    [SerializeField] private GameObject img_patch;
    private bool showingPatch;

    private GameObject currentTool;

    private List<GameObject> dirtList;
    private List<GameObject> damageList;
    private List<GameObject> patchList;

    private EventSystem _eventSystem;
    private GraphicRaycaster _raycaster;
    private PointerEventData _pointerEventData;
    
    private Color pink = new Color32(239, 60, 228, 255);
    private Color blue = new Color32(91, 104, 199, 255);


    private void OnEnable()
    {
        _eventSystem = EventSystem.current;
    }

    void Start()
    {
        _raycaster = GameObject.Find("[Minigames]").GetComponent<GraphicRaycaster>();

        top_01.SetActive(true);
        bottom_01.SetActive(true);
        dirtList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Dirt"));
        damageList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Damage"));
        patchList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Patch"));
        bottom_01.SetActive(false);
    }

    void Update()
    {
        // this makes the selected tool follow the mouse
        if (showingCleaner || showingScanner || showingPatch)
        {
            currentTool.transform.position = Input.mousePosition;
        }

        // this checks which tool is being used 
        if (Input.GetMouseButton(0))
        {
            if (showingCleaner)
            {
                usingCleaner = true;
            }
            if (showingScanner)
            {
                usingScanner = true;
            }
            if (showingPatch)
            {
                _pointerEventData = new PointerEventData(_eventSystem);
                _pointerEventData.position = Input.mousePosition;
                List<RaycastResult> raycastResults = new List<RaycastResult>();
                _raycaster.Raycast(_pointerEventData, raycastResults);
                foreach (RaycastResult result in raycastResults)
                {
                    if (result.gameObject.tag == "Patch")
                    {
                        PlacePatch(result.gameObject);
                    }
                }
            }
        }
        else 
        {
            usingCleaner = false;
            usingScanner = false;
        }


        // this checks if the player hits dirt when cleaning with a raycast
        if (usingCleaner)
        {
            _pointerEventData = new PointerEventData(_eventSystem);
            _pointerEventData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            _raycaster.Raycast(_pointerEventData, raycastResults);

            foreach (RaycastResult result in raycastResults)
            {
                if (result.gameObject.tag == "Dirt")
                {
                    if (result.gameObject.GetComponent<Image>().color.a > 0.2f)
                    {
                        CleanDirt(result.gameObject);
                    }
                    else
                    {
                        result.gameObject.SetActive(false);
                        dirtList.Remove(result.gameObject);
                        CheckIfDone();
                    }
                }
            }
        }
        
        // this checks if the player is scanning damages with a raycast
        if (usingScanner)
        {
            _pointerEventData = new PointerEventData(_eventSystem);
            _pointerEventData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            _raycaster.Raycast(_pointerEventData, raycastResults);

            foreach (RaycastResult result in raycastResults)
            {
                if (result.gameObject.tag == "Damage")
                {
                    ScanDamage(result.gameObject);
                }
            }
        }
    }


    // this fades the color of the dirt while it gets cleaned
    private void CleanDirt(GameObject dirt)
    {
        Color fadeColor = dirt.GetComponent<Image>().color;
        fadeColor.a = Mathf.Lerp(fadeColor.a, 0, (Time.deltaTime * 0.5f));
        dirt.GetComponent<Image>().color = fadeColor;
    }

    // this fades the color of the damages to either pink or blue
    private void ScanDamage(GameObject damage)
    {
        if (damage.GetComponent<DamageScript>().needsFixing)
        {
            damage.GetComponent<Image>().color = Color.Lerp(damage.GetComponent<Image>().color, pink, (Time.deltaTime * 0.9f));
        }
        else
        {
            damage.GetComponent<Image>().color = Color.Lerp(damage.GetComponent<Image>().color, blue, (Time.deltaTime * 0.9f));
        }

        // this checks if the color is close enough to make it easier to finish scanning
        if (Vector4.Distance(damage.GetComponent<Image>().color, pink) < 0.1f || Vector4.Distance(damage.GetComponent<Image>().color, blue) < 0.1f)
        {
            damageList.Remove(damage);
            damage.GetComponent<Image>().raycastTarget = false;
            CheckIfDone();
        }
    }

    // this makes placed patches visible
    private void PlacePatch(GameObject patch)
    {
        patch.GetComponent<Image>().color = Color.white;
        patchList.Remove(patch);
        ToggleTool("patch");
        CheckIfDone();
    }


    // this toggles the different tool bools and images according to the buttons
    public void ToggleTool(string toolName)
    {
        // this toggles the bools
        if (toolName == "cleaner")
        {
            showingCleaner = !showingCleaner;
            showingScanner = false;
            showingPatch = false;
        } 
        else if (toolName == "scanner")
        {
            showingScanner = !showingScanner;
            showingCleaner = false;
            showingPatch = false; 
        }
        else if (toolName == "patch")
        {
            showingPatch = !showingPatch;
            showingCleaner = false;
            showingScanner = false;
        }

        // this turns on the right images and assignes them as current tool
        if (showingCleaner)
        {
            img_cleaner.SetActive(true);
            img_scanner.SetActive(false);
            img_patch.SetActive(false);
            currentTool = img_cleaner;
        }
        else
        {
            img_cleaner.SetActive(false);
        }
        if (showingScanner)
        {
            img_cleaner.SetActive(false);
            img_scanner.SetActive(true);
            img_patch.SetActive(false);
            currentTool = img_scanner;
        }
        else
        {
            img_scanner.SetActive(false);
        }
        if (showingPatch)
        {
            img_cleaner.SetActive(false);
            img_scanner.SetActive(false);
            img_patch.SetActive(true);
            currentTool = img_patch;

            // this checks if the places for the patches have been cleaned
            foreach (GameObject patch in patchList)
            {
                patch.SetActive(true);
                patch.GetComponent<PatchScript>().CheckIfCleaned();
            }
        }
        else 
        {
            img_patch.SetActive(false);
            foreach (GameObject patch in patchList)
            {
                patch.SetActive(false);
            }
        }
    }


    // this checks if all dirt is cleaned and damages are scanned and fixed
    // the items of the list are removed after each step
    private void CheckIfDone()
    {
        if (dirtList.Count == 0 && damageList.Count == 0 && patchList.Count == 0)
        {
            Debug.Log("You did it!");
        }
    }

}

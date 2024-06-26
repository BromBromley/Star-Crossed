using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Minigame1A : MonoBehaviour
{
    // this script manages the minigame "fixing the spacesuit"

    private int levelCounter;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private GameObject level01;
    [SerializeField] private GameObject level02;
    private List<GameObject> dirtList = new List<GameObject>();
    private List<GameObject> damageList = new List<GameObject>();
    private List<GameObject> patchList = new List<GameObject>();

    // images of the tools following the mouse
    [SerializeField] private GameObject img_cleaner;
    private bool showingCleaner;
    private bool usingCleaner;
    [SerializeField] private GameObject img_scanner;
    private bool showingScanner;
    private bool usingScanner;
    [SerializeField] private GameObject img_patch;
    private bool showingPatch;

    private GameObject currentTool;

    // raycasting when using the tools
    private EventSystem _eventSystem;
    private GraphicRaycaster _raycaster;
    private PointerEventData _pointerEventData;

    // colors for scanning and cleaning
    private Color pink = new Color32(239, 60, 228, 255);
    private Color blue = new Color32(104, 113, 140, 255);
    private Color transparentColor = new Color32(255, 255, 255, 0);

    [SerializeField] private GameObject[] bitemarks = new GameObject[3];
    [SerializeField] private GameObject[] bitePatches = new GameObject[3];
    private int bitemarkIndex;
    private bool finishedSecondRound;


    private void OnEnable()
    {
        _eventSystem = EventSystem.current;
    }

    void Start()
    {
        _raycaster = GameObject.Find("[Minigames]").GetComponent<GraphicRaycaster>();
        nextLevelButton.SetActive(false);
        SetUpRightLevel();
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

    private void SetUpRightLevel()
    {
        if (levelCounter == 0)
        {
            SetUpRoundOne();
        }
        else
        {
            SetUpRoundTwo();
        }
    }

    // this turns on the elements for level 1 and assigns them to the lists
    // called in Start()
    private void SetUpRoundOne()
    {
        level02.transform.GetChild(0).gameObject.SetActive(false);
        level02.transform.GetChild(1).gameObject.SetActive(false);
        level02.SetActive(false);
        level01.SetActive(true);
        level01.transform.GetChild(0).gameObject.SetActive(true);
        level01.transform.GetChild(1).gameObject.SetActive(true);
        for (int j = 0; j < level01.transform.childCount; j++)
        {
            for (int i = 0; i < level01.transform.GetChild(j).transform.childCount; i++)
            {
                if (level01.transform.GetChild(j).transform.GetChild(i).tag == "Dirt")
                {
                    dirtList.Add(level01.transform.GetChild(j).transform.GetChild(i).gameObject);
                }
                if (level01.transform.GetChild(j).transform.GetChild(i).tag == "Damage")
                {
                    damageList.Add(level01.transform.GetChild(j).transform.GetChild(i).gameObject);
                }
                if (level01.transform.GetChild(j).transform.GetChild(i).tag == "Patch")
                {
                    patchList.Add(level01.transform.GetChild(j).transform.GetChild(i).gameObject);
                }
            }
        }
        level01.transform.GetChild(1).gameObject.SetActive(false);
    }
    // this turns on the elements for level 2 and assigns them to the lists
    public void SetUpRoundTwo()
    {
        levelCounter++;
        level01.transform.GetChild(0).gameObject.SetActive(false);
        level01.transform.GetChild(1).gameObject.SetActive(false);
        level01.SetActive(false);
        level02.SetActive(true);
        level02.transform.GetChild(0).gameObject.SetActive(true);
        level02.transform.GetChild(1).gameObject.SetActive(true);
        for (int j = 0; j < level02.transform.childCount; j++)
        {
            for (int i = 0; i < level02.transform.GetChild(j).transform.childCount; i++)
            {
                if (level02.transform.GetChild(j).transform.GetChild(i).tag == "Dirt")
                {
                    dirtList.Add(level02.transform.GetChild(j).transform.GetChild(i).gameObject);
                }
                if (level02.transform.GetChild(j).transform.GetChild(i).tag == "Damage")
                {
                    damageList.Add(level02.transform.GetChild(j).transform.GetChild(i).gameObject);
                }
                if (level02.transform.GetChild(j).transform.GetChild(i).tag == "Patch")
                {
                    patchList.Add(level02.transform.GetChild(j).transform.GetChild(i).gameObject);
                }
            }
        }
        damageList.Remove(bitemarks[1].gameObject);
        damageList.Remove(bitemarks[2].gameObject);
        bitemarks[1].SetActive(false);
        bitemarks[2].SetActive(false);
        patchList.Remove(bitePatches[1].gameObject);
        patchList.Remove(bitePatches[2].gameObject);
        bitePatches[1].SetActive(false);
        bitePatches[2].SetActive(false);
        level02.transform.GetChild(1).gameObject.SetActive(false);
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

        if (patch.GetComponent<PatchScript>().patchFallsOff)
        {
            StartCoroutine(RemovePatch(patch));
        }
    }


    // this removes the patches from the bitemarks during level 2
    private IEnumerator RemovePatch(GameObject patch)
    {
        yield return new WaitForSeconds(3);

        bitemarks[bitemarkIndex].gameObject.SetActive(false);
        bitemarkIndex++;
        bitemarks[bitemarkIndex].gameObject.SetActive(true);
        damageList.Add(bitemarks[bitemarkIndex].gameObject);
        bitePatches[bitemarkIndex].gameObject.SetActive(true);
        patchList.Add(bitePatches[bitemarkIndex].gameObject);

        float fadeTime = 0f;
        float speed = 5f;

        TestAudioManager.onGlitch?.Invoke(false);
        while (patch.GetComponent<Image>().color.a > 0)
        {
            fadeTime += speed * Time.deltaTime;
            patch.GetComponent<Image>().color = Color.Lerp(Color.white, transparentColor, fadeTime);
            yield return null;
        }
        patch.SetActive(false);

        if (bitemarkIndex == 2)
        {
            finishedSecondRound = true;
        }
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
        if (dirtList.Count == 0 && patchList.Count == 0)
        {
            if (levelCounter == 0)
            {
                Debug.Log("You did it!");
                nextLevelButton.SetActive(true);
            }
            if (finishedSecondRound)
            {
                Debug.Log("You did it!");
                TestSceneManager.onFinishedTask?.Invoke(0);
            }
        }
    }

}

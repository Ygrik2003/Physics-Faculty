using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapScrollInfo : MonoBehaviour
{
    [Range(1, 50)]
    [Header("Controllers")]
    public int panCount;
    [Range(0, 500)]
    public int panOffset;
    [Range(0f, 20f)]
    public float snapSpeed;
    [Range(0f, 10f)]
    public float scaleOffset;
    [Range(1f, 20f)]
    public float scaleSpeed;
    [Header("Other Objects")]
    public GameObject panPrefab;
    public ScrollRect scrollRect;

    private GameObject[] instPans;
    private Vector2[] pansPos;
    private Vector2[] pansScale;

    //[Header ("Other Objects")]
   // private GameObject buttonPrefab;

    //private GameObject[] instButtons;
    //private Vector2[] buttonPos;

    private RectTransform contentRect;
    private Vector2 contentVector;
    
    private int selectedPanID;
    private bool isScrolling;

    private void Start()
    {
        contentRect = GetComponent<RectTransform>();
        instPans = new GameObject[panCount];
        pansPos = new Vector2[panCount];
        pansScale = new Vector2[panCount];
        //buttonPos = new Vector2[panCount];
        //instButtons = new GameObject[panCount];
        for ( int i = 0; i < panCount; i++)
        {
            instPans[i] = Instantiate(panPrefab, transform, false);
            if (i == 0) continue;
            instPans[i].transform.localPosition = new Vector2(instPans[i - 1].transform.localPosition.x + 
                panPrefab.GetComponent<RectTransform>().sizeDelta.x + panOffset, instPans[i].transform.localPosition.y);
            pansPos[i] = -instPans[i].transform.localPosition;
            /* instButtons[i] = Instantiate(buttonPrefab, transform, false);
             if (i == 0) continue;
             instButtons[i].transform.localPosition = new Vector2(instButtons[i - 1].transform.localPosition.x +
                 panPrefab.GetComponent<RectTransform>().sizeDelta.x + panOffset, instButtons[i].transform.localPosition.y);
             buttonPos[i] = -instButtons[i].transform.localPosition;*/
            //buttonPos = new Vector2[panCount];
            //instButtons = new GameObject[panCount];
        }
    }

    private void FixedUpdate()
    {
        if (contentRect.anchoredPosition.x >= pansPos[0].x && !isScrolling || contentRect.anchoredPosition.x <= pansPos[pansPos.Length-1].x && !isScrolling)
        {
            scrollRect.inertia = false;
        }
        float nearestPos = float.MaxValue;
        for (int i = 0; i < panCount; i++)
        {
            float distance = Mathf.Abs(contentRect.anchoredPosition.x - pansPos[i].x);
            if (distance < nearestPos)
            {
                nearestPos = distance;
                selectedPanID = i;
            }
            float scale = Mathf.Clamp(1 / (distance / panOffset) * scaleOffset, 0.5f, 1f);
            if (instPans[i].transform.localScale.x == scale || instPans[i].transform.localScale.y == scale)
            {
                pansScale[i].x = Mathf.SmoothStep(instPans[i].transform.localScale.x, scale + 0.3f, scaleSpeed * Time.fixedDeltaTime);
                pansScale[i].y = Mathf.SmoothStep(instPans[i].transform.localScale.y, scale + 0.3f, scaleSpeed * Time.fixedDeltaTime);
                instPans[i].transform.localScale = pansScale[i];
            }
        }
        float scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
        if (scrollVelocity < 400 && !isScrolling) scrollRect.inertia = false;
        if (isScrolling || scrollVelocity > 400) return;
        if (isScrolling) return;
        contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, pansPos[selectedPanID].x, 
            snapSpeed * Time.fixedDeltaTime);
        contentRect.anchoredPosition = contentVector;
    }
    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
        if (scroll) scrollRect.inertia = true;
    }
}

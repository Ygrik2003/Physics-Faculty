using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static MenuButton;

public class MainWindow : MonoBehaviour
{

    //[SerializeField] [Range(0f,1f)] private float centerOfEllipseX = 0.5f, centerOfEllipseY = 0.55f;         // counting from top
    //[SerializeField] [Range(0f,1f)] private float verticalRadius = 0.15f, horizontalRadius = 0.3f;
    [SerializeField] private GameObject ButtonsObject;
    [SerializeField] [Range(0f,2f)] private float inputCd = 0.5f;
    [SerializeField] [Range(0f,2f)] private float animationTime = 0.4f;
    [SerializeField] [Range(0f,1f)] private float minScale = 0.6f, maxScale = 1.2f;



    private bool rotatingRight = false, rotatingLeft = false;
    private float lastInput = 0;
    //private int actualCenterX, actualCenterY, actualRadiusX, actualRadiusY;

    private float angularVelocity;
    private float animationStart;

    private float rotationCurr = 0f, angleBetweenBtns = 90f;
    private int numOfBtns;

    private BtnType selectedButton = 0;
    private List<GameObject> Buttons = new();
    RectTransform scaler;

    public void Start()
    {
        if (animationTime > inputCd)
            animationTime = inputCd;
        /*
        GetComponentsInChildren( btnTransforms );                               // Get Transform for all btns
        btnTransforms.Remove( gameObject.GetComponent<RectTransform>() );           // Delete own Transform
        numOfBtns = btnTransforms.Count;
        angleBetweenBtns = 360.0f / numOfBtns;



        */
        Transform[] tempTransforms = ButtonsObject.GetComponentsInChildren<RectTransform>();
        foreach (Transform t in tempTransforms)
        {
            if (t.gameObject != ButtonsObject)
                Buttons.Add(t.gameObject);
        }

        numOfBtns = Buttons.Count;

        angularVelocity = angleBetweenBtns / animationTime;
        scaler = ButtonsObject.GetComponent<RectTransform>();

        //RecomputeActualValues();
        RearrangeButtons();

    }

    //private void RecomputeActualValues()
    //{
    //    actualCenterX = (int) (scaler.rect.width * centerOfEllipseX);
    //    actualCenterY = (int) (scaler.rect.height * ( 1 - centerOfEllipseY ) );
    //
    //    actualRadiusX = (int) ( scaler.rect.width  * horizontalRadius );
    //    actualRadiusY = (int) ( scaler.rect.height * verticalRadius );
    //}

    private Vector3 CalcScale()
    {
        float xyScale = minScale + Abs( (rotationCurr + 360) % 360 - 180 ) / 180.0f * (maxScale - minScale);
        return new Vector3( xyScale, xyScale , 1 );
    }
    private void RearrangeButtons()
    {
        int currButtonToDraw = (int)selectedButton;
        int tempX, tempY;
        while (true)
        {
            tempX = (int) (scaler.rect.center.x + (scaler.rect.width / 2) * Sin( rotationCurr * Acos(-1) /180 ));
            tempY = (int) (scaler.rect.center.y - (scaler.rect.height / 2) * Cos( rotationCurr * Acos(-1) /180 ));
            Buttons[currButtonToDraw].GetComponent<RectTransform>().localPosition = new Vector3(tempX, tempY, 0);
            Buttons[currButtonToDraw].GetComponent<RectTransform>().localScale = CalcScale();
            rotationCurr += angleBetweenBtns;
            currButtonToDraw = (currButtonToDraw + 1) % numOfBtns;
            if (currButtonToDraw == (int)selectedButton)
                break;
        }
        rotationCurr -= 360;
    }
    public void SwitchToStart()
    {
        if (selectedButton != BtnType.Start) return;
            SceneManager.LoadScene("GameScene");
    }
    public void SwitchToInfo(GameObject window)
    {
        if (selectedButton != BtnType.Info) return;
            window.SetActive(true);
    }
    public void SwitchToSettings(GameObject window)
    {
        if (selectedButton != BtnType.Settings) return;
            window.SetActive(true);
    }
    public void SwitchToExit(GameObject window)
    {
        if (selectedButton != BtnType.Exit) return;
            window.SetActive(true);
    }
    public void ButtonPressed(MenuButton button)
    {
        if (selectedButton != button.type)
        {
            if (Abs(button.type - selectedButton) == 1 || (4 - Abs(button.type - selectedButton)) == 1)
            {
                if (selectedButton - button.type == numOfBtns - 1)
                    rotate(-1);
                else if (selectedButton - button.type == 1 - numOfBtns)
                    rotate(1);
                else
                    rotate(selectedButton - button.type);
            }
            
            return;
        }
        button.Pressed();
        gameObject.SetActive(false);
    }
    private void rotate(float dir)
    {
        if ((Time.time - lastInput) > inputCd)
        {
            lastInput = Time.time;
            if (dir < 0.0f)
            {
                rotatingRight = true;
            }
            else
            {
                rotatingLeft = true;
            }
            animationStart = Time.time;
        }
    }

    public void Update()
    {
        if (Abs(Input.GetAxis("Horizontal")) > 0.0f)
        {
            rotate(Input.GetAxis("Horizontal"));
        }
        if (rotatingRight){
            if ( Abs(rotationCurr - angularVelocity * Time.deltaTime) < angleBetweenBtns )
                rotationCurr -= angularVelocity * Time.deltaTime;
        } else if (rotatingLeft){
            if ( (rotationCurr + angularVelocity * Time.deltaTime) < angleBetweenBtns )
            rotationCurr += angularVelocity * Time.deltaTime;
        }
        if (rotatingLeft || rotatingRight)
            RearrangeButtons();
        
        if ( (rotatingLeft || rotatingRight) && (Time.time - animationStart - Time.deltaTime) > animationTime ){
            Buttons[(int)selectedButton].GetComponent<MenuButton>().setWiggled(false);
            if (rotatingRight)
                selectedButton = (BtnType)(((int)selectedButton + 1) % numOfBtns);
            else
                selectedButton = (BtnType)(((int)selectedButton + numOfBtns - 1) % numOfBtns);
            Buttons[(int)selectedButton].GetComponent<MenuButton>().setWiggled(true);
            rotationCurr = 0;
            RearrangeButtons();
            rotatingLeft = rotatingRight = false;
        }
    }
}

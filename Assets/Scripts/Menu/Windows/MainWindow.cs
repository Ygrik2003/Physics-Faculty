using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static MenuButton;

public class MainWindow : MonoBehaviour
{

    [SerializeField] [Range(0f,1f)] private float centerOfEllipseX = 0.5f, centerOfEllipseY = 0.55f;         // counting from top
    [SerializeField] [Range(0f,1f)] private float verticalRadius = 0.15f, horizontalRadius = 0.3f;
    [SerializeField] [Range(0f,2f)] private float rotateCd = 0.5f;
    [SerializeField] [Range(0f,2f)] private float minScale = 0.6f, maxScale = 1.2f;
    [SerializeField] [Range(0f,2f)] private float animationTime = 0.4f;

    private bool ignoreInput = false;

    private bool rotatingRight = false, rotatingLeft = false;
    private float lastInput = 0;
    private int actualCenterX, actualCenterY, actualRadiusX, actualRadiusY;

    private float angularVelocity;
    private float animationStart;

    private float rotationCurr = 0f, angleBetweenBtns;
    private int numOfBtns;

    private int selectedButton = 0;
    private List<RectTransform> btnTransforms = new List<RectTransform>();
    private List<MenuButton> btns = new List<MenuButton>();
    private Transform canvasTransform;
    RectTransform scaler = new RectTransform();

    public void Start()
    {
        if (animationTime > rotateCd)
            animationTime = rotateCd;
        GetComponentsInChildren( btnTransforms );                               // Get Transform for all btns
        btnTransforms.Remove( gameObject.GetComponent<RectTransform>() );           // Delete own Transform
        numOfBtns = btnTransforms.Count;
        angleBetweenBtns = 360.0f / numOfBtns;

        angularVelocity = angleBetweenBtns / animationTime;

        GetComponentsInChildren( btns );

        scaler = GetComponentInParent<RectTransform>();
        RecomputeActualValues();
        RearrangeButtons();

        btns[0].setWiggled(true);

    }

    private void RecomputeActualValues()
    {
        actualCenterX = (int) (scaler.rect.width * centerOfEllipseX);
        actualCenterY = (int) (scaler.rect.height * ( 1 - centerOfEllipseY ) );

        actualRadiusX = (int) ( scaler.rect.width  * horizontalRadius );
        actualRadiusY = (int) ( scaler.rect.height * verticalRadius );
    }

    private Vector3 CalcScale()
    {
        float xyScale = minScale + (Abs( (rotationCurr + 360) % 360 - 180 ) / 180.0f) * (maxScale - minScale);
        return new Vector3( xyScale, xyScale , 1 );
    }
    private void RearrangeButtons()
    {
        int currButtonToDraw = selectedButton;
        int tempX, tempY;
        while (true)
        {
            tempX = (int) (actualCenterX + actualRadiusX * Sin( rotationCurr * Acos(-1) /180 ) - scaler.rect.width/2);
            tempY = (int) (actualCenterY - actualRadiusY * Cos( rotationCurr * Acos(-1) /180 ) - scaler.rect.height/2);
            btnTransforms[currButtonToDraw].localPosition = new Vector3(tempX, tempY, 0);
            btnTransforms[currButtonToDraw].localScale = CalcScale();
            rotationCurr += angleBetweenBtns;
            currButtonToDraw = (currButtonToDraw + 1) % numOfBtns;
            if (currButtonToDraw == selectedButton)
                break;
        }
        rotationCurr -= 360;
    }
    
    public void ButtonPressed(MenuButton button)
    {
        if (button != btns[selectedButton]){
            int leftButn = (selectedButton + numOfBtns - 1) % numOfBtns, rightButn = (selectedButton + 1) % numOfBtns;
            if ( btns[leftButn] == button ){
                rotate(-1);
            } else if ( btns[rightButn] == button ){
                rotate(1);
            }
            return;
        }

        btns[selectedButton].Pressed();

        if (selectedButton == 0){
            SceneManager.LoadScene("GameScene");
            return;
        }
        if (button.OverlapWindow)
            gameObject.SetActive(false);
        else
            ignoreInput = true;
        button.scriptToStart.gameObject.SetActive(true);
    }

    public void setIgnoreInput(bool newState){
        ignoreInput = newState;
    }

    private void rotate(float dir)
    {
        if ((Time.time - lastInput) > rotateCd)
        {
            lastInput = Time.time;
            btns[selectedButton].Highlighted();
            if (dir > 0.0f)
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
        // Input Processing
        if (Abs(Input.GetAxis("Horizontal")) > 0.0f && !ignoreInput)
        {
            rotate(Input.GetAxis("Horizontal"));
        }

        if ( (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return)) && !ignoreInput ) {
            ButtonPressed(btns[selectedButton]);
        }


        // Rotation Animation
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
            btns[selectedButton].setWiggled(false);
            if (rotatingRight)
                selectedButton = (selectedButton + 1) % numOfBtns;
            else
                selectedButton = (selectedButton + numOfBtns - 1) % numOfBtns;
            btns[selectedButton].setWiggled(true);
            rotationCurr = 0;
            RearrangeButtons();
            rotatingLeft = rotatingRight = false;
        }
    }

}

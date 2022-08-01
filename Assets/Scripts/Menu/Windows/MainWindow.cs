using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainWindow : MonoBehaviour
{
    [SerializeField] [Range(0f,1f)] private float centerOfEllipseX = 0.5f, centerOfEllipseY = 0.55f;         // counting from top
    [SerializeField] [Range(0f,1f)] private float verticalRadius = 0.15f, horizontalRadius = 0.3f;
    [SerializeField] [Range(0f,2f)] private float inputCd = 0.5f;
    [SerializeField] [Range(0f,1f)] private float minScale = 0.6f, maxScale = 1.2f;
    [SerializeField] [Range(0f,2f)] private float animationTime = 0.4f;
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
        if (animationTime > inputCd)
            animationTime = inputCd;
        GetComponentsInChildren( btnTransforms );                               // Get Transform for all btns
        btnTransforms.Remove( gameObject.GetComponent<RectTransform>() );           // Delete own Transform
        numOfBtns = btnTransforms.Count;
        angleBetweenBtns = 360.0f / numOfBtns;

        angularVelocity = angleBetweenBtns / animationTime;

        GetComponentsInChildren( btns );

        scaler = GetComponentInParent<RectTransform>();
        RecomputeActualValues();
        RearrangeButtons();

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
        float xyScale = minScale + (Abs( rotationCurr - 180 )/180.0f) * (maxScale - minScale);
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
            currButtonToDraw = (currButtonToDraw + 1)%numOfBtns;
            if (currButtonToDraw == selectedButton)
                break;
        }
        rotationCurr -= 360;
    }

    public void setVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }
    public void StartPressed()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void InfoPressed()
    {
        setVisible(false);
    }
    public void SettingsPressed()
    {
        setVisible(false);
    }

    public void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            if ( ( Abs(Input.GetAxis("Horizontal")) > 0.0f ) && (Time.time - lastInput) > inputCd ){
                lastInput = Time.time;
                if (Input.GetAxis("Horizontal") > 0.0f){
                    rotatingRight = true;
                } else {
                    rotatingLeft = true;
                }
                animationStart = Time.time;
                //RearrangeButtons();
            }

            if (rotatingRight){
                rotationCurr -= angularVelocity * Time.deltaTime;
            } else if (rotatingLeft){
                rotationCurr += angularVelocity * Time.deltaTime;
            }

            if (rotatingLeft || rotatingRight)
                RearrangeButtons();
            
            if ( (rotatingLeft || rotatingRight) && (Time.time - animationStart) > animationTime ){
                btns[selectedButton].setWiggled(false);
                if (rotatingRight)
                    selectedButton = (selectedButton + 1)%numOfBtns;
                else
                    selectedButton = (selectedButton + 3)%numOfBtns;
                btns[selectedButton].setWiggled(true);
                rotationCurr = 0;
                RearrangeButtons();
                rotatingLeft = rotatingRight = false;
            }
        }
    }

}

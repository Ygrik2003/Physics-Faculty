using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static MenuButton;

public class MainWindow : MonoBehaviour
{

    [SerializeField] private GameObject ButtonsObject;
    [SerializeField] [Range(0f,2f)] private float inputCd = 0.5f;
    [SerializeField] [Range(0f,2f)] private float animationTime = 0.4f;
    [SerializeField] [Range(0f,2f)] private float minScale = 0.6f, maxScale = 1.2f;
    [SerializeField] [Range(0f,1f)] private float maxShadowCoef = 0.9f;

    private bool rotatingRight = false, rotatingLeft = false;
    private float lastInput = 0;
    private bool ignoreInput = false;

    private float angularVelocity;
    private float animationStart;

    private float rotationCurr = 0f, angleBetweenBtns = 90f;
    private int numOfBtns;

    private int selectedButton = 0;
    private List<Transform> BtnsTransform = new();
    private List<MenuButton> BtnsScript = new();
    private List<Renderer> BtnsRenderer = new();
    RectTransform scaler;

    Renderer rend;

    public void Start()
    {
        if (animationTime > inputCd)
            animationTime = inputCd;

        scaler = ButtonsObject.GetComponent<RectTransform>();

        numOfBtns = scaler.childCount;

        for (int i = 0; i < numOfBtns; i++){
            BtnsTransform.Add( scaler.GetChild(i) );
            BtnsScript.Add( scaler.GetChild(i).gameObject.GetComponent<MenuButton>() );
            BtnsRenderer.Add(scaler.GetChild(i).gameObject.GetComponentInChildren<Renderer>() );
        }
        BtnsScript[0].setWiggled(true);

        angleBetweenBtns = 360f / numOfBtns;
        angularVelocity = angleBetweenBtns / animationTime;

        RearrangeButtons();

    }

    private Vector3 CalcScale()
    {
        float xyScale = minScale + Abs( (rotationCurr + 360) % 360 - 180 ) / 180.0f * (maxScale - minScale);
        return new Vector3( xyScale, xyScale , 1 );
    }

    private void RearrangeButtons()
    {
        int currButtonToDraw = selectedButton;
        int tempX, tempY;
        while (true)
        {
            tempX = (int) ((scaler.rect.width / 2) * Sin( rotationCurr * Acos(-1) /180 ));
            tempY = (int) (- (scaler.rect.height / 2) * Cos( rotationCurr * Acos(-1) /180 ));
            BtnsTransform[currButtonToDraw].localPosition = new Vector3(tempX, tempY, 0);
            BtnsTransform[currButtonToDraw].localScale = CalcScale();
            if (BtnsRenderer[currButtonToDraw] != null){
                BtnsRenderer[currButtonToDraw].material.SetFloat("_ShadowCoeff", maxShadowCoef * (1 - Abs( (rotationCurr + 360) % 360  - 180) / 180.0f) );
            }
            rotationCurr += angleBetweenBtns;
            currButtonToDraw = (currButtonToDraw + 1) % numOfBtns;
            if (currButtonToDraw == selectedButton)
                break;
        }
        rotationCurr -= 360f;
    }

    public void ButtonPressed(MenuButton button)
    {
        if (button == BtnsScript[selectedButton]){
            button.Pressed();
            if (button.objToShow == null){
                SceneManager.LoadScene("GameScene");
                return;
            }
            button.objToShow.SetActive(true);
            if (button.OverlapingWindow){
                gameObject.SetActive(false);
            } else
                setIgnoreInput(true);

            return;
        }
        if ( button == BtnsScript[ (selectedButton + 1) % numOfBtns] )
        {
            rotate(1);
        }
        if ( button == BtnsScript[ (selectedButton + numOfBtns - 1) % numOfBtns] )
        {
            rotate(-1);
        }
    }

    private void rotate(float dir)
    {
        if ((Time.time - lastInput) > inputCd && !ignoreInput)
        {
            BtnsScript[selectedButton].Highlighted();
            lastInput = Time.time;
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

    public void setIgnoreInput(bool newState)
    {
        ignoreInput = newState;
        if (newState){
            BtnsScript[selectedButton].setWiggled(false);
            foreach ( Renderer item in BtnsRenderer)
            {
                if (item != null)
                    item.material.SetFloat("_ShadowCoeff", maxShadowCoef);
            }
        } else {
            BtnsScript[selectedButton].setWiggled(true);
            RearrangeButtons();
        }
    }

    public void Update()
    {
        // Input Processing
        if (Abs(Input.GetAxis("Horizontal")) > 0.0f)
        {
            rotate(Input.GetAxis("Horizontal"));
        }

        if  ( (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return)) && !ignoreInput ){
            ButtonPressed(BtnsScript[selectedButton]);
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
            BtnsScript[selectedButton].setWiggled(false);
            if (rotatingRight)
                selectedButton = (selectedButton + 1) % numOfBtns;
            else
                selectedButton = (selectedButton + numOfBtns - 1) % numOfBtns;
            BtnsScript[selectedButton].setWiggled(true);
            rotationCurr = 0;
            RearrangeButtons();
            rotatingLeft = rotatingRight = false;
        }
    }
}

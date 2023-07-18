using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SwordSkill : MonoBehaviour
{
    public bool painting;
    private bool startDraw;
    private bool endDraw;
    private Texture2D canvas;
    private Color[] canvasPixels;
    private int canvasWidth = 32;
    private int canvasHeight = 32;
    float timer = 0;
    bool isMouseDown;


    CharacterController controller;
    public GameObject weapon;
    Animator weaponSkill;
    Animator playerAnimator;

    public Sprite sprite;
    public Texture2D secondTexture;

    public bool control;

    public RectTransform canvasRectTransform;

    public RawImage canvasImage;

    public GameObject animationControl;


    private void Start()
    {

        UnityEngine.Debug.Log("in Sword skills " + transform.position);
        controller = gameObject.GetComponent<CharacterController>();
        playerAnimator = gameObject.GetComponent<Animator>();
        weaponSkill = weapon.GetComponent<Animator>();
        // Erstelle das Canvas
        secondTexture = sprite.texture;
        canvas = new Texture2D(canvasWidth, canvasHeight);
        canvasPixels = new Color[canvasWidth * canvasHeight];

        // Initialisiere das Canvas mit weißer Farbe
        for (int i = 0; i < canvasPixels.Length; i++)
        {
            canvasPixels[i] = Color.white;
        }

        // Setze das Canvas als Textur für das RawImage um das casten der Attacken zu sehen :)
        canvasImage.texture = canvas;
        canvasImage.SetNativeSize();

        // Erhalte das RectTransform-Komponente des Canvas
        canvasRectTransform = canvasImage.GetComponent<RectTransform>();
    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0)) // linke Maustaste gedrückt
        {
            UnityEngine.Debug.Log("in Sword skills " + transform.position);
            weaponSkill.SetTrigger("castAttack");
            isMouseDown = true;
            timer = Time.time;
            startDraw = true;
            UnityEngine.Debug.Log("Timer gestartet!");

        }
        else if (Input.GetMouseButtonUp(0)) // linke Maustaste losgelassen
        {
            painting = false;
            isMouseDown = false;
            weaponSkill.ResetTrigger("castAttack");
            weaponSkill.Play("swordIdle");
            if(Time.time - timer >=2 && Time.time - timer < 5) {
                bool temp = CompareTextures();
                if(temp) {
                    UnityEngine.Debug.Log("Attack!");
                    playerAnimator.Play("playerAttack1");
                    
                    animationControl.transform.position = Vector3.zero;
                }
                
            }
        }

        if(Time.time - timer >= 8 && isMouseDown) {
            painting = false;
            UnityEngine.Debug.Log("zu lange gebraucht");
            weaponSkill.ResetTrigger("castAttack");
            //Alle anderen effekte waehrend Skillaktivierung z.B. nicht springen und movement speed down weg
        } else if(Time.time - timer >= 2 && isMouseDown) {
            painting = true;
            UnityEngine.Debug.Log("Fertig aufgeladen");
        }

        

        if (painting)
        {
            
            if(startDraw) {
                
                resetCanvas();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = false;
                startDraw = false; 

            }
            
            
            Vector2Int pixelPos = GetCanvasPixelPosition();
            Color color = Color.black;

            // Male den Pixel auf das Canvas
            if (pixelPos.x >= 0 && pixelPos.x < canvasWidth && pixelPos.y >= 0 && pixelPos.y < canvasHeight)
            {
                int pixelIndex = pixelPos.y * canvasWidth + pixelPos.x;
                canvasPixels[pixelIndex] = color;
                canvas.SetPixels(canvasPixels);
                canvas.Apply();
            }


            
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }

        

        

    }

    private Vector2Int GetCanvasPixelPosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        Vector2 localMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, mousePosition, null, out localMousePosition);

        Vector2 normalizedPosition = new Vector2(
            Mathf.InverseLerp(-screenCenter.x, screenCenter.x, localMousePosition.x),
            Mathf.InverseLerp(-screenCenter.y, screenCenter.y, localMousePosition.y)
        );

        int pixelX = Mathf.FloorToInt(normalizedPosition.x * canvasWidth);
        int pixelY = Mathf.FloorToInt(normalizedPosition.y * canvasHeight);

        return new Vector2Int(pixelX, pixelY);
    }

    private bool CompareTextures()
    {
        if (canvas.width != secondTexture.width || canvas.height != secondTexture.height)
        {
            Debug.LogError("Die Texturen haben unterschiedliche Größen und können nicht verglichen werden.");
            return false;
        }

        Color[] firstPixels = canvas.GetPixels();
        Color[] secondPixels = secondTexture.GetPixels();
        int matchCount = 0;
        int totalCount = 0;

        for(int i = 0; i < firstPixels.Length; i++) {
            if(firstPixels[i] == Color.black) {
                totalCount++;
                
            }
        }
        UnityEngine.Debug.Log("Alle Schwarzen: " + totalCount);
        

        

        for (int i = 0; i < firstPixels.Length; i++)
        {
            if (firstPixels[i] == secondPixels[i])
            {
                if(firstPixels[i] == Color.black){
                    matchCount++;
                }
            }
        }
        float matchPercentage;
        UnityEngine.Debug.Log("Matching: " + matchCount);
        if(totalCount >= 30) {
            matchPercentage = (float)matchCount / totalCount;
        } else {
            return false;
        }
        
        return matchPercentage >= 0.7f;
    }

    private void Test() {

        controller.Move(animationControl.transform.localPosition);

    }

    private void resetCanvas() {
        for (int i = 0; i < canvasPixels.Length; i++)
        {
            canvasPixels[i] = Color.white;
        }
    }
}

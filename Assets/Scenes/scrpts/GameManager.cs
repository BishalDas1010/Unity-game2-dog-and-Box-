using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public GameObject block;
    public float maxX;
    public Transform Sponpoint;
    public float SponRate;

    bool gameStarted = false;

    //score 
    public GameObject TapText;
    //store the 
    public TextMeshProUGUI ScoreText;
    int score = 0 ;


    void Start() { 
        TapText.SetActive(false);
    }

    void Update()
    {
        // Start on first tap (mobile) or left click (PC).
        bool pressedThisFrame =
            (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) ||
            (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame);

        if (pressedThisFrame && !gameStarted)
        {
            StarSpoming();
            gameStarted = true;
        }
    }

    private void StarSpoming()
    {
        InvokeRepeating("SponBlock", 0.5f, SponRate);
    }

    void SponBlock()
    {
        Vector3 SponPos = Sponpoint.position;
        SponPos.x = Random.Range(-maxX, maxX);
        Instantiate(block, SponPos, Quaternion.identity);
        score++;
        ScoreText.text =score.ToString();
    }
}
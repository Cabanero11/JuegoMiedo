using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    public Transform playerTransform;
    public float bobbingSpeed = 0.18f;
    public float bobbingAmount = 0.05f;
    public float midpoint = 2.0f;

    private float timer = 0.0f;
    private float defaultYPos;

    void Start()
    {
        defaultYPos = transform.localPosition.y;
    }

    void Update()
    {

        if (PauseMenuScript.isGamePausedStatic)
        {
            //Debug.Log("Parando camara bobbing");
            return;
        }

        EfectoBobbingCamara();

    }


    private void EfectoBobbingCamara()
    {
        float waveslice = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            timer += bobbingSpeed;
            if (timer > Mathf.PI * 2)
            {
                timer -= Mathf.PI * 2;
            }
        }

        if (waveslice != 0)
        {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Clamp(Mathf.Abs(horizontal) + Mathf.Abs(vertical), 0.0f, 1.0f);
            translateChange *= totalAxes;
            Vector3 newPos = transform.localPosition;
            newPos.y = defaultYPos + translateChange;
            transform.localPosition = newPos;
        }
        else
        {
            Vector3 newPos = transform.localPosition;
            newPos.y = defaultYPos;
            transform.localPosition = newPos;
        }
    }
}

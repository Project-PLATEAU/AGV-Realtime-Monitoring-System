using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraBehavior : MonoBehaviour
{
    public Button KentoraTopViewBtn;
    public Button WholeMapBtn;
    public Button Course1Btn;
    public Button Course2Btn;

    private bool isKentoraTopShown = false;
    private bool isWholeCamShown = false;
    private bool isCourse1CamShown = false;
    private bool isCourse2CamShown = false;

    private void OnKentoraTopBtnClick() {
        GameObject TopCamObj = GameObject.Find("TopCamera");
        Camera TopCam = TopCamObj.GetComponent<Camera>();

        GameObject KentoraCamObj = GameObject.Find("KentoraCamera");
        Camera KentoraCam = KentoraCamObj.GetComponent<Camera>();

        isKentoraTopShown = !isKentoraTopShown;

        if(isKentoraTopShown) {
            TopCam.rect = new Rect(0, 0, 1f, 1f);
            KentoraCam.rect = new Rect(0, 0, 0.25f, 0.25f);
            TopCam.depth = 1;
            KentoraCam.depth = 2;
        } else {
            TopCam.rect = new Rect(0, 0, 0.3f, 0.3f);
            KentoraCam.rect = new Rect(0, 0, 1.0f, 1.0f);
            TopCam.depth = 2;
            KentoraCam.depth = 1;
        }
    }

    private void OnWholeMapBtnClick() {
        GameObject WholeCamObj = GameObject.Find("WholeMapCamera");
        Camera WholeCam = WholeCamObj.GetComponent<Camera>();

        GameObject KentoraCamObj = GameObject.Find("KentoraCamera");
        Camera KentoraCam = KentoraCamObj.GetComponent<Camera>();

        isWholeCamShown = !isWholeCamShown;

        if(isWholeCamShown) {
            WholeCam.rect = new Rect(0, 0, 1f, 1f);
            KentoraCam.rect = new Rect(0, 0, 0.25f, 0.25f);
            WholeCam.depth = 1;
            KentoraCam.depth = 2;
        } else {
            WholeCam.rect = new Rect(0, 0, 0, 0);
            KentoraCam.rect = new Rect(0, 0, 1.0f, 1.0f);
            WholeCam.depth = 2;
            KentoraCam.depth = 1;
        }
    }

    private void OnCourse1BtnClick() {
        GameObject Course1CamObj = GameObject.Find("Course1Camera");
        Camera Course1Cam = Course1CamObj.GetComponent<Camera>();

        isCourse1CamShown = !isCourse1CamShown;

        if(isCourse1CamShown) {
            Course1Cam.rect = new Rect(0, 0, 1f, 1f);
            Course1Cam.depth = 10;
        } else {
            Course1Cam.rect = new Rect(0, 0, 0, 0);
            Course1Cam.depth = 0;
        }
    }

    private void OnCourse2BtnClick() {
        GameObject Course2CamObj = GameObject.Find("Course2Camera");
        Camera Course2Cam = Course2CamObj.GetComponent<Camera>();

        isCourse2CamShown = !isCourse2CamShown;

        if(isCourse2CamShown) {
            Course2Cam.rect = new Rect(0, 0, 1f, 1f);
            Course2Cam.depth = 10;
        } else {
            Course2Cam.rect = new Rect(0, 0, 0, 0);
            Course2Cam.depth = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        KentoraTopViewBtn.onClick.AddListener(OnKentoraTopBtnClick);
        WholeMapBtn.onClick.AddListener(OnWholeMapBtnClick);
        Course1Btn.onClick.AddListener(OnCourse1BtnClick);
        Course2Btn.onClick.AddListener(OnCourse2BtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

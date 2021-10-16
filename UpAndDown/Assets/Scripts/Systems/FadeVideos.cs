using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class FadeVideos : MonoBehaviour
{
    //Components
    public VideoPlayer vPlayer0;
    public VideoPlayer vPlayer1;
    public VideoPlayer vPlayer2;
    
    //Floats
    private const float TargetAlpha = 0.5f;
    private float _vPlayer0StartAlpha;
    private float _vPlayer1StartAlpha;
    private float _vPlayer2StartAlpha;
    private void Start()
    {
        _vPlayer0StartAlpha = vPlayer0.targetCameraAlpha;
        _vPlayer1StartAlpha = vPlayer1.targetCameraAlpha;
        _vPlayer2StartAlpha = vPlayer2.targetCameraAlpha;
    }
    

    public void FadeInVPlayer0()
    {
        vPlayer0.gameObject.SetActive(true);
        vPlayer0.targetCameraAlpha = TargetAlpha;
    }

    public void FadeOutVPlayer0()
    {
        vPlayer0.targetCameraAlpha = _vPlayer0StartAlpha;
        vPlayer0.gameObject.SetActive(false);
    }

    public void FadeInVPlayer1()
    {
        vPlayer1.gameObject.SetActive(true);
        vPlayer1.targetCameraAlpha = TargetAlpha;
    }

    public void FadeOutVPlayer1()
    {
        vPlayer1.targetCameraAlpha = _vPlayer1StartAlpha;
        vPlayer1.gameObject.SetActive(false);
    }

    public void FadeInVPlayer2()
    {
        vPlayer2.gameObject.SetActive(true);
        vPlayer2.targetCameraAlpha = TargetAlpha;
    }

    public void FadeOutVPlayer2()
    {
        vPlayer2.targetCameraAlpha = _vPlayer2StartAlpha;
        vPlayer2.gameObject.SetActive(false);
    }
    
}

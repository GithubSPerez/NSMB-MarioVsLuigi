using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SecondaryCameraPositioner : MonoBehaviour {
    bool destroyed = false;
    private int corner = -1;

    private GameObject[] otherCam = new GameObject[2];

    public void UpdatePosition() {
        
        if (corner == -1)
        {
            corner = 0;
            if (!GameManager.Instance.levelVerticalLoop)
            {
                //print("nop");
            }
            else
            {
                for (int i = 0; i < 2; i ++)
                {
                    otherCam[i] = Instantiate(gameObject, Camera.main.transform.Find("BaseCamera"));
                    otherCam[i].GetComponent<SecondaryCameraPositioner>().corner = 1 + i;
                    Transform camTrans = GameManager.Instance.transform.Find("Camera");
                    GameObject cam = camTrans.gameObject;
                    cam.GetComponent<Camera>().GetUniversalAdditionalCameraData().cameraStack.Add(otherCam[i].GetComponent<Camera>());
                }
                
                //print(cam.name);
            }
        }

        if (corner == 0)
        {
            for (int i = 0; i < 2; i ++)
            {
                if (otherCam[i] != null)
                    otherCam[i].GetComponent<SecondaryCameraPositioner>().UpdatePosition();
            }
        }

        if (destroyed)
            return;

        if (GameManager.Instance) {
            if (!GameManager.Instance.loopingLevel) {
                Destroy(gameObject);
                destroyed = true;
                return;
            }
            bool right = Camera.main.transform.position.x > GameManager.Instance.GetLevelMiddleX();

            float levelYMiddle = GameManager.Instance.GetLevelMinY() + GameManager.Instance.levelHeightTile / 4f;
            bool up = Camera.main.transform.position.y > levelYMiddle;

            transform.localPosition = new Vector3(0, 0, 0);
            if (corner == 0 || corner == 2) transform.localPosition += new Vector3(GameManager.Instance.levelWidthTile * (right ? -1 : 1), 0, 0);
            if (corner == 1 || corner == 2) transform.localPosition += new Vector3(0, GameManager.Instance.levelHeightTile * (up ? -1 : 1), 0);
        }
    }
}
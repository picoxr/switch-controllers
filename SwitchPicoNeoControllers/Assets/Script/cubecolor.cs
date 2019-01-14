using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pvr_UnitySDKAPI;
using UnityEngine.SceneManagement;

public class cubecolor : MonoBehaviour {

    // Use this for initialization
    public Text txt;
    public Text txt1;
    public Text txt2;
    public Text txt3;
    public Text txt4;
    public Text txt5;
    public Image[] image;
    public Image[] image1;
    private ControllerHand controllerHand;
    private float times = 0;
    void Start ()
    {
        for (int i = 0; i < image.Length; i++)
        {
            image[i].enabled = false;
        }
        for (int i = 0; i < image1.Length; i++)
        {
            image1[i].enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        handle_press(0, txt, "handle0：");
        handle_press(1, txt4, "handle1：");
        handle_electric(0, txt1);
        handle_electric(1, txt3);

        handle_position(0, txt2);
        handle_position(1, txt5);

        handle_direction(0);
        handle_direction(1);

        handle_click(0);
        handle_click(1);


        if (Input.GetMouseButton(0))
        {
            image[0].enabled = true;
            image1[0].enabled = true;
        }
    }

    public void red()
    {
        this.GetComponent<MeshRenderer>().material.color = Color.red;
    }
    public void blue()
    {
        this.GetComponent<MeshRenderer>().material.color = Color.blue;
    }
    public void yellow()
    {
        this.GetComponent<MeshRenderer>().material.color = Color.yellow;
    }

    public void next()
    {
        SceneManager.LoadScene(1);
    }

    public void slide(SwipeDirection num)
    {
        switch (num)
        {
            case SwipeDirection.SwipeUp:
                image1[0].enabled = true;
                image1[1].enabled = false;
                image1[2].enabled = false;
                image1[3].enabled = false;
              //  transform.Translate(0, 0, 20 * Time.deltaTime);
                break;
            case SwipeDirection.SwipeDown:
                image1[0].enabled = false;
                image1[1].enabled = true;
                image1[2].enabled = false;
                image1[3].enabled = false;
            //    transform.Translate(0,0,-20 * Time.deltaTime);
                break;
            case SwipeDirection.SwipeLeft:
                image1[0].enabled = false;
                image1[1].enabled = false;
                image1[2].enabled = false;
                image1[3].enabled = true;
              //  transform.Translate(-20 * Time.deltaTime,0,0);
                break;
            case SwipeDirection.SwipeRight:
                image1[0].enabled = false;
                image1[1].enabled = false;
                image1[2].enabled = true;
                image1[3].enabled = false;
              //  transform.Translate(20 * Time.deltaTime,0,0);
                break;
            default:
                times += Time.deltaTime;
                if (times > 0.5f)
                {
                    times = 0;
                    for (int i = 0; i < image.Length; i++)
                    {
                        image1[i].enabled = false;
                    }
                }
              
                break;
        }
    }

    public void click(TouchPadClick touchPadClick)
    {
        switch (touchPadClick)
        {
            case TouchPadClick.ClickUp:
                image[0].enabled = true;
                image[1].enabled = false;
                image[2].enabled = false;
                image[3].enabled = false;
               // transform.Translate(0, 0, 30 * Time.deltaTime);
                break;
            case TouchPadClick.ClickDown:
                image[0].enabled = false;
                image[1].enabled = true;
                image[2].enabled = false;
                image[3].enabled = false;
               // transform.Translate(0,0,-30 * Time.deltaTime);
                break;
            case TouchPadClick.ClickLeft:
                image[0].enabled = false;
                image[1].enabled = false;
                image[2].enabled = false;
                image[3].enabled = true;
               // transform.Translate(-30 * Time.deltaTime,0,0);
                break;
            case TouchPadClick.ClickRight:
                image[0].enabled = false;
                image[1].enabled = false;
                image[2].enabled = true;
                image[3].enabled = false;
              //  transform.Translate(30 * Time.deltaTime, 0, 0);
                break;
            default:
                times += Time.deltaTime;
                if (times > 0.5f)
                {
                    times = 0;
                    for (int i = 0; i < image.Length; i++)
                    {
                        image[i].enabled = false;
                    }
                }

                break;
        }
    }

    public void handle_electric(int num,Text txt)
    {

        switch (num)
        {
            case 0:
                int dian = Controller.UPvr_GetControllerPower(0);
                txt.text = "Main handle battery level：" + dian.ToString();
                break;
            case 1:
                int dian1 = Controller.UPvr_GetControllerPower(1);
                txt.text = "Another handle battery level：" + dian1.ToString();
                break;
            default:
                break;
        }
    }

    public void handle_position(int num, Text txt)
    {

        Vector3 vec = Controller.UPvr_GetControllerPOS(num);
        float x = vec.x;
        float y = vec.y;
        float z = vec.z;
        if (num == 0)
        {

            txt.text = "Handle0Position：" + "x:" + x.ToString() + " ,y:" + y.ToString() + " ,z:" + z.ToString();
        }
        if (num == 1)
        {
            txt.text = "Handle1Position：" + "x:" + x.ToString() + " ,y:" + y.ToString() + " ,z:" + z.ToString();
        }
    }

    public void handle_press(int num,Text txt,string word)
    {
        if (Controller.UPvr_GetKey(num, Pvr_KeyCode.APP))
        {
            txt.text =word+ "App button is pressed";
        }
        else if (Controller.UPvr_GetKey(num, Pvr_KeyCode.HOME))
        {
            txt.text = word + "HOME button is pressed";
        }
        else if (Controller.UPvr_GetKey(num, Pvr_KeyCode.TOUCHPAD))
        {
            txt.text = word + "TOUCHPAD button is pressed";
        }
        else if (Controller.UPvr_GetKey(num, Pvr_KeyCode.VOLUMEDOWN))
        {
            txt.text = word + "Volume- button is pressed";
        }
        else if (Controller.UPvr_GetKey(num, Pvr_KeyCode.VOLUMEUP))
        {
            txt.text = word + "Volume+ button is pressed";
        }
        else if (Controller.UPvr_GetKey(num, Pvr_KeyCode.TRIGGER))
        {
           
            txt.text = word + "Trigger button is pressed";
        }
        else
        {
            txt.text =word+ "No button is pressed";
        }
    }

    public void handle_direction(int num)
    {
        SwipeDirection swipeDirection = Controller.UPvr_GetSwipeDirection(num);
        slide(swipeDirection);
    }

    public void handle_click(int num)
    {
        TouchPadClick touchPadClick = Controller.UPvr_GetTouchPadClick(num);
        click(touchPadClick);
    }
}

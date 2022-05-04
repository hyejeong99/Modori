using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WritingReport_RayCast : MonoBehaviour
{
    float distance = 15f;
    int num = 0;
    public int maxNum = 1; // 2문장 기준
    string hittedTag;
    bool hitted = false;
    GameObject curParent;
    RaycastHit2D hit;

    Vector3 MousePosition;
    Camera cam;

    
    void Start()
    {
        cam = GetComponent<Camera>();

        curParent = GameObject.FindGameObjectsWithTag(num.ToString())[1];
        for (int i = 0; i < 4; i++)
        {
            curParent.transform.GetChild(i).gameObject.SetActive(true);
        }
        Debug.Log("선택 text 활성화");
    }

    
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) //   || Input.GetTouch(0).phase == TouchPhase.Began
        {
            MousePosition = Input.mousePosition;    // todo : 터치 버전 추가해야함
            MousePosition = cam.ScreenToWorldPoint(MousePosition);

            hit = Physics2D.Raycast(MousePosition, transform.forward, distance);
            
            Debug.DrawRay(MousePosition, transform.forward * 500, Color.red, 0.3f);
            StartCoroutine("RayDelay");
        }

    }


    void Ray(ref RaycastHit2D hit, ref int num)
    {
        hittedTag = hit.collider.gameObject.tag;
        //  부모tag > 빈칸텍스트=0 , 선택텍스트=1
        //  선택 text 활성화
        curParent = GameObject.FindGameObjectsWithTag(num.ToString())[1];
        for (int i = 0; i < 4; i++)
        {
            curParent.transform.GetChild(i).gameObject.SetActive(true);
        }
        Debug.Log("선택 text 활성화");

        
        //  빈칸text 선택에 맞게 활성화
        curParent = GameObject.FindGameObjectsWithTag(num.ToString())[0];
        if (hittedTag.Equals("choice1"))
            curParent.transform.GetChild(0).gameObject.SetActive(true);  
        else if (hittedTag.Equals("choice2"))
            curParent.transform.GetChild(1).gameObject.SetActive(true);
        else if (hittedTag.Equals("choice3"))
            curParent.transform.GetChild(2).gameObject.SetActive(true);
        else
            curParent.transform.GetChild(3).gameObject.SetActive(true);
        Debug.Log("빈칸 text 활성화");
        

        //  선택text 비활성화
        curParent = GameObject.FindGameObjectsWithTag(num.ToString())[1];
        for (int i = 0; i < 4; i++)
        {
            curParent.transform.GetChild(i).gameObject.GetComponent<BoxCollider2D>().enabled = false;
            curParent.transform.GetChild(i).gameObject.SetActive(false);
        }
        Debug.Log("선택 text 비활성화");
        num++;

        
        //  마지막 선택지라면
        if (num == maxNum)
        {
            StartCoroutine("RayDelay");
            StartCoroutine("Rendering");
        }
        else if(num < maxNum)
        {
            StartCoroutine("RayDelay");
        }
    }


    IEnumerator Rendering()
    {
        yield return new WaitForEndOfFrame();


        byte[] imgBytes;
        string path = @"C:\UnitySpace\Modori\Assets\Image\report.png";

        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        texture.Apply();

        imgBytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, imgBytes);
    }

    IEnumerator RayDelay()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("1초 기다리기");
        if (hit)
        {
            Ray(ref hit, ref num);
        }
    }

}

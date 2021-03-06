using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReadingReport : MonoBehaviour
{
   
    GameObject exit;

    SpriteRenderer spriteR;
    Sprite[] sprites;

    int num = 0;
    string path;
    int loadNum = 0;
    int curNum = 0;

    void Start()
    {
        /* PlayerPrefs.DeleteKey("mermaid");
         PlayerPrefs.DeleteKey("sister");
         PlayerPrefs.DeleteKey("prince");
         PlayerPrefs.DeleteKey("witch");*/

        loadNum = PlayerPrefs.GetInt("loadNum");

        exit = GameObject.Find("exit").transform.GetChild(0).gameObject;
        spriteR = this.GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>("Reports");

        //  파일이 존재한다면
        if(sprites.Length > 0)
            spriteR.sprite = sprites[num++];
        //  기존 이미지 파일이 없다면
        else if (loadNum > 0 && curNum < loadNum)
        {
            Debug.Log("loadNum:" + loadNum + "\ncurNum:" + curNum);
            path = PlayerPrefs.GetString("path"+curNum);
            curNum++;

            byte[] imgBytes = { };
            if (path.Length > 0)
                imgBytes = System.IO.File.ReadAllBytes(path);

            if (imgBytes.Length > 0)
            {
                Debug.Log("imgBytes 있음");
                Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                texture.LoadImage(imgBytes);
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
            }
        }
    }


    private void Update()
    {
        if(Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            OnMouseDown();
        }
    }


    private void OnMouseDown()
    {
        exit.SetActive(true);
        Debug.Log(num);
        if (num < sprites.Length)
        {
            spriteR.sprite = sprites[num++];
            exit.SetActive(true);
        }
        else if (loadNum > 0 && curNum < loadNum)
        {
            string path = PlayerPrefs.GetString("path" + curNum);
            curNum++;

            byte[] imgBytes= { };
            if (path.Length>0)   
                imgBytes = System.IO.File.ReadAllBytes(path);

            if (imgBytes.Length > 0)
            {
                Debug.Log("imgBytes 있음");
                Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                texture.LoadImage(imgBytes);
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));

                exit.SetActive(true);
            }
            
        }
        
    }

    
}

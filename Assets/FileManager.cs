using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using SimpleFileBrowser;
using System.Collections.Generic;

public class FileManager : MonoBehaviour
{
    public string location;
    public RawImage ex;
    public Sprite sp;
    public Text text;
    private void Start() {
        location = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SetPath();
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            LoadSavedGames();
        }
    }
    public void SetPath() {
		FileBrowser.ShowLoadDialog((paths) => { Debug.Log("Selected: " + paths[0]); location = paths[0]; },
								  () => { Debug.Log("Canceled"); },
								  FileBrowser.PickMode.Folders, false, null, null, "Select Folder", "Select");

	}

    void Screenshot() {
        Camera currentCamera = Camera.main;
        int tw = 2048; 
        int th = 2048; 
        RenderTexture rt = new RenderTexture(tw, th, 24, RenderTextureFormat.ARGB32);
        rt.antiAliasing = 4;

        currentCamera.targetTexture = rt;

        currentCamera.Render();//
r
        Texture2D thumb = new Texture2D(tw, th, TextureFormat.RGB24, false);

        RenderTexture.active = rt;

        thumb.ReadPixels(new Rect(0, 0, tw, th), 0, 0, false);

        byte[] bytes = thumb.EncodeToPNG();
        Object.Destroy(thumb);

        File.WriteAllBytes(location + "/Image.png", bytes);
        RenderTexture.active = null;

        currentCamera.targetTexture = null;

        rt.DiscardContents();

        sp = LoadNewSprite(location+ "/Image.png");
        ex.texture = sp.texture;
        ex.color = new Color(1, 1, 1, 1);
    }

    public  Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.Tight) {

        Texture2D SpriteTexture = LoadTexture(FilePath);
        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit, 0, spriteType);

        return NewSprite;
    }
    public  Texture2D LoadTexture(string FilePath) {
        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath)) {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           
            if (Tex2D.LoadImage(FileData))          
                return Tex2D;                
        return null;                     
    }


    void LoadSavedGames() {
        List<string> nameFile = new List<string>();
        if (Directory.Exists(location)) {
            string worldsFolder = location;
            DirectoryInfo d = new DirectoryInfo(worldsFolder);
            foreach (var file in d.GetFiles("*.png")) {
                nameFile.Add(file.Name);
            }
        } else {
            return;
        }

        if (nameFile.Count==0) {
            Screenshot();
            text.text = "save done!";
            text.color = new Color(0, 1, 0, 1);
        } else {
           
            text.text = "The same location as the saved image!";
            text.color = new Color(1,0,0,1);
        }
    }

}

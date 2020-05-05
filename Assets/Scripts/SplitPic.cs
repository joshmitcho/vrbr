using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SplitPic : MonoBehaviour {

    Mesh mesh;
    Vector3[] vertices;
    Vector2[] uvs;

    Renderer rend;
    Object[] pictures;

    int n;
    public Text txt;

    public Text log;
    public bool isRight;

    // Use this for initialization
    void Start () {

        //map left half of image to left panel and right half to right panel
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        uvs = new Vector2[vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            if (isRight)
            {
                uvs[i] = new Vector2(-(vertices[i].x / 20f + 0.25f), -(vertices[i].z / 10f + 0.5f));
            }
            else
            {
                uvs[i] = new Vector2(-(vertices[i].x / 20f + 0.75f), -(vertices[i].z / 10f + 0.5f));
            }
        }
        mesh.uv = uvs;

        //Load images for this module
        pictures = Resources.LoadAll("Lab" + Gaze.labNum.ToString(), typeof(Texture2D)).Cast<Texture2D>().ToArray();

        
        //Set initial picture
        rend = GetComponent<Renderer>();
        n = 0;
        ChangePicture();

    }

    public void ChangePicture()
    {
        rend.material.mainTexture = (Texture2D)pictures[n];

        txt.text = string.Format("{0}/{4}: Q{1}, Q{2}, Q{3}", n+1, (n+1)*3-2, (n+1)*3-1, (n+1)*3, pictures.Length );

        n = (n + 1) % pictures.Length;
    }

}

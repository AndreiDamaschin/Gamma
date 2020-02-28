using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class USBCOM : MonoBehaviour
{
    short i;
    float y;
    short channel = 0;
    Material material;
    GameObject GameObject;
    Texture2D texTexture2D;
    static TextMesh textMesh;
    Color color = new Color();
    public InputField inputField;
    static GameObject Gameobject;
    Element[] element = new Element[8129];
    static Vector3 vector3 = new Vector3();
    System.Random random = new System.Random();
    USBCOMDLL.USBCOMDLL uSBCOMDLL = new USBCOMDLL.USBCOMDLL();
    void Start()
    {
        inputField.onEndEdit.AddListener((obj) => {try {uSBCOMDLL.Start($"COM{inputField.text}");} catch (Exception e) {Debug.LogError($"Not port found ! {e.Message}");}});
        (texTexture2D = new Texture2D(852, 480)).LoadImage(File.Exists(@"Assets/Scenes/Background.jpg") ? File.ReadAllBytes(@"Assets/Scenes/Background.jpg") : null);
        (GameObject = new GameObject("Camera", typeof(Camera))).transform.SetParent(transform);
        element[0] = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<Element>();
        Gameobject = GameObject.CreatePrimitive(PrimitiveType.Plane);
        material = Gameobject.GetComponent<Renderer>().material;
        Camera camera = GameObject.GetComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.farClipPlane = 100;
        camera.orthographic = true;
        camera.orthographicSize = 30;
        camera.nearClipPlane = 0.01f;
        Gameobject.name = "Background";
        material.mainTexture = texTexture2D;
        material.EnableKeyword("_EMISSION");
        camera.backgroundColor = Color.black;
        GameObject = new GameObject("Bar Chart");
        GameObject.transform.SetParent(transform);
        vector3.Set(13, 1, 6);
        Gameobject.transform.localScale = vector3;
        Gameobject.transform.SetParent(transform);
        vector3.Set(0, 0, 50);
        GameObject.transform.localPosition = vector3;
        vector3.Set(0, 0, 100);
        Gameobject.transform.localPosition = vector3;
        vector3.Set(90, 270, 90);
        Gameobject.transform.localEulerAngles = vector3;
        material.SetColor("_EmissionColor", Color.white);
        material.SetTexture("_EmissionMap", texTexture2D);
        for (i = 1; i <= 8128; i++)
        {
            y = random.Next(random.Next(50));
            color.r = (float)random.NextDouble();
            color.g = (float)random.NextDouble();
            color.b = (float)random.NextDouble();
            vector3.Set(i * 0.0125f - 51, y / 2 - 23.5f, 0);
            element[i] = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<Element>();
            material = element[i].GetComponent<Renderer>().material;
            element[i].transform.SetParent(GameObject.transform);
            element[i].transform.localPosition = vector3;
            material.SetColor("_EmissionColor", color);
            vector3.Set(0.01f, y, 0.01f);
            element[i].transform.localScale = vector3;
            material.EnableKeyword("_EMISSION");
            element[i].reports = y.ToString();
            element[i].channel = i.ToString();
            material.color = color;
            if (i % 500 == 0)
            {
                vector3.Set(i * 0.0125f - 51, -25, 0);
                Gameobject = new GameObject(i.ToString());
                textMesh = Gameobject.AddComponent<TextMesh>();
                Gameobject.transform.SetParent(GameObject.transform);
                Gameobject.transform.localPosition = vector3;
                textMesh.anchor = TextAnchor.UpperCenter;
                textMesh.fontStyle = FontStyle.Bold;
                textMesh.text = i.ToString();
                textMesh.color = Color.white;
                textMesh.fontSize = 20;
            }
        }
        for (i = 5; i <= 50; i += 5)
        {
            vector3.Set(-55.5f, i - 25, 0);
            Gameobject = new GameObject(i.ToString());
            textMesh = Gameobject.AddComponent<TextMesh>();
            Gameobject.transform.SetParent(GameObject.transform);
            Gameobject.transform.localPosition = vector3;
            textMesh.fontStyle = FontStyle.Bold;
            textMesh.text = i.ToString();
            textMesh.color = Color.white;
            textMesh.fontSize = 20;
        }
        for (i = 0; i < 3; i++)
        {
            Gameobject = new GameObject($"Point Light {i.ToString()}");
            Gameobject.AddComponent<Light>().type = LightType.Point;
            Gameobject.transform.SetParent(GameObject.transform);
            vector3.Set(62 - i * 4, -22, i % 2 == 0 ? 0 : -4);
            Gameobject.transform.localPosition = vector3;
        }
        Gameobject = new GameObject("Coordinates");
        textMesh = Gameobject.AddComponent<TextMesh>();
        Gameobject.transform.SetParent(GameObject.transform);
        element[0].transform.SetParent(GameObject.transform);
        material = element[0].GetComponent<Renderer>().material;
        vector3.Set(58, -22, 0);
        element[0].transform.localPosition = vector3;
        vector3.Set(55, 30, 0);
        Gameobject.transform.localPosition = vector3;
        vector3.Set(5, 5, 5);
        element[0].transform.localScale = vector3;
        textMesh.fontStyle = FontStyle.Bold;
        element[0].channel = "C.";
        element[0].reports = "R.";
        textMesh.color = Color.red;
        textMesh.fontSize = 20;
    }
    void LateUpdate()
    {
        for (i = 0; i <= 8128; i++)
            element[i].transform.Rotate(0, 0.5f, 0);
        if (uSBCOMDLL.channel != 0 && uSBCOMDLL.channel != channel)
        {
            for (i = uSBCOMDLL.channel; i < uSBCOMDLL.values.Length; i++)
            {
                vector3.Set(0.01f, uSBCOMDLL.values[i], 0.01f);
                element[i].transform.localScale = vector3;
                vector3.Set(i * 0.0125f - 51, uSBCOMDLL.values[i] / 2 - 23.5f, 0);
                element[i].transform.localPosition = vector3;
            }
            channel = uSBCOMDLL.channel;
        }
        if (y % 5 == 0)
        {
            y = (short)(y == 1000 ? 0 : y);
            color.r = (float)random.NextDouble();
            color.g = (float)random.NextDouble();
            color.b = (float)random.NextDouble();
            material.color = color;
        }
        y++;
    }

    void e()
    {

    }

    class Element : MonoBehaviour
    {
        public string channel;
        public string reports;
        void OnMouseOver()
        {
            textMesh.text = $"{channel} - {reports}";
        }
    }
}

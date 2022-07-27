using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;

public class Detection : MonoBehaviour
{
    private WebCamTexture webcamTexture;
    CascadeClassifier cascade;

    /*private string filename;
    private CascadeClassifier cascade;
    private MatOfRect hands;
    private Texture2D texture;
    private Mat rgbaMat;
    private Mat grayMat;*/
    // Start is called before the first frame update
    OpenCvSharp.Rect hand1;
    OpenCvSharp.Rect hand2;


    public float HandOneDiff = 0;
    public float HandTwoDiff = 0;

    private float HandOneOld = 0;
    private float HandTwoOld = 0;
    
    void Start()
    {
        //obtain cameras avialable

        // parseXmlFile (data);

        WebCamDevice[] cam_devices = WebCamTexture.devices; //create camera texture
        webcamTexture = new WebCamTexture(cam_devices[0].name); //start camera
        webcamTexture.Play(); // initialize members;
        cascade = new CascadeClassifier(Application.dataPath + @"/haarcascade_closed_frontal_palm_default.xml");
        
        //initializeData();
    }

    void Update()
    {

        GetComponent<Renderer>().material.mainTexture = webcamTexture;

        Mat frame = OpenCvSharp.Unity.TextureToMat(webcamTexture);
        findNewhand(frame);
        display(frame);
    }
    void findNewhand(Mat frame)
    {
        var hands = cascade.DetectMultiScale(frame, 1.1, 2, HaarDetectionType.ScaleImage);
        if (hands.Length >= 1)
        {
            
            hand1 = hands[0]; 
            
            HandOneDiff =  hand1.X - HandOneOld ;

           // Debug.Log("Hand 1 = " + HandOneDiff);

            if (hands.Length >= 2)
            {
                hand2 = hands[1]; 

                HandTwoDiff =  hand2.Y - HandTwoOld ;

             //   Debug.Log("Hand 2 = " + HandTwoDiff);
            }
         }
    }
    void display(Mat frame)
    {
        if (hand1 != null)
        {
            frame.Rectangle(hand1,new Scalar(0,10,100),10);
        }

        
        if (hand2 != null)
        {
            frame.Rectangle(hand2,new Scalar(250,60,40),10);
        }

        Texture newTexture = OpenCvSharp.Unity.MatToTexture(frame);
        GetComponent<Renderer>().material.mainTexture = newTexture;
    }
}
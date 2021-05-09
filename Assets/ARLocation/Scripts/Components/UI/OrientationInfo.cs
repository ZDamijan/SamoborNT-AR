using UnityEngine;
using UnityEngine.UI;

namespace ARLocation.UI
{
    public class OrientationInfo : MonoBehaviour
    {
        private GameObject redArrow;
        private GameObject trueNorthLabel;
        private GameObject magneticNorthLabel;
        private GameObject headingAccuracyLabel;
        private GameObject latitudeLabel;
        private GameObject longitudeLabel;
        private GameObject altitudeLabel;
        private GameObject compassImage;
        private ARLocationProvider locationProvider;
        private GameObject mainCamera;
        private bool isMainCameraNull;
        private Text text;
        private Text text1;
        private Text text2;
        private Text text3;
        private Text text4;
        private Text text5;
        private RectTransform rectTransform;
        private RectTransform rectTransform1;

        // Use this for initialization
        void Start()
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            isMainCameraNull = mainCamera == null;

            locationProvider = ARLocationProvider.Instance;

            redArrow = GameObject.Find(gameObject.name + "/Panel/CompassImage/RedArrow");
            trueNorthLabel = GameObject.Find(gameObject.name + "/Panel/TrueNorthLabel");
            magneticNorthLabel = GameObject.Find(gameObject.name + "/Panel/MagneticNorthLabel");
            headingAccuracyLabel = GameObject.Find(gameObject.name + "Panel/HeadingAccuracyLabel");
            latitudeLabel = GameObject.Find(gameObject.name + "/Panel/LatitudeLabel");
            longitudeLabel = GameObject.Find(gameObject.name + "/Panel/LongitudeLabel");
            altitudeLabel = GameObject.Find(gameObject.name + "Panel/AltitudeLabel");
            compassImage = GameObject.Find(gameObject.name + "Panel/CompassImage");

            text = trueNorthLabel.GetComponent<Text>();
            text1 = magneticNorthLabel.GetComponent<Text>();
            text2 = headingAccuracyLabel.GetComponent<Text>();
            text3 = latitudeLabel.GetComponent<Text>();
            text4 = longitudeLabel.GetComponent<Text>();
            text5 = altitudeLabel.GetComponent<Text>();

            rectTransform1 = compassImage.GetComponent<RectTransform>();
            rectTransform = redArrow.GetComponent<RectTransform>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isMainCameraNull)
            {
                return;
            }

            var currentHeading = locationProvider.CurrentHeading.heading;
            var currentMagneticHeading = locationProvider.CurrentHeading.magneticHeading;
            var currentAccuracy = locationProvider.Provider.CurrentHeading.accuracy;
            var currentLatitude = locationProvider.CurrentLocation.latitude;
            var currentLongitude = locationProvider.CurrentLocation.longitude;
            var currentAltitude = locationProvider.CurrentLocation.altitude;

            text.text = "TRUE NORTH: " + currentHeading;
            text1.text = "MAGNETIC NORTH: " + currentMagneticHeading;
            text2.text = "ACCURACY: " + currentAccuracy;
            text3.text = "LAT: " + currentLatitude;
            text4.text = "LONG: " + currentLongitude;
            text5.text = "ALT: " + currentAltitude;

            rectTransform.rotation = Quaternion.Euler(0, 0, (float)currentMagneticHeading);
            rectTransform1.rotation = Quaternion.Euler(0, 0, (float)currentHeading);
        }
    }
}

using UnityEngine;

public class LockHead : MonoBehaviour {

    void Start()
    {
        UnityEngine.XR.XRDevice.DisableAutoXRCameraTracking(GetComponent<Camera>(), true);

        Transform t = GetComponent<Transform>();

        t.rotation = new Quaternion(0, 0, 0, 0);
    }

}

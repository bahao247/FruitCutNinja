/*===============================================================================
Copyright (c) 2015-2016 PTC Inc. All Rights Reserved. Confidential and Proprietary - 
Protected under copyright and other laws.
Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.   
===============================================================================*/


using System;
using UnityEngine;

using Vuforia;

public class VRIntegrationHelper : MonoBehaviour
{
    private static Matrix4x4 mLeftCameraMatrixOriginal;
    private static Matrix4x4 mRightCameraMatrixOriginal;

    private static Camera mLeftCamera;
    private static Camera mRightCamera;

    private static HideExcessAreaAbstractBehaviour mLeftExcessAreaBehaviour;
    private static HideExcessAreaAbstractBehaviour mRightExcessAreaBehaviour;

    private static Rect mLeftCameraPixelRect;
    private static Rect mRightCameraPixelRect;

    private static bool mLeftCameraDataAcquired = false;
    private static bool mRightCameraDataAcquired = false;

    public bool IsLeft;
    public Transform TrackableParent;

    void Awake()
    {
        GetComponent<Camera>().fieldOfView = 90f;
    }

    void Start()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
    }

    void OnVuforiaStarted()
    {
        mLeftCamera = DigitalEyewearARController.Instance.PrimaryCamera;
        mRightCamera = DigitalEyewearARController.Instance.SecondaryCamera;

        mLeftExcessAreaBehaviour = mLeftCamera.GetComponent<HideExcessAreaAbstractBehaviour>();
        mRightExcessAreaBehaviour = mRightCamera.GetComponent<HideExcessAreaAbstractBehaviour>();
    }

    void LateUpdate()
    {
        // to this only once per frame, not for both cameras
        if (IsLeft)
        {
            if (mLeftCameraDataAcquired && mRightCameraDataAcquired)
            {
                // make sure the central anchor point is set to the latest head tracking pose:
                DigitalEyewearARController.Instance.CentralAnchorPoint.localRotation = mLeftCamera.transform.localRotation;
                DigitalEyewearARController.Instance.CentralAnchorPoint.localPosition = mLeftCamera.transform.localPosition;

                // temporarily set the primary and secondary cameras to their offset position and set the pixelrect they will have for rendering
                Vector3 localPosLeftCam = mLeftCamera.transform.localPosition;
                Rect leftCamPixelRect = mLeftCamera.pixelRect;
                Vector3 leftCamOffset = mLeftCamera.transform.right.normalized * mLeftCamera.stereoSeparation * -0.5f;
                mLeftCamera.transform.position = mLeftCamera.transform.position + leftCamOffset;
                mLeftCamera.pixelRect = mLeftCameraPixelRect;

                Vector3 localPosRightCam = mRightCamera.transform.localPosition;
                Rect rightCamPixelRect = mRightCamera.pixelRect;
                Vector3 rightCamOffset = mRightCamera.transform.right.normalized * mRightCamera.stereoSeparation * 0.5f;
                mRightCamera.transform.position = mRightCamera.transform.position + rightCamOffset;
                mRightCamera.pixelRect = mRightCameraPixelRect;

                BackgroundPlaneBehaviour bgPlane = mLeftCamera.GetComponentInChildren<BackgroundPlaneBehaviour>();
                bgPlane.BackgroundOffset = mLeftCamera.transform.position - DigitalEyewearARController.Instance.CentralAnchorPoint.position;

                mLeftExcessAreaBehaviour.PlaneOffset = mLeftCamera.transform.position - DigitalEyewearARController.Instance.CentralAnchorPoint.position;
                mRightExcessAreaBehaviour.PlaneOffset = mRightCamera.transform.position - DigitalEyewearARController.Instance.CentralAnchorPoint.position;

                if (TrackableParent != null)
                    TrackableParent.localPosition = Vector3.zero;

                // update Vuforia explicitly
                VuforiaARController.Instance.UpdateState(false, true);

                if (TrackableParent != null)
                    TrackableParent.position += bgPlane.BackgroundOffset;

                // set the projection matrices for skewing
                VuforiaARController.Instance.ApplyCorrectedProjectionMatrix(mLeftCameraMatrixOriginal, true);
                VuforiaARController.Instance.ApplyCorrectedProjectionMatrix(mRightCameraMatrixOriginal, false);


                // read back the projection matrices set by Vuforia and set them to the stereo cameras
                // not sure if the matrices would automatically propagate between the left and right, so setting it explicitly twice
                mLeftCamera.SetStereoProjectionMatrices(mLeftCamera.projectionMatrix, mRightCamera.projectionMatrix);
                mRightCamera.SetStereoProjectionMatrices(mLeftCamera.projectionMatrix, mRightCamera.projectionMatrix);

                // reset the left camera
                mLeftCamera.transform.localPosition = localPosLeftCam;
                mLeftCamera.pixelRect = leftCamPixelRect;

                // reset the position of the right camera
                mRightCamera.transform.localPosition = localPosRightCam;
                mRightCamera.pixelRect = rightCamPixelRect;
            }
        }
    }

    // OnPreRender is called once per camera each frame
    void OnPreRender()
    {
		if (IsLeft && !mLeftCameraDataAcquired)
		{
			if (
				!float.IsNaN(mLeftCamera.projectionMatrix[0,0]) &&
				!float.IsNaN(mLeftCamera.projectionMatrix[0,1]) &&
				!float.IsNaN(mLeftCamera.projectionMatrix[0,2]) &&
				!float.IsNaN(mLeftCamera.projectionMatrix[0,3]) &&
				!float.IsNaN(mLeftCamera.projectionMatrix[1,0]) &&
				!float.IsNaN(mLeftCamera.projectionMatrix[1,1]) &&
				!float.IsNaN(mLeftCamera.projectionMatrix[1,2]) &&
				!float.IsNaN(mLeftCamera.projectionMatrix[1,3]) &&
				!float.IsNaN(mLeftCamera.projectionMatrix[2,0]) &&
				!float.IsNaN(mLeftCamera.projectionMatrix[2,1]) &&
				!float.IsNaN(mLeftCamera.projectionMatrix[2,2]) &&
				!float.IsNaN(mLeftCamera.projectionMatrix[2,3]) &&
				!float.IsNaN(mLeftCamera.projectionMatrix[3,0]) &&
				!float.IsNaN(mLeftCamera.projectionMatrix[3,1]) &&
				!float.IsNaN(mLeftCamera.projectionMatrix[3,2]) &&
				!float.IsNaN(mLeftCamera.projectionMatrix[3,3])
			)
			{
				mLeftCameraMatrixOriginal = mLeftCamera.projectionMatrix;
				mLeftCameraPixelRect = mLeftCamera.pixelRect;
				mLeftCameraDataAcquired = true;
			}
		}
		else if (!mRightCameraDataAcquired)
		{
			if (
				!float.IsNaN(mRightCamera.projectionMatrix[0,0]) &&
				!float.IsNaN(mRightCamera.projectionMatrix[0,1]) &&
				!float.IsNaN(mRightCamera.projectionMatrix[0,2]) &&
				!float.IsNaN(mRightCamera.projectionMatrix[0,3]) &&
				!float.IsNaN(mRightCamera.projectionMatrix[1,0]) &&
				!float.IsNaN(mRightCamera.projectionMatrix[1,1]) &&
				!float.IsNaN(mRightCamera.projectionMatrix[1,2]) &&
				!float.IsNaN(mRightCamera.projectionMatrix[1,3]) &&
				!float.IsNaN(mRightCamera.projectionMatrix[2,0]) &&
				!float.IsNaN(mRightCamera.projectionMatrix[2,1]) &&
				!float.IsNaN(mRightCamera.projectionMatrix[2,2]) &&
				!float.IsNaN(mRightCamera.projectionMatrix[2,3]) &&
				!float.IsNaN(mRightCamera.projectionMatrix[3,0]) &&
				!float.IsNaN(mRightCamera.projectionMatrix[3,1]) &&
				!float.IsNaN(mRightCamera.projectionMatrix[3,2]) &&
				!float.IsNaN(mRightCamera.projectionMatrix[3,3])
			)
			{
				mRightCameraMatrixOriginal = mRightCamera.projectionMatrix;
				mRightCameraPixelRect = mRightCamera.pixelRect;
				mRightCameraDataAcquired = true;
			}
		}       
    }
}

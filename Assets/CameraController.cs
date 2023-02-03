using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   public Transform cameraHolder;
   // Update is called once per frame
   void Update()
   {
      gameObject.transform.position = cameraHolder.transform.position;
   }
}

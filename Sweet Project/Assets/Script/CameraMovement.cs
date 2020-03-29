using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.LepiStudios.SweetProject {

	public class CameraMovement : MonoBehaviour
	{

        #region Private Serialization Fields

        [Tooltip("The player")]
        [SerializeField] private GameObject localPlayer;

        [Tooltip("The upper right corner of the movement")]
        [SerializeField] private Transform upperRightPosition;

        [Tooltip("The lower left corner of the movement")]
        [SerializeField] private Transform lowerLeftCorner;

        #endregion

        #region MonoBehaviour Callbacks


        ///<summary> MonoBehaviour method called on GameObject by Unity during initialization phase. </summary>
        public void AssignCameraToLocalPlayer(GameObject player)
		{
            localPlayer = player;

            //camera position at the beginning exactly on the player
            transform.position = new Vector3(localPlayer.transform.position.x, localPlayer.transform.position.y, transform.position.z);
		}

		///<summary> MonoBehaviour method called on GameObject by Unity every frame. </summary>
		void LateUpdate()
		{
            if (localPlayer == null) return;
			if(localPlayer.transform.position.x < lowerLeftCorner.position.x)
            {
                float newPositionX = transform.position.x - (lowerLeftCorner.position.x - localPlayer.transform.position.x);
                transform.position = new Vector3(newPositionX, transform.position.y, transform.position.z);
                //transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x - (Mathf.Abs(lowerLeftCorner.position.x) - Mathf.Abs(player.transform.position.x)),transform.position.y,transform.position.z),1f);
            } else if(localPlayer.transform.position.x > upperRightPosition.position.x)
            {
                float newPositionX = transform.position.x + (localPlayer.transform.position.x - upperRightPosition.position.x);
                transform.position = new Vector3(newPositionX, transform.position.y, transform.position.z);
            }

            if (localPlayer.transform.position.y < lowerLeftCorner.position.y)
            {
                float newPositionY = transform.position.y - (lowerLeftCorner.position.y - localPlayer.transform.position.y);
                transform.position = new Vector3(transform.position.x, newPositionY, transform.position.z);
            }
            else if (localPlayer.transform.position.y > upperRightPosition.position.y)
            {
                float newPositionY = transform.position.y + (localPlayer.transform.position.y - upperRightPosition.position.y);
                transform.position = new Vector3( transform.position.x, newPositionY, transform.position.z);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(upperRightPosition.position, lowerLeftCorner.position);
        }

        #endregion


    }

}

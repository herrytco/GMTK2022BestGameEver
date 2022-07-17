using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public void JumpToPiece(PieceController controller)
    {
        Vector3 pos = transform.position;
        Vector3 posNew = controller.transform.position;

        transform.position = new Vector3(
            posNew.x, posNew.y, pos.z
        );
    }
}
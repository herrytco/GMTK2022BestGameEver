using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public void JumpToPiece(PieceController controller)
    {
        transform.position = controller.transform.position;
    }
}
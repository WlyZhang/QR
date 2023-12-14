using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private async void Start()
    {
        string path = $"{Application.streamingAssetsPath}/QR.png";

        await QR.Decode(path);

        QR.Encode("Hello Unity", 200, 200);
    }
}

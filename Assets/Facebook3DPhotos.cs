#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

public class Facebook3DPhotos {

    [MenuItem("3D Photos/Take Screenshot")]
    public static void TakeScreenshot() {

        var cam = Camera.main;

        var width = cam.pixelWidth;
        var height = cam.pixelHeight;

        var shader = Shader.Find("Hidden/DepthDumpShader");
        var mat = new Material(shader);

        var rt = RenderTexture.GetTemporary(width, height, 32);
        var rtDepth = RenderTexture.GetTemporary(width, height, 32);
        var oldTxd = cam.targetTexture;

        cam.targetTexture = rt;
        cam.depthTextureMode |= DepthTextureMode.Depth;
        cam.Render();
        cam.targetTexture = oldTxd;

        Graphics.Blit(rt, rtDepth, mat);

        var txd2d = new Texture2D(width, height, TextureFormat.ARGB32, false, false);

        RenderTexture.active = rt;
        txd2d.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
        txd2d.Apply();
        var encodedColor = txd2d.EncodeToPNG();

        RenderTexture.active = rtDepth;
        txd2d.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
        txd2d.Apply();
        var encodedDepth = txd2d.EncodeToPNG();

        File.WriteAllBytes("screenshot.png", encodedColor);
        File.WriteAllBytes("screenshot_depth.png", encodedDepth);

        RenderTexture.ReleaseTemporary(rt);
        RenderTexture.ReleaseTemporary(rtDepth);

        Object.DestroyImmediate(mat);
        Object.DestroyImmediate(txd2d);

    }

}
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using ZXing;
using ZXing.QrCode;

public class QR
{
	/// <summary>
	/// QR序列化
	/// </summary>
	/// <param name="data"></param>
	/// <param name="width"></param>
	/// <param name="height"></param>
	/// <returns></returns>
	public static Texture2D Encode(string data, int width, int height, bool isSave = true)
    {
		Texture2D encoded = new Texture2D(width, height, TextureFormat.RGBA32, false);
		var colors = GetColor32(data, width, height);
		encoded.SetPixels32(colors);
		encoded.Apply();

		if(isSave)
        {
			DateTime now = DateTime.UtcNow; // 获取当前 UTC 时间
			long timestamp = (long)(now - new DateTime(1970, 1, 1)).TotalSeconds; // 计算从 1970-01-01 至今的秒数

			string path = $"{Application.dataPath}/../QRImage/";
			string name = $"CX-GameX-{timestamp}.png";
			byte[] bytes = encoded.EncodeToPNG();

			ByteToFile(path, name, bytes, bytes.Length);
        }

		return encoded;
	}

	
	/// <summary>
	/// 获取像素数组
	/// </summary>
	/// <param name="textForEncoding"></param>
	/// <param name="width"></param>
	/// <param name="height"></param>
	/// <returns></returns>
	private static Color32[] GetColor32(string textForEncoding, int width, int height)
	{
		BarcodeWriter writer = new BarcodeWriter
		{
			Format = BarcodeFormat.QR_CODE,
			Options = new QrCodeEncodingOptions
			{
				Height = height,
				Width = width,
				Margin = 2,
				PureBarcode = true
			}
		};
		return writer.Write(textForEncoding);
	}


	/// <summary>
	/// QR反序列化
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static async Task<string> Decode(string path)
	{
		Texture2D texture = await LoadQR(path);

		BarcodeReader mReader = new BarcodeReader();
		Color32[] colors = texture.GetPixels32();
		var result = mReader.Decode(colors, texture.width, texture.height);
		if (result != null)
		{
			Debug.Log(result.Text);

			return result.Text;
		}
		return null;
	}

	/// <summary>
	/// 加载QR
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	private static async Task<Texture2D> LoadQR(string path)
    {
		WWW www = new WWW(path);

		while(!www.isDone)
        {
			await Task.Yield();
        }

		Texture2D texture = www.texture;

		return texture;
    }

	/// <summary>
	/// 保存文件
	/// </summary>
	/// <param name="path"></param>
	/// <param name="info"></param>
	/// <param name="length"></param>
	private static void ByteToFile(string path, string name, byte[] info, int length)
	{
		if(!Directory.Exists(path))
        {
			Directory.CreateDirectory(path);
        }

		//文件流信息  
		//StreamWriter sw;  
		Stream sw;
		FileInfo t = new FileInfo(path+name);
		t.Delete();
		sw = t.Create();
		//以行的形式写入信息  
		//sw.WriteLine(info);  
		sw.Write(info, 0, length);
		//关闭流  
		sw.Close();
		//销毁流  
		sw.Dispose();
	}
}

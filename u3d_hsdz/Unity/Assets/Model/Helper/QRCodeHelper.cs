using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.Common;

namespace ETModel
{
    public static class QRCodeHelper
    {
        public static Texture2D GenerateQRcode(string content, int width = 256, int height = 256)
        {
            MultiFormatWriter writer = new MultiFormatWriter();
            Dictionary<EncodeHintType, object> hints = new Dictionary<EncodeHintType, object>();
            hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
            hints.Add(EncodeHintType.MARGIN, 1);
            hints.Add(EncodeHintType.ERROR_CORRECTION, ZXing.QrCode.Internal.ErrorCorrectionLevel.M);
            BitMatrix bitMatrix = writer.encode(content, BarcodeFormat.QR_CODE, width, height, hints);

            int w = bitMatrix.Width;
            int h = bitMatrix.Height;
            Texture2D tex2D = new Texture2D(w, h);
            for (int x = 0; x < h; x++)
            {
                for (int y = 0; y < w; y++)
                {
                    if (bitMatrix[x, y])
                    {
                        tex2D.SetPixel(y, x, Color.black);
                    }
                    else
                    {
                        tex2D.SetPixel(y, x, Color.white);
                    }
                }
            }

            tex2D.Apply();
            return tex2D;
        }

        public static Sprite GetQRCodeSprite(string content, int width = 256, int height = 256)
        {
            var t2d = GenerateQRcode(content, width, height);
            return Sprite.Create(t2d, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
        }
    }
}

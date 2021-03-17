using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Drawing;

namespace FaceDetect
{
    internal class EmguCvCapture
    {
        VideoCapture _capture;
        CascadeClassifier _cascadeClassifier;
        const int BLURING_COEFFICIENT = 31; // mod 2 == 1

        internal EmguCvCapture()
        {
            _capture = new VideoCapture(0);
            _cascadeClassifier = new CascadeClassifier(@"haarcascades\haarcascade_frontalface_default.xml");
        }

        internal void Start()
        {
            _capture.Start();
        }

        internal Image CapturePicture(bool faceIsBluring)
        {
            Image result = null;
            if (_capture.IsOpened)
            {
                using (var frame = new Mat())
                {
                    try
                    {
                        _capture.Read(frame);
                        _capture.Grab();
                        var srcImage = frame.ToImage<Bgr, Byte>();

                        if (faceIsBluring)
                            BlurFace(srcImage);

                        result = srcImage.ToBitmap();
                        srcImage.Dispose();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return result;
        }

        private void BlurFace(Image<Bgr, byte> srcImage)
        {
            var faces = DetecFaces(srcImage);
            foreach (var faceRect in faces)
            {
                BlurArea(srcImage, faceRect);
            }

        }

        private static void BlurArea(Image<Bgr, byte> srcImage, Rectangle bluringArea)
        {
            srcImage.ROI = bluringArea;
            CvInvoke.GaussianBlur(srcImage, srcImage, new Size(BLURING_COEFFICIENT, BLURING_COEFFICIENT), 0);
            srcImage.ROI = Rectangle.Empty;
        }

        private Rectangle[] DetecFaces(Image<Bgr, byte> srcImage)
        {
            Rectangle[] faces;
            using (var ugray = new UMat())
            {
                CvInvoke.CvtColor(srcImage, ugray, ColorConversion.Bgr2Gray);
                CvInvoke.EqualizeHist(ugray, ugray);
                faces = _cascadeClassifier.DetectMultiScale(
                    ugray, 1.1, 10, new Size(20, 20));
            }

            return faces;
        }

        internal void Stop()
        {
            _capture.Stop();
            _cascadeClassifier.Dispose();
        }
    }
}

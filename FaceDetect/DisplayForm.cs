using System;
using System.Drawing;
using System.Windows.Forms;

namespace FaceDetect
{
    public partial class DisplayForm : Form
    {
        EmguCvCapture _videoCapture;
        public DisplayForm()
        {
            InitializeComponent();
            _videoCapture = new EmguCvCapture();
        }

        private void CaptureCameraCallback(object sender, EventArgs e)
        {
            pictureBox.Image?.Dispose();
            if (pictureBox.Size.Width > 0 && pictureBox.Size.Height > 0)
            {
                pictureBox.Image = new Bitmap(pictureBox.Size.Width, pictureBox.Size.Height);
                var srcImage = _videoCapture.CapturePicture(blurCheckBox.Checked);
                if (srcImage != null)
                {
                    ImageScaleHelper.ScaleAndDraw(srcImage, pictureBox.Image);
                    srcImage.Dispose();
                }
                pictureBox.Refresh();
            }
        }

        private void DisplayForm_Load(object sender, EventArgs e)
        {
            _videoCapture.Start();
        }

        private void DisplayForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer.Stop();
            _videoCapture.Stop();
        }
    }
}

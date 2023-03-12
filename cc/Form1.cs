using cc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace cc
{
    public partial class MainClass : Form
    {
        //These values are all used in both major classes
        static Image precip_img;
        public static string precip_type;
        static bool is_displayed;
        static int intensity_value;
        static int skew_strength;
        static int s_d;
        //0 = none 
        //1 = left 
        //2 = right
        static int skew_direction;
        public static bool precalc_complete;
        //int form_width = this.Width;
        static Image post_resize;

        //same for height
        //static double new_width = form_width / 40;
        //round down to int
        static int new_width_v2 = Convert.ToInt32(new_width);
        static PictureBox pb;
        static List<PictureBox> picture_boxes = new List<PictureBox>();
        static int img_count;

        static int horizontal_space;
        static int y;
        static int x;

        static int screen_width;
        //Use 1080p as a base value, for only 1 pixel added 
        readonly static double added_pix = this.Height / 1080;

        readonly static int added_pix_int = Convert.ToByte(added_pix);

        static int x_interval;

        static Random rnd_interval = new Random();
        static Random rnd_start = new Random();

        static string potential_change;
        //is there a way to set the form opacity to 0, but keep the picture boxes on their own layer?
        public MainClass()
        {
            InitializeComponent();
            //default value
            //PrecipSelect.Text = "Rain";
            //RightLeft.Text = "None";
 

        }
        //start here with the start button
        public void ImageSetup(string pt)
        {
            List<PictureBox> picture_boxes = this.Controls.OfType<PictureBox>().ToList();

            foreach (PictureBox pb in picture_boxes)
            {
                this.Controls.Remove(pb);
            }
            LoadImage(pt + "nobg.png");

        }
        private void LoadImage(string path)
        {
            string set_path = Path.Combine(Application.StartupPath, path);
            precip_img = Image.FromFile(set_path);
            ResizeBitmap();

        }

        //call whenever  save or start is clicked
        public void UpdateValues()
        {

            precalc_complete = false;

            if (skew_direction == 1)
            {   //negative could be what's causing the div by 0 error
                s_d = -skew_strength;
            }
            else if (skew_direction == 2)
            {
                s_d = skew_strength;
            }
            //Reset the image being used

            ImageSetup(precip_type);
            this.Close();
        }

        //I think this would work if the settings form saved its settings properly, visually
        private void CheckImg()
        {

            if (precip_type != potential_change)
            {
                ImageSetup(precip_type);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            UpdateValues();
            //Reset the image being used
            //only go the image operation again if it changed
            ImageSetup(precip_type);
            this.Close();
        }


        private static void ResizeImage(Image imgToResize, Size size)
        {
            Bitmap bmp = new Bitmap(imgToResize, size);
            Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
            post_resize = bmp;

        }
        private static void ResizeBitmap()
        {
            Bitmap imgbitmap = new Bitmap(precip_img);
            ResizeImage(imgbitmap, new Size(new_width_v2, new_width_v2));
            
            AddBoxes();
        }


        //Unless it's been reset, this does not need to be fully recalculated every time 
        //need to store as much data as possible and have the animations class reuse it
        static readonly Animations an = new Animations();
        //scan this for div 0 errors
        protected void AddBoxes()
        {

            for (int i = 0; i < img_count; i++)
            {
                x = (new_width_v2 + horizontal_space) * i + horizontal_space;
                pb = new PictureBox
                {
                    Image = post_resize,
                    Size = new Size(new_width_v2, new_width_v2),
                    Location = new Point(x, this.Height / y)
                };
                picture_boxes.Add(pb);
                this.Controls.Add(pb);
            }
            if (precalc_complete)
            {
                an.AniLoop();
            }
            else
            {
                an.Precalculations();
            }
        }

        internal class Animations : Form
        {
            protected List<PictureBox> temp_list;
            protected Rectangle hit_detection;
            protected bool hit_edge;
            protected readonly Random rn_index = new Random();
            //snow rotation speed
            protected static readonly int rotationDelay = 150;
            protected DateTime nextRotation = DateTime.Now.AddMilliseconds(rotationDelay);
            //static ensures it is not modified and prevents range error
            protected static List<int> precalculations_list = new List<int>();

            /*Precalcs to prevent lag, like a game loading screen. 
            This only runs when settings are changed or the program is restarted. 
            Otherwise, it jumps straight to Animate()*/
            public void Precalculations()
            {   
      
                //you are here ^
                img_count = intensity_value * 5;

                horizontal_space = (form_width - (img_count * new_width_v2)) / (img_count + 1);
                y = rnd_start.Next(10, 16);
                //skew: 0, 1, 2 = None, Left, Right
                //maybe remove the randomization and make the intervals set
                int low_end = this.Height / 1000;
                int high_end = this.Height / 600;
                //clear to prevent endless list expansion
                precalculations_list.Clear();

                screen_width = this.Width;

                //direction calculations 
                if (is_displayed)
                {   //x axis skew
                    int[] x_intervals_array = { 0, 800, 720, 640, 560, 480, 400, 320, 240, 160, 80 };
                    //where the actual fuck is it getting a div by 0?
                    x_interval = screen_width / x_intervals_array[skew_strength];
                    Console.WriteLine("x interval:" + x_interval);

                }
                //y axis
                for (int i = 0; i < img_count; i++)
                {

                    double movement_interval = rnd_interval.Next(low_end, high_end);
                    int movement_interval_v2 = Convert.ToUInt16(movement_interval);
                    precalculations_list.Add(movement_interval_v2);

                }
                precalc_complete = true;
    
                AniLoop();
            }
            //it's never called before this
            public void AniLoop()
            {
                System.Windows.Forms.Timer loop = new System.Windows.Forms.Timer
                {
                    Interval = 16
                };
                loop.Tick += (object sender, EventArgs e) =>
                {
                    Animate(precip_type);
                };
                loop.Start();
            }
            //shared between all precip types, with rotation added for snow
            //need to incorporate multithreading for tiers 2 and 3, and anything with the snow 
            public void Animate(string pt)
            {   //make sure this scales to image size

                bool rotate = (DateTime.Now >= nextRotation);
                rn_index.Next(0, precalculations_list.Count - 1);
                temp_list = picture_boxes.ToList();
                hit_detection = this.ClientRectangle;
                hit_edge = false;


                foreach (PictureBox pb in temp_list)
                {
                    //Use the values from the list above, -1 to account for 0 indexing
                    int list_index = rn_index.Next(0, precalculations_list.Count - 1);
                    pb.Top += added_pix_int + precalculations_list[list_index];
                    pb.Left += s_d;
                    if (pt != "Rain" && rotate)
                    {
                        Image img = pb.Image;
                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        pb.Image = img;

                    }
                    //doesn't work with super low image sizes for some reason, but should be fine
                    pb.MouseEnter += (sender, e) =>
                    {
                        this.Controls.Remove(pb);
                        picture_boxes.Remove(pb);
                    };
                    //when one touches the edge of the screen, clear the list/boxes 
                    if (pb.Left < hit_detection.Left ||
                        pb.Right > hit_detection.Right ||
                        pb.Bottom > hit_detection.Bottom)
                    {
                        this.Controls.Remove(pb);
                        picture_boxes.Remove(pb);
                        hit_edge = true;
                    }

                }
                if (rotate)
                {
                    nextRotation = DateTime.Now.AddMilliseconds(rotationDelay);
                }
                //cannot modify collection, so happens afterward

                if (hit_edge && picture_boxes.Count < img_count - (img_count / 2)
                    || picture_boxes.Count < img_count - (img_count / 2))
                {
                    temp_list.Clear();
                    AddBoxes();

                }
            }

        }; //animations class ends here

        /*private void RightLeft_SelectedIndexChanged(object sender, EventArgs e)
        {
            int r_i = RightLeft.SelectedIndex;
            //Show the skew strength selection if a direction is chosen
            bool yesno = r_i != 0 ? is_displayed = true : is_displayed = false;
            DisplayOnOff(yesno);
            skew_direction = r_i;
        }


        private void SkewInt_Update(object sender, EventArgs e)
        {
            SkewInt.Text = SkewStrengthBar.Value.ToString();
            skew_strength = SkewStrengthBar.Value;
        }*/
        //keep settings saved...not sure how to do this
        //maybe just set default vals
    }
};
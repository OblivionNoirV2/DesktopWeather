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
        static Image current_img;
        
        static int stage_counter;

        public static bool precalc_complete;

        static Image post_resize;
        //this is set on load
        static int form_width;
        static int form_height;
        //same for height
        static double new_width = form_width / 40;
        //round down to int
        static int new_width_v2 = Convert.ToInt32(new_width);
        static PictureBox pb;
        static List<PictureBox> picture_boxes = new List<PictureBox>();

        //the current set of picture boxes being used for the stage
        static List<string> current_set = new List<string>() { 
            //stage one values
            "one", "one", "one", "one", "one"
        };
        static int horizontal_space;
        static int y;
        static int x;
        public static int shield_count;
        static int screen_width;
        //Use 1080p as a base value, for only 1 pixel added 
        readonly static double added_pix = form_height / 1080;

        readonly static int added_pix_int = Convert.ToByte(added_pix);


        static Random rnd_interval = new Random();
        static Random rnd_start = new Random();

        //Things that change per stage
        static int img_speed;
        static int img_count;
        //number represents clicks required. Powerful = 8, X is a powerful that changes images and requires an x press
        static int one_rate;
        static int two_rate;
        static int three_rate;
        static int four_rate;
        static int powerful_rate;
        static int x_rate;
        public MainClass()
        {
            InitializeComponent();
            //default value
            //PrecipSelect.Text = "Rain";
            //RightLeft.Text = "None";
 

        }
        //start here with the start button
        private void StartButton_Click(object sender, EventArgs e)
        {
            stage_counter = 1;
            ImageSetup("one", 1);
        }
        //This runs every 20 seconds, on a background thread

        public class DifficultyScaling
        {
            public void RaiseDifficulty(int current_stage)
            {
                //if it's at max capacity, don't do an addition, just a swap
                bool is_maxed = current_set.Count == 20 ? true : false;



                //Check if all the values in the list are identical
                if (!current_set[0].Equals(current_set[1]))
                {
                    //if they are not, don't update the image yet

                }
                else
                {
                    //If they are, switch to the next stronger attack

                }
                //Then check to see if it's reached the max count...


            }
        };
      
        public void ImageSetup(string file_name, int stage)
        {
            List<PictureBox> picture_boxes = this.Controls.OfType<PictureBox>().ToList();
            //clear any existing images
            foreach (PictureBox pb in picture_boxes)
            {
                this.Controls.Remove(pb);
            }

            string set_path = Path.Combine(Application.StartupPath, file_name + ".png");
            current_img = Image.FromFile(set_path);
            Console.WriteLine(set_path);
            ResizeBitmap();
        }
        //loads an array of the file paths being used 
        public void LoadImages(string path, int stage)
        {

            string set_path = Path.Combine(Application.StartupPath, path);
            current_img = Image.FromFile(set_path);
            ResizeBitmap();

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
            Bitmap imgbitmap = new Bitmap(current_img);
            ResizeImage(imgbitmap, new Size(new_width_v2, new_width_v2));

            MainClass mc = new MainClass();
            
            mc.AddBoxes();
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
                    Location = new Point(x, form_height / y)
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


            MainClass mc2 = new MainClass();
            /*Precalcs to prevent lag, like a game loading screen. 
            This only runs when settings are changed or the program is restarted. 
            Otherwise, it jumps straight to Animate()*/

            public void Precalculations()
            {   
      
                //This can be infinite, but maybe bring out multithreading for high values
                img_count = stage_counter + 2;

                horizontal_space = (form_width - (img_count * new_width_v2)) / (img_count + 1);
                y = rnd_start.Next(10, 16);
                //skew: 0, 1, 2 = None, Left, Right
                int low_end = this.Height / 1000;
                int high_end = this.Height / 600;
                //clear to prevent endless list expansion
                precalculations_list.Clear();
                //in case the window was moved
                screen_width = this.Width;

                for (int i = 0; i < img_count; i++)
                {

                    double movement_interval = rnd_interval.Next(low_end, high_end);
                    int movement_interval_v2 = Convert.ToUInt16(movement_interval);
                    precalculations_list.Add(movement_interval_v2);

                }
                precalc_complete = true;
    
                AniLoop();
            }
            //gets slightly faster each stage
            public void AniLoop()
            {
                System.Windows.Forms.Timer loop = new System.Windows.Forms.Timer
                {
                    Interval = 16
                };
                loop.Tick += (object sender, EventArgs e) =>
                {
                    Animate();
                };
                loop.Start();
            }
            public void Animate()
            {   //make sure this scales to image size

      
                rn_index.Next(0, precalculations_list.Count - 1);
                temp_list = picture_boxes.ToList();
                hit_detection = this.ClientRectangle;
                hit_edge = false;


                foreach (PictureBox pb in temp_list)
                {
                    //Use the values from the list above, -1 to account for 0 indexing
                    int list_index = rn_index.Next(0, precalculations_list.Count - 1);
                    pb.Top += added_pix_int + precalculations_list[list_index];
                 
                  

                    pb.MouseClick += (sender, e) =>
                    {
                        this.Controls.Remove(pb);
                        picture_boxes.Remove(pb);
                    };

                }
                //cannot modify collection, so happens afterward

                if (hit_edge)
                {
                    this.Controls.Remove(pb);
                    picture_boxes.Remove(pb);
                    if (shield_count < 1)
                    {
                        YouLose();

                    } else
                    {
                        UpdateShieldCount(false);
                    }
                }
                //change this to a minute timer for each stage
                if (hit_edge && picture_boxes.Count < img_count - (img_count / 2)
                    || picture_boxes.Count < img_count - (img_count / 2))
                {
                    temp_list.Clear();
              
                    mc2.AddBoxes();

                }
            }

        }; //animations class ends here

        static public void YouLose()
        {

        }

        static public void YouWin()
        {

        }

        static public void UpdateStageCount()
        {
            //update the label
        }
        //true if we're adding to the count
        static public void UpdateShieldCount(bool pos)
        {
            if (pos)
            {
                shield_count++;
            } else
            {
                shield_count--;
            }
        }
        private void MainClass_Load(object sender, EventArgs e)
        {
            form_width = this.Width;
            form_height = this.Height;
        }

    





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
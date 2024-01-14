using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace PhotoEncoding
{
    public class PixelStepper
    {
        private Bitmap bm;
        private int OnX;
        private int OnY;
        private int OnRGB; //0 = on R, 1 = on G, 2 = on B


        public PixelStepper(Stream s)
        {
            bm = new Bitmap(s);
            OnX = 0;
            OnY = 0;
            OnRGB = 0;
        }

        //Returns next RGB value. If at the end of file (no more pixels left, will return null)
        public byte? NextValue()
        {
            
            //If they are negative, that is my trick of saying they have reached the end of the file.
            if (OnX == -1 && OnY == -1)
            {
                return null;
            }

            //Get what we will return
            byte ToReturn = 0;
            Color pixel = bm.GetPixel(OnX, OnY);
            
            //Return appropriate value
            if (OnRGB == 0)
            {
                ToReturn = pixel.R;
            }
            else if (OnRGB == 1)
            {
                ToReturn = pixel.G;
            }
            else if (OnRGB == 2)
            {
                ToReturn = pixel.B;
            }

            //Increment accordingly
            if (OnRGB == 2) //We've exhausted all 3 of this pixel's values (R, G, and B. Next pixel needed)
            {
                if (OnX >= (bm.Width - 1)) //We are on the last pixe in this row. Time to go to a new row
                {
                    OnX = 0; //Move X back to 0
                    OnY = OnY + 1; //Go down to next row
                }
                else //We are NOT on the last pixel in this row. Move to the next pixel in this row.
                {
                    OnX = OnX + 1; //Only move forward 1 pixel
                }
                OnRGB = 0; //Reset the RGB value we are on.
            }
            else //We still have other RGB values to use in this pixel. Increment to the next one.
            {
                OnRGB = OnRGB + 1;
            }
            

            //If we are over, that is it for next time
            if (OnY >= bm.Height)
            {
                OnX = -1;
                OnY = -1;
            }

            return ToReturn;
        }

        public void Reset()
        {
            OnX = 0;
            OnY = 0;
            OnRGB = 0;
        }
    }
}
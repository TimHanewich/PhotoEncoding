using System;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace PhotoEncoding
{
    public class PhotoEncoder
    {
        private string path; //The file that we will be encoding to an image

        public PhotoEncoder(string file_input_path)
        {
            path = file_input_path;
        }

        //Determines the width and height and returns as an int (width will always be same as height - a perfect square)
        public int DeterminePhotoDimensions()
        {
            //Determine what the dimensions are
            FileInfo fi = new FileInfo(path);
            long FileBytes = fi.Length;
            byte[] FileNameBytes = Encoding.UTF8.GetBytes(fi.Name);
            int TotalBytes = Convert.ToInt32(FileNameBytes.Length + Toolkit.BreakerSignature.Length + FileBytes + Toolkit.TerminatorSignature.Length); //File Name bytes, FileNameConent breaker signature bytes, file content bytes, terminator signature bytes. In that order.
            int dimensions = Convert.ToInt32(Math.Ceiling(Math.Sqrt(Convert.ToSingle(TotalBytes) / 3f)));
            return dimensions;
        }

        public void Encode(string output_path)
        {           
            
            //Get the file name and convert it to bytes
            FileInfo fi = new FileInfo(path);
            byte[] FileNameBytes = Encoding.UTF8.GetBytes(fi.Name);

            //Get the dimensions
            int dimensions = DeterminePhotoDimensions();

            //Load everything into a ByteStepper
            ByteStepper bs = new ByteStepper();
            bs.FileNameBytes = FileNameBytes;
            bs.BreakerSignature = Toolkit.BreakerSignature;
            Stream input_stream = System.IO.File.OpenRead(path);
            bs.FileContent = input_stream;
            bs.TerminatorSignature = Toolkit.TerminatorSignature;
        
            //Create the new bitmap
            Bitmap bm = new Bitmap(dimensions, dimensions);
            Random r = new Random();
            for (int y = 0; y < bm.Height; y++)
            {
                for (int x = 0; x < bm.Width; x++)
                {

                    //Get Next up in line
                    byte? nR = bs.NextByte();
                    byte? nG = bs.NextByte();
                    byte? nB = bs.NextByte();

                    //plug in to real RGB values we will write.
                    byte R = 0;
                    byte G = 0;
                    byte B = 0;
                    if (nR != null)
                    {
                        R = nR.Value;
                    }
                    else
                    {
                        R = Convert.ToByte((r.Next(0, 256))); //Plug in random R
                    }
                    if (nG != null)
                    {
                        G = nG.Value;
                    }
                    else
                    {
                        G = Convert.ToByte((r.Next(0, 256))); //Plug in random G
                    }
                    if (nB != null)
                    {
                        B = nB.Value;
                    }
                    else
                    {
                        B = Convert.ToByte((r.Next(0, 256))); //Plug in random B
                    }

                    //Set pixel
                    bm.SetPixel(x, y, Color.FromArgb(R, G, B));
                }
            }

            //Save it
            bm.Save(output_path);

            //Close the input file stream
            input_stream.Close();
        }


    }
}
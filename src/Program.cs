using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.Encodings;
using System.Text;

namespace PhotoEncoder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Encode(@"C:\Users\timh\Downloads\PhotoEncoder\test.txt", @"C:\Users\timh\Downloads\PhotoEncoder\pic_enc.png");
            //PhotoDecoder pd = new PhotoDecoder(System.IO.File.OpenRead(@"C:\Users\timh\Downloads\PhotoEncoder\pic_enc.png"));
            //Stream s = System.IO.File.OpenWrite(@"C:\Users\timh\Downloads\PhotoEncoder\decoded.txt");
            //pd.Decode(s);
            //s.Close();

            PhotoEncoder pe = new PhotoEncoder(@"C:\Users\timh\Downloads\PhotoEncoder\test.txt");
            Console.WriteLine(pe.DeterminePhotoDimensions());
        }

        public static void create()
        {
            Bitmap bm = new Bitmap(16, 16);
            int red = 255;
            for (int y = 0; y < bm.Height; y++)
            {
                for (int x = 0; x < bm.Width; x++)
                {
                    bm.SetPixel(x, y, Color.FromArgb(red, 0, 0));
                    red = red - 1;
                }
            }
            bm.Save(@"C:\Users\timh\Downloads\PhotoEncoder\pic.png");
        }

        public static void read()
        {
            Bitmap bm = new Bitmap(@"C:\Users\timh\Downloads\PhotoEncoder\pic.png");
            for (int y = 0; y < bm.Height; y++)
            {
                for (int x = 0; x < bm.Width; x++)
                {
                    Color pixel = bm.GetPixel(x, y);
                    Console.WriteLine(pixel.R.ToString() + ", " + pixel.G.ToString() + ", " + pixel.B.ToString());
                }
            }

        }

        public static void Encode(string input_path, string output_path)
        {

            string path = input_path;
            FileInfo fi = new FileInfo(path);
            Console.WriteLine("File is " + fi.Length.ToString("#,##0") + " bytes");
            
            //Determine what the dimensions are
            long FileBytes = fi.Length;
            byte[] FileNameBytes = Encoding.UTF8.GetBytes(fi.Name);
            int TotalBytes = Convert.ToInt32(FileBytes + FileNameBytes.Length + Toolkit.BreakerSignature.Length + Toolkit.TerminatorSignature.Length); //File Name bytes, FileNameConent breaker signature bytes, file content bytes, terminator signature bytes. In that order.
            Console.WriteLine("Total bytes, with everything, is " + TotalBytes.ToString());
            int dimensions = Convert.ToInt32(Math.Ceiling(Math.Sqrt(Convert.ToSingle(TotalBytes) / 3f)));
            Console.WriteLine("Dimensons should be " + dimensions.ToString() + " x " + dimensions.ToString());

            //Load everything into a ByteSpitter
            ByteSpitter bs = new ByteSpitter();
            bs.FileNameBytes = FileNameBytes;
            bs.BreakerSignature = Toolkit.BreakerSignature;
            bs.FileContent = System.IO.File.OpenRead(path);
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

            
        }



    }
}
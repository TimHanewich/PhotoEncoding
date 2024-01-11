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
    }
}
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.Encodings;
using System.Text;

namespace PhotoEncoding
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //PhotoEncoder pe = new PhotoEncoder(@"C:\Users\timh\Downloads\History of Space Exploration.docx");
            //pe.Encode(@"C:\Users\timh\Downloads\encoded.png");

            string EncodedFilePath = @"C:\Users\timh\Downloads\encoded.png";
            string OutputFolderPath = @"C:\Users\timh\Downloads\DECODE HERE";

            PhotoDecoder pd = new PhotoDecoder(System.IO.File.OpenRead(EncodedFilePath));
            Console.WriteLine(pd.DecodeFileName()); //History of Space Exploration.docx
            string OutputFilePath = Path.Combine(OutputFolderPath, pd.DecodeFileName());
            Stream OutputTo = System.IO.File.OpenWrite(OutputFilePath);
            pd.Decode(OutputTo);
            OutputTo.Close();

            Console.WriteLine("Your encoded picture has been decoded to '" + OutputFilePath + "'");
            //Your encoded picture has been decoded to 'C:\Users\timh\Downloads\DECODE HERE\History of Space Exploration.docx'
            

            
        }
    }
}
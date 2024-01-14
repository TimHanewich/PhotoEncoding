using PhotoEncoding;
using System;

namespace PhotoEncoding
{
    public class ByteStepper
    {
        public byte[] FileNameBytes {get; set;}
        public byte[] BreakerSignature {get; set;} //splits the name bytes and the file content bytes in the image
        public Stream FileContent {get; set;}
        public byte[] TerminatorSignature {get; set;}

        //Private variables
        private int OnSegment;//0 = FileNameBytes, 1 = BreakerSignature, 2 = FileContent, 3 = TerminatorSignature
        private int OnIndex;//For the byte arrays that are index-based (everyting besides FileContent), tracks what index we are on now.

        public ByteStepper()
        {
            FileNameBytes = new byte[]{};
            BreakerSignature = new byte[]{};
            FileContent = Stream.Null;
            TerminatorSignature = new byte[]{};

            OnSegment = 0;
            OnIndex = 0;
        }

        public byte? NextByte()
        {
            byte? ToReturn = null;

            if (OnSegment == 0)
            {
                if (OnIndex >= FileNameBytes.Length) //we are over
                {
                    OnSegment = 1;
                    OnIndex = 0;
                    ToReturn = NextByte();
                }
                else
                {
                    ToReturn = FileNameBytes[OnIndex];
                    OnIndex = OnIndex + 1;
                }
            }
            else if (OnSegment == 1)
            {
                if (OnIndex >= BreakerSignature.Length) //we are over
                {
                    OnSegment = 2;
                    OnIndex = 0;
                    ToReturn = NextByte();
                }
                else
                {
                    ToReturn = BreakerSignature[OnIndex];
                    OnIndex = OnIndex + 1;
                }
            }
            else if (OnSegment == 2)
            {
                int nb = FileContent.ReadByte();
                if (nb == -1) //We've reached the end of the file content
                {
                    OnSegment = 3;
                    OnIndex = 0;
                    ToReturn = NextByte();
                }
                else
                {
                    ToReturn = Convert.ToByte(nb);
                }
            }
            else if (OnSegment == 3)
            {
                if (OnIndex >= TerminatorSignature.Length)
                {
                    ToReturn = null;
                }
                else
                {
                    ToReturn = TerminatorSignature[OnIndex];
                    OnIndex = OnIndex + 1;
                }
            }

            return ToReturn;
        }
    }
}
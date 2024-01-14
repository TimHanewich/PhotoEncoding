using System;

namespace PhotoEncoding
{
    public class Toolkit
    {
        public static byte[] BreakerSignature
        {
            get
            {
                return new byte[]{123, 45, 210, 17, 88, 92, 200, 33, 5, 76, 32, 199, 100, 23, 42, 99};
            }
        }

        public static byte[] TerminatorSignature
        {
            get
            {
                return new byte[]{205, 64, 31, 77, 92, 12, 244, 189, 88, 209, 77, 29, 38, 78, 162, 15};
            }
        }
    }
}
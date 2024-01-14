using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Drawing;

namespace PhotoEncoding
{
    public class PhotoDecoder
    {
        private PixelStepper ps;

        public PhotoDecoder(Stream s)
        {
            ps = new PixelStepper(s);
        }

        private int FindSequence(byte[] sequence)
        {
            //Setup
            ps.Reset(); //Go back to the beginning

            //Set up vars for tracking
            int MatchedBytesCount = 0; //a count of the # of bytes that have been matched to the current sequence. Once it is the full length, we found it
            int StartOfMatchedSequenceIndex = 0; //Where the beginning of this matched index started (will be returned as the answer)
            int OnIndex = 0; //The index (byte) we are on in the file's content
            while (true)
            {
                //Get next byte
                byte? nb = ps.NextValue();
                if (nb == null)
                {
                    throw new Exception("Unable to find breaker signature in the entire file content!");
                }

                //Is it a match?
                if (Convert.ToByte(nb) == sequence[MatchedBytesCount])
                {
                    if (MatchedBytesCount == 0)
                    {
                        StartOfMatchedSequenceIndex = OnIndex;
                    }
                    MatchedBytesCount = MatchedBytesCount + 1;
                }
                else
                {
                    MatchedBytesCount = 0; //necessary
                    StartOfMatchedSequenceIndex = 0; //Not necessary, but doing this anyway.
                }

                //If the # of matched bytes EQUALS the breaker length, we found it! Return it!
                if (MatchedBytesCount == sequence.Length)
                {
                    return StartOfMatchedSequenceIndex;
                }

                //Increment on index
                OnIndex = OnIndex + 1;
            }
        }
        
        //Finds the start index (location) of the breaker signature (what separates the encoded file name values from the file content)
        private int FindBreakerSignature()
        {
            return FindSequence(Toolkit.BreakerSignature);            
        }

        //Finds the start index (location) of the terminator signature that signifies the end of the file content in the RGB pixel values.
        private int FindTerminatorSignature()
        {
            return FindSequence(Toolkit.TerminatorSignature);
        }
        
        
        public string DecodeFileName()
        {
            int BreakerStart = FindBreakerSignature();
            ps.Reset();
            
            //Collect all the bytes leading up to that 
            List<byte> NameBytes = new List<byte>();
            for (int t = 0; t < BreakerStart; t++)
            {
                byte? nb = ps.NextValue();
                if (nb != null)
                {
                    NameBytes.Add(nb.Value);
                }
                else
                {
                    throw new Exception("A byte before the breaker signature was null. Broken!");
                }
            }

            //Convert
            string name = System.Text.Encoding.UTF8.GetString(NameBytes.ToArray());
            return name;
        }
    
        public void Decode(Stream output)
        {
            output.Seek(0, SeekOrigin.Begin); //Reset
            
            //Find signature locations
            int BreakerSignatureLocation = FindBreakerSignature();
            int TerminatorSignature = FindTerminatorSignature();

            //"Fast forward" to the location of where the file bytes start
            ps.Reset();
            for (int t = 0; t < (BreakerSignatureLocation + Toolkit.BreakerSignature.Length); t++)
            {
                ps.NextValue();
            }

            //Dump to file
            for (int t = 0; t < (TerminatorSignature - BreakerSignatureLocation - Toolkit.BreakerSignature.Length); t++)
            {
                byte? nb = ps.NextValue();
                if (nb == null)
                {
                    break;
                }
                else
                {
                    output.WriteByte(nb.Value);
                }
            }
        }  
    
    }
}
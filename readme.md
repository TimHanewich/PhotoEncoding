# File to Photo Encoding
This C# (.NET) project is capable of encoding any file's binary content into a bitmap image, and then decoding the encoded image back to the original file:

![encoding decoding process](https://i.imgur.com/mqCWuYr.png)

Both the original file name (including extension) and the binary content of the original file is encoded into the RGB values of the pixels of the resulting encoded file. 

As an example, the below image is the fully encoded [**December 2023 Microsoft Power Platform Licensing Guide**](https://go.microsoft.com/fwlink/?linkid=2085130), ~965 KB.
![example](https://i.imgur.com/ct7EiTS.png)

## Why did I make this?
Image hosting services like [Imgur](https://imgur.com/) allow users, both authenticated and anonymous, to store full uncompressed bitmap images for free. By converting files to bitmap images using this program, we essentially gain the ability to now store any *file* on Imgur as well:

1. Convert a file to an encoded image.
2. Upload the encoded image to Imgur.
3. Download the image from Imgur.
4. Use this program to decode the image back to the original file state.

The steps above fully describe the process of both uploading and retrieving a file content from any image hosting site, but Imgur being used specifically here. 

This project is experimental in nature - the steps above can, obviously, be cumbersome for a large number of files. Perhaps some of those steps can be programmatically automated as well. This project was an experiment to see if this idea of mine could be accomplished, which it has proven to be.

## How to use - Encoding
You can use the `PhotoEncoder` class to encode a file to a bitmap image (PNG and JPG are tested and proven to work well):
```
PhotoEncoder pe = new PhotoEncoder(@"C:\Users\timh\Downloads\History of Space Exploration.docx");
pe.Encode(@"C:\Users\timh\Downloads\encoded.png");
```
The resulting `encoded.png` file will look something like this:  
![encoded example](https://i.imgur.com/GEeM0Tu.png)

The encoded image contains both the encoded name of the original file (including its extension) and the file's binary content itself.

## How to use - Decoding
To decode the encoded image *back* into its original state, using the `PhotoDecoder` class:
```
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
```

You'll see above that the `PhotoDecoder` class has a function for finding, extracting, and decoding the original file name and extension. Optionally, you can use this to determine the file's original name and then decode the image into a file with the same name, as shown above. Or, at the very least, you can use this to read the extension of the file, thus understanding what file type this is and how to open it.
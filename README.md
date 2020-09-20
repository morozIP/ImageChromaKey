# ImageChromaKey
The project uses two solutions to remove the green background (you can change the color by changing the filter in the code).
In accordance with the incoming criterion, I focused only on the principle of image processing, and not on beautiful design.<br>
  •	User-entered values are used for filtering values of Green in Argb<br>
  •	220 used as standard parameter<br>

1.	Solution (on TabPage1) based on the using of methods System.Drawing.Color GetPixel and SetPixel.
    Use loop for each pixel from Image by GetPixel, checking green color value and replace pixel by SetPixel.<br>
      •	Slow speed solution<br>
      •	Little code field<br>
2.	Solution (on TabPage2) based on the using of static unsafe method.
    * Source Bitmap is fixing/blocking in the memory (how I understood) by method LockBits;
    * BitmapData iterating through byte array; 
    * Every pixel read (through a set of bytes) by Argb;
    * Checking the green color value;
    * If necessary, changing values of a set of bytes of the pixel in Argb;
    * All iteration filled a new Bitmap(target) by BitmapData;
    * In finally source’s and target’s Bitmap using UnlockBits for next processes with image.<br>
      •	Very fast solution<br>
      •	Incoming image formats are treated equally<br>
      •	Unsafe<br>
      •	Bigger code field<br>


Outstanding Issues:<br>
  •	The first solution works correctly only with PNG format for saving transparency, JPEG does not support transparency, replaced pixels with Transparent changing to white, and in saving source input JPEG in PNG background remains white. Did not solve this problem because is work in TabPage2.

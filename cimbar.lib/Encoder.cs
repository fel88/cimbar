namespace cimbar.lib
{

    public class Encoder : SimpleEncoder
    {

        public uint encode(string filename, string output_prefix)
        {
            var f = new FileStream(filename, FileMode.Open, FileAccess.Read);

            uint i = 0;
            while (true)
            {
                var frame = encode_next(f);
                if (frame != null)
                    break;

                string output = $"{output_prefix}_{i}.png";
                // imwrite expects BGR
                OpenCvSharp.Cv2.CvtColor(frame, frame, OpenCvSharp.ColorConversionCodes.RGB2BGR);
                OpenCvSharp.Cv2.ImWrite(output, frame);
                ++i;
            }
            return i;
        }

        
    }
}
using OpenCvSharp;
using System;
using System.IO;
using System.Numerics;

namespace cimbar.lib
{
    internal class Common
    {
        public static Mat getAnchor(bool dark)
        {
            string name = dark ? "anchor-dark" : "anchor-light";
            return Common.load_img($"{name}");
        }

        public static Mat getSecondaryAnchor(bool dark)
        {
            string name = dark ? "anchor-secondary-dark" : "anchor-secondary-light";
            return Common.load_img($"{name}");
        }

        public static Mat getHorizontalGuide(bool dark)
        {
            string name = dark ? "guide-horizontal-dark" : "guide-horizontal-light";
            return Common.load_img($"{name}");
        }

        public static Mat getVerticalGuide(bool dark)
        {
            string name = dark ? "guide-vertical-dark" : "guide-vertical-light";
            return Common.load_img($"{name}");
        }

        internal static Mat load_img(string v)
        {
            // auto it = cimbar::bitmaps.find(path);
            // if (it == cimbar::bitmaps.end())
            //     return null;

            //string bytes = base91::decode(it->second);
            //  vector < unsigned char> data(bytes.data(), bytes.data() + bytes.size());

            int width, height, channels;
            //std::unique_ptr < uint8_t[], void(*)(void *) > imgdata(stbi_load_from_memory(data.data(), static_cast<int>(data.size()), &width, &height, &channels, STBI_rgb_alpha), ::free);
            var r = ResourceHelper.ReadResourceBytes(v);
            var mat = Cv2.ImDecode(r, ImreadModes.Unchanged);
            //if (!imgdata)
            //   return new Mat();

            //int len = width * height * channels;
            //Mat mat=new Mat (height, width,  CV_MAKETYPE(CV_8U, channels));
            //std::copy(imgdata.get(), imgdata.get() + len, mat.data);
            Cv2.CvtColor(mat, mat, ColorConversionCodes.RGBA2RGB);
            return mat;

        }
        public struct RGB
        {
            public RGB(byte _r, byte _g, byte _b)
            {
                r = _r;
                g = _g;
                b = _b;
            }

            public byte r;
            public byte g;
            public byte b;
        }


        static RGB getColor4_old(int index)
        {
            RGB[] colors = {
               new RGB(0, 0xFF, 0xFF),
            new RGB(0xFF, 0xFF, 0),
          new   RGB(0xFF, 0, 0xFF),
         new   RGB(0, 0xFF, 0),
        };
            return colors[index];
        }

        static RGB getColor8_old(int index)
        {
            RGB[] colors = {
                new RGB(0, 0xFF, 0xFF), // cyan
			new RGB(0x7F, 0x7F, 0xFF),  // mid-blue
		new RGB(0xFF, 0, 0xFF), // magenta
		new RGB(0xFF, 65, 65), // red
		new RGB(0xFF, 0x9F, 0),  // orange
		new RGB(0xFF, 0xFF, 0), // yellow
		new RGB(0xFF, 0xFF, 0xFF),
        new RGB(0, 0xFF, 0),
        };
            return colors[index];
        }
        static RGB getColor4(int index)
        {
            // opencv uses BGR, but we don't have to conform to its tyranny
            RGB[] colors = {
                new RGB(0, 0xFF, 0),
            new RGB(0, 0xFF, 0xFF),
            new RGB(0xFF, 0xFF, 0),
            new RGB(0xFF, 0, 0xFF),
        };
            return colors[index];
        }

        static RGB getColor(int index, int num_colors, int color_mode)
        {
            if (color_mode == 0)
            {
                if (num_colors <= 4)
                    return getColor4_old(index);
                else
                    return getColor8_old(index);
            }

            if (num_colors <= 4)
                return getColor4(index);
            else
                return getColor8(index);
        }
        static RGB getColor8(int index)
        {
            RGB[] colors = {
              new  RGB(0, 0xFF, 0xFF), // cyan
		  new   RGB(0xFF, 0xFF, 0), // yellow
			  new RGB(0x7F, 0x7F, 0xFF),  // mid-blue
			  new RGB(0xFF, 0xFF, 0xFF), // white
			  new RGB(0, 0xFF, 0), // green
			  new RGB(0xFF, 0x9F, 0),  // orange
			  new RGB(0xFF, 0, 0xFF), // magenta
			  new RGB(0xFF, 65, 65), // red
		};
            return colors[index];
        }

        internal static Mat getTile(int symbol_bits, int symbol, bool dark = true, int num_colors = 4, int color = 0,
            int color_mode = 1)
        {
            Vec3b background = new Vec3b(0xFF, 0xFF, 0xFF);
            string imgPath = $"{symbol_bits}.{symbol:X2}";
            Mat tile = load_img(imgPath);

            byte r, g, b;
            var rgb = getColor(color, num_colors, color_mode);
            r = rgb.r;
            g = rgb.g;
            b = rgb.b;
            for (int i = 0; i < tile.Cols; i++)
                for (int j = 0; j < tile.Rows; j++)
                {
                    var c = tile.At<Vec3b>(i, j);
                    if (c == background)
                    {
                        if (dark)
                            c = new Vec3b(0, 0, 0);
                        tile.Set(i, j, c);
                        continue;
                    }
                    c = new Vec3b(r, g, b);
                     tile.Set(i, j, c);
                }

            /*cv::MatIterator_<cv::Vec3b> end = tile.end<cv::Vec3b>();
            for (cv::MatIterator_<cv::Vec3b> it = tile.begin<cv::Vec3b>(); it != end; ++it)
            {
                cv::Vec3b & c = *it;
                if (c == background)
                {
                    if (dark)
                        c = { 0, 0, 0};
                    continue;
                }
                c = { r, g, b};
            }*/
            return tile;


        }
    }
}
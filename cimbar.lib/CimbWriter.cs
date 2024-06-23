using OpenCvSharp;

namespace cimbar.lib
{
    internal class CimbWriter
    {
        public CimbWriter(int bitsPerSymbol, int bitsPerColor, bool dark, int colorMode, int canvas_size)
        {
        }
        Mat _image;
        CellPositions _positions;
        CimbEncoder _encoder;
        int _offset = 0;

        internal Mat image()
        {
            throw new NotImplementedException();
        }

        internal int num_cells()
        {
            throw new NotImplementedException();
        }
        void paste(Mat img, int x, int y)
        {
            var rect = new Rect(x + _offset, y + _offset, img.Cols, img.Rows);
            using Mat temp1 = new Mat(_image, rect);
            img.CopyTo(temp1);
        }

        internal bool write(int bits)
        {
            // check with _encoder for tile, then place it in template according to mapping
            // mapping will track current index/location?
            if (done())
                return false;

            CellPositions.coordinate xy = _positions.next();
            Mat cell = _encoder.encode(bits);
            paste(cell, xy.first, xy.second);
            return true;


        }

        private void paste(Mat cell, object first, object second)
        {
            throw new NotImplementedException();
        }

        private bool done()
        {
            return _positions.done();

        }
    }
}
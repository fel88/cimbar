using OpenCvSharp;

namespace cimbar.lib
{
    internal class CimbWriter
    {
        public CimbWriter(int symbol_bits, int color_bits, bool dark, int color_mode, int size)
        {
            _positions = new CellPositions(Config.cell_spacing(),
                        Config.cells_per_col(), Config.cell_offset(), Config.corner_padding(), Config.interleave_blocks(), Config.interleave_partitions());
            _encoder = new CimbEncoder(symbol_bits, color_bits, dark, color_mode);
            if (size > Config.image_size())
                _offset = (size - Config.image_size()) / 2;
            else
                size = Config.image_size();

            Scalar bgcolor = dark ? new Scalar(0, 0, 0) : new Scalar(0xFF, 0xFF, 0xFF);
            _image = new Mat(size, size, MatType.CV_8UC3, bgcolor);

            // from here on, we only care about the internal size
            size = Config.image_size();

            Mat anchor = Common.getAnchor(dark);
            paste(anchor, 0, 0);
            paste(anchor, 0, size - anchor.Cols);
            paste(anchor, size - anchor.Rows, 0);

            Mat secondaryAnchor = Common.getSecondaryAnchor(dark);
            paste(secondaryAnchor, size - anchor.Rows, size - anchor.Cols);

            Mat hg = Common.getHorizontalGuide(dark);
            paste(hg, (size / 2) - (hg.Cols / 2), 2);
            paste(hg, (size / 2) - (hg.Cols / 2), size - 4);
            paste(hg, (size / 2) - (hg.Cols / 2) - hg.Cols, size - 4);
            paste(hg, (size / 2) - (hg.Cols / 2) + hg.Cols, size - 4);

            Mat vg = Common.getVerticalGuide(dark);
            paste(vg, 2, (size / 2) - (vg.Rows / 2));
            paste(vg, size - 4, (size / 2) - (vg.Rows / 2));

        }

        Mat _image;
        CellPositions _positions;
        CimbEncoder _encoder;
        int _offset = 0;

        internal Mat image()
        {
            return _image;

        }

        internal int num_cells()
        {
            return _positions.count();

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

        

        private bool done()
        {
            return _positions.done();

        }
    }
}
using OpenCvSharp;

namespace cimbar.lib
{
    internal class CimbEncoder
    {
        public CimbEncoder(int symbol_bits, int color_bits, bool dark, int color_mode)
        {
            _numSymbols = (1 << symbol_bits);
            _numColors = (1 << color_bits);
            _dark = (dark);
            _colorMode = (color_mode);

            load_tiles(symbol_bits);

        }
        List<Mat> _tiles = new List<Mat>();
        int _numSymbols;
        int _numColors;
        bool _dark;
        int _colorMode;
        Mat load_tile(int symbol_bits, int index)
        {
            int symbol = index % _numSymbols;
            int color = index / _numSymbols;
            return Common.getTile(symbol_bits, symbol, _dark, _numColors, color, _colorMode);
        }


        // dir will need to be passed via env? Doesn't make sense to compile it in, and doesn't *really* make sense to use cwd
        private bool load_tiles(int symbol_bits)
        {
            int numTiles = _numColors * _numSymbols;
            for (int i = 0; i < numTiles; ++i)
                _tiles.Add(load_tile(symbol_bits, i));
            return true;
        }

        internal Mat encode(int bits)
        {
            bits = bits % _tiles.Count;
            return _tiles[bits];

        }
    }



}
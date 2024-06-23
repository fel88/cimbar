using OpenCvSharp;
using System.IO;

namespace cimbar.lib
{
    public class SimpleEncoder
    {
        bool _coupled;
        int _eccBytes;
        int _eccBlockSize;
        int _bitsPerSymbol;
        int _bitsPerColor;
        bool _dark;

        public SimpleEncoder(int ecc_bytes = -1, int bits_per_symbol = 0, int bits_per_color = -1)
        {
            _eccBytes = (ecc_bytes >= 0 ? ecc_bytes : Config.ecc_bytes());
            _eccBlockSize = (Config.ecc_block_size());
            _bitsPerSymbol = (bits_per_symbol != 0 ? bits_per_symbol : Config.symbol_bits());
            _bitsPerColor = (bits_per_color >= 0 ? bits_per_color : Config.color_bits());
            _dark = (Config.dark());
            _coupled = (false);
            _colorMode = (Config.color_mode());
        }


        int _colorMode;
        byte _encodeId = 0;
        public Mat encode_next(Stream stream, int canvas_size = 0)
        {
            if (_coupled)
                return encode_next_coupled(stream, canvas_size);

            if (!stream.CanRead)
                return null;

            int bits_per_op = _bitsPerColor + _bitsPerSymbol;
            CimbWriter writer = new CimbWriter(_bitsPerSymbol, _bitsPerColor, _dark, _colorMode, canvas_size);

            int numCells = writer.num_cells();
            bitbuffer bb = new bitbuffer(Config.capacity(bits_per_op));

            int bitPos = 0;
            int endBitPos = numCells * bits_per_op;

            int progress = 0;
            int bitsPerRead = _bitsPerSymbol;
            int bitsPerWrite = bits_per_op;

            reed_solomon_stream rss = new reed_solomon_stream(stream, _eccBytes, _eccBlockSize);
            bitreader br = new bitreader();
            while (rss.good() && progress < 2)  // 1 symbol pass + 1 color pass
            {
                int bytes = rss.readsome();
                if (bytes == 0)
                    break;
                br.assign_new_buffer(rss.buffer(), bytes);

                // reorder. We're encoding the symbol bits and striping them across the whole image
                // then encoding the color bits and striping them in the same way (filling in the gaps)
                while (!br.empty())
                {
                    int bits = br.read(bitsPerRead);
                    if (!br.partial())
                        bb.write(bits, bitPos, bitsPerWrite);
                    bitPos += bits_per_op;

                    if (bitPos >= endBitPos)
                    {
                        // switch to color section
                        bitsPerRead = _bitsPerColor;
                        bitsPerWrite = _bitsPerColor;
                        bitPos = 0;
                        ++progress;
                        break;
                    }
                }
            }

            // dump whatever we have to image
            for (bitPos = 0; bitPos < endBitPos; bitPos += bits_per_op)
            {
                int bits = bb.read(bitPos, bits_per_op);
                writer.write(bits);
            }

            // return what we've got
            return writer.image();
        }

        private Mat encode_next_coupled(Stream stream, int canvas_size)
        {
            throw new NotImplementedException();
        }
    }
}
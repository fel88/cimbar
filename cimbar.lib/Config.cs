
using cimbar.lib;

using GridConf = cimbar.lib.Conf8x8;
namespace cimbar.lib
{
    internal static class Config
    {
        public static int capacity(int bitspercell)
        {
            if (bitspercell == 0)
                bitspercell = bits_per_cell();
            return total_cells() * bitspercell / 8;
        }
        public static int color_mode()
        {
            return 1; // unless we override per-thread?
        }

        public static int ecc_bytes()
        {
            return GridConf.ecc_bytes;
        }

        public static int ecc_block_size()
        {
            return GridConf.ecc_block_size;
        }

        public  static int cells_per_col()
        {
            return GridConf.cells_per_col;
        }
        public static int image_size()
        {
            return GridConf.image_size;
        }

        public static int corner_padding()
        {
            return lrint(54.0 / cell_spacing());

        }
        static int cell_size()
        {
            return GridConf.cell_size;
        }
        public static int bits_per_cell()
        {
            return color_bits() + symbol_bits();
        }

        public static int color_bits()
        {
            return GridConf.color_bits;
        }

        public static int symbol_bits()
        {
            return GridConf.symbol_bits;
        }

        public static int cell_spacing()
        {
            return cell_size() + 1;
        }

        private static int lrint(double value)
        {
            return (int)Math.Floor(value);
        }

        static int total_cells()
        {
            return (int)(Math.Pow(cells_per_col(), 2) - Math.Pow(corner_padding(), 2) * 4);
        }

        public static bool dark()
        {
            return true;
        }

        internal static int cell_offset()
        {
            return GridConf.cell_offset;

        }

        internal static int interleave_blocks()
        {
            return ecc_block_size();
        }

        internal static int interleave_partitions()
        {
            return 2;

        }
    }

}
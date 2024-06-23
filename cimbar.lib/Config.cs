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
        static int cells_per_col()
        {
            return GridConf.cells_per_col;
        }
        static int corner_padding()
        {
            return lrint(54.0 / cell_spacing());

        }
        static int cell_size()
        {
            return GridConf.cell_size;
        }
        static int bits_per_cell()
        {
            return color_bits() + symbol_bits();
        }

        static int color_bits()
        {
            return GridConf.color_bits;
        }

        static int symbol_bits()
        {
            return GridConf.symbol_bits;
        }

        static int cell_spacing()
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


    }

}
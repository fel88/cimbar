namespace cimbar.lib
{
    struct Conf5x5
    {
        public static int color_bits = 2;
        public static int symbol_bits = 2;
        public static int ecc_bytes = 40;
        public static int ecc_block_size = 216;
        public static int image_size = 988;

        public static int cell_size = 5;
        public static int cell_offset = 9;
        public static int cells_per_col = 162;
    }

    struct Conf8x8
    {
        public static int color_bits = 2;
        public static int symbol_bits = 4;
        public static int ecc_bytes = 30;
        public static int ecc_block_size = 155;
        public static int image_size = 1024;

        public static int cell_size = 8;
        public static int cell_offset = 8;
        public static int cells_per_col = 112;
    }
}
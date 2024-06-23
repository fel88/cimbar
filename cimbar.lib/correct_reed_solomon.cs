namespace cimbar.lib
{
    public class correct_reed_solomon
    {
        public int block_length;
        public int message_length;
        public int min_distance; 
        public polynomial_t encoded_polynomial;
        public polynomial_t encoded_remainder;

        public field_t field;
        public polynomial_t generator;

    }
}
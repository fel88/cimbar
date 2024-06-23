namespace cimbar.lib
{
    public class CellPositions
    {
        internal bool done()
        {
            return _index >= _positions.Count;
        }

        internal coordinate next()
        {
            return _positions[_index++];
        }

        positions_list compute_linear(int spacing, int dimensions, int offset, int marker_size)
        {
            /*ex: if dimensions == 128, and marker_size == 8:
		8 tiles at top is 128 - 16 == 112

        8 tiles at bottom is also 128 - 16 == 112


        structure would be:
            112 * 8

        128 * 112

        112 * 8
    */
            positions_list res = new positions_list();
            int offset_y = offset;
            int marker_offset_x = spacing * marker_size;
            int top_width = dimensions - marker_size - marker_size;
            int top_cells = top_width * marker_size;
            for (int i = 0; i < top_cells; ++i)
            {
                int x = (i % top_width) * spacing + marker_offset_x + offset;
                int y = (i / top_width) * spacing + offset_y;
                 res.Add ( new coordinate ( x, y));
            }

            int mid_y = marker_size * spacing;
            int mid_width = dimensions;
            int mid_cells = mid_width * top_width;  // top_width is also "mid_height"
            for (int i = 0; i < mid_cells; ++i)
            {
                int x = (i % mid_width) * spacing + offset;
                int y = (i / mid_width) * spacing + mid_y + offset_y;
                res.Add(new coordinate(x, y));
            }

            int bottom_y = (dimensions - marker_size) * spacing;
            int bottom_width = top_width;
            int bottom_cells = bottom_width * marker_size;
            for (int i = 0; i < bottom_cells; ++i)
            {
                int x = (i % bottom_width) * spacing + marker_offset_x + offset;
                int y = (i / bottom_width) * spacing + bottom_y + offset_y;
                res.Add(new coordinate(x, y)); 
            }
            return res;
        }


        positions_list compute(int spacing, int dimensions, int offset, int marker_size,
            int interleave_blocks = 0, int interleave_partitions = 1)
        {
            positions_list pos = compute_linear(spacing, dimensions, offset, marker_size);
            if (interleave_blocks != 0)
                return Interleave.interleave(pos, interleave_blocks, interleave_partitions);
            return pos;
        }

        public CellPositions(int spacing, int dimensions, int offset, int marker_size, int interleave_blocks, int interleave_partitions)
        {
            _positions = compute(spacing, dimensions, offset, marker_size, interleave_blocks, interleave_partitions);
            reset();
        }

        internal int count()
        {
            return _positions.Count;
        }
        void reset()
        {
            _index = 0;
        }

        int _index;
        positions_list _positions = new positions_list();
        public class positions_list : List<coordinate>
        {

        }
        public class coordinate
        {
            public coordinate()
            {

            }
            public coordinate(int x,int y)
            {
                first = x;
                second = y;
            }
            public int first;
            public int second;
        }
        // using coordinate = std::pair<int, int>;
        //using positions_list = std::vector<coordinate>;


    }
}
namespace cimbar.lib
{
    internal class CellPositions
    {
        internal bool done()
        {
            throw new NotImplementedException();
        }

        internal coordinate next()
        {
            throw new NotImplementedException();
        }

        int _index;
        positions_list _positions;
        public class positions_list : List<coordinate>
        {

        }
        public class coordinate 
        {
            public int first;
            public int second;
        }
        // using coordinate = std::pair<int, int>;
        //using positions_list = std::vector<coordinate>;

        
    }
}
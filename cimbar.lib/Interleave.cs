using System.Collections.Concurrent;
using static cimbar.lib.CellPositions;

namespace cimbar.lib
{
    internal class Interleave
    {
        internal static CellPositions.positions_list interleave(CellPositions.positions_list positions,
            int num_chunks, int partitions)
        {
            positions_list res = new positions_list();

            List<int> indices = interleave_indices(positions.Count, num_chunks, partitions);
            for (int i = 0; i < indices.Count; ++i)
            {
                int interleaveIdx = indices[i];
                res.Add(positions[interleaveIdx]);
            }
            return res;

        }

        private static List<int> interleave_indices(int size, int num_chunks, int partitions)
        {
            List<int> indices = new List<int>();
            if (num_chunks == 0)
            {
                for (int i = 0; i < size; ++i)
                    indices.Add(i);
                return indices;
            }

            int partitionSize = size / partitions;
            for (int part = 0; part < size; part += partitionSize)
                for (int chunk = 0; chunk < num_chunks; ++chunk)
                    for (int i = chunk; i < partitionSize; i += num_chunks)
                        indices.Add(i + part);

            return indices;
        }

    }
}
namespace SimulatorClient.Extensions
{
    public static class WellExtensions
    {
        public static ushort[] ToColRow(this string wellName)
        {
            var colNames = new[] { "A", "B","C","D","E","F","G","H" };
            var col = wellName.Substring(0,1);
            ushort colIndex = 0;
            foreach (var colName in colNames)
            {
                if (colName == col)
                    break;
                colIndex++;
            }
        
            if (ushort.TryParse(wellName.Substring(1), out var row))
                row -= 1;
        
            return new[] { colIndex, row };
        }
    }
}
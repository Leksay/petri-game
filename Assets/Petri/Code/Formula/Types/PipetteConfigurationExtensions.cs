using System;

namespace Petri.Formula
{
    public static class PipetteCellConfigurationExtensions
    {
        public static (int x, int y) GetSize(this PipetteSizeConfiguration configuration)
        {
            switch (configuration)
            {
                case PipetteSizeConfiguration.Small_2x8:
                    return (2, 8);
                case PipetteSizeConfiguration.Medium_3x8:
                    return (3, 8);
                case PipetteSizeConfiguration.Big_4x8:
                    return (4, 8);
                case PipetteSizeConfiguration.Large_5x8:
                    return (5, 8);
                default:
                    throw new ArgumentOutOfRangeException(nameof(configuration), configuration, null);
            }
        }
    }
}
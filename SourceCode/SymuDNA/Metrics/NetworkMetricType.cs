#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

namespace Symu.DNA.Metrics
{
    /// <summary>
    /// List of available Network level metrics Types
    /// </summary>
    public enum NetworkMetricType
    {
        #region Quantity
        RowCount,
        ColumnCount,
        /// <summary>
        /// Normalized row entries count
        /// </summary>
        RowDegreeCentrality,
        /// <summary>
        /// Normalized column entries count
        /// </summary>
        ColumnDegreeCentrality,
        /// <summary>
        /// It's a network level concept that identifies the average amount of, e.g. knowledge, per agent
        /// </summary>
        Load,
        /// <summary>
        /// The ratio of the number of links versus the maximum possible links for a network
        /// Number of possible relation that reflects the level of social  organizational cohesion.
        /// </summary>
        Density,
        #endregion

        #region Variance
        /// <summary>
        /// The centrality degree can be interpreted in terms of the ability of a node to pick up anything that passes nearby on the network (such as a virus, or information)
        /// Calculated at the network level
        /// </summary>
        DegreeCentralization,
        /// <summary>
        /// The centrality degree can be interpreted in terms of the ability of a node to pick up anything that passes nearby on the network (such as a virus, or information)
        /// Calculated by row
        /// </summary>
        RowDegreeCentralization,
        /// <summary>
        /// The centrality degree can be interpreted in terms of the ability of a node to pick up anything that passes nearby on the network (such as a virus, or information)
        /// Calculated by column
        /// </summary>
        ColumnDegreeCentralization
        #endregion
    }

}
#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

namespace Symu.DNA.GraphNetworks.TwoModesNetworks
{
    ///// <summary>
    /////     Abstract class for two modes network
    ///// </summary>
    ///// <remarks>todo should be replaced by QuickGraph
    ///// TVertex = IAgentId
    ///// </remarks>
    //public abstract class TwoModesNetwork_obsolete< TNode> where TNode : class
    //{

    //    /// <summary>
    //    ///     List of all entities Ids and their node
    //    /// </summary>
    //    public readonly Dictionary<IAgentId, List<TNode>> List =
    //        new Dictionary<IAgentId, List<TNode>>();
    //    public int Count => List.Count;


    //    public bool Any()
    //    {
    //        return List.Any();
    //    }

    //    public void Clear()
    //    {
    //        List.Clear();
    //    }
    //    /// <summary>
    //    /// Check that the resourceId exists in the repository
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <returns></returns>
    //    public bool Exists(IAgentId key)
    //    {
    //        return List.ContainsKey(key);
    //    }

    //    public bool Exists(IAgentId key, TNode node)
    //    {
    //        return Exists(key) && List[key].Contains(node);
    //    }

    //    public void Add(IAgentId key, TNode node)
    //    {
    //        AddKey(key);
    //        AddNode(key, node);
    //    }

    //    /// <summary>
    //    ///     Add a node to a key
    //    ///     Key is supposed to be already present in the collection.
    //    ///     if not use Add method
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <param name="node"></param>
    //    public void AddNode(IAgentId key, TNode node)
    //    {
    //        if (node == null)
    //        {
    //            throw new ArgumentNullException(nameof(node));
    //        }
    //        if (!Exists(key))
    //        {
    //            throw new ArgumentNullException(nameof(key));
    //        }

    //        if (!List[key].Contains(node))
    //        {
    //            List[key].Add(node);
    //        }
    //    }

    //    public void AddKey(IAgentId key)
    //    {
    //        if (!Exists(key))
    //        {
    //            List.Add(key, new List<TNode>());
    //        }
    //    }

    //    public IEnumerable<TNode> GetNodes(IAgentId key)
    //    {
    //        return Exists(key) ? (IEnumerable<TNode>) List[key] : new TNode[0];
    //    }
    //    /// <summary>
    //    ///     Get nodes count of a key
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <returns></returns>
    //    public byte GetNodesCount(IAgentId key)
    //    {
    //        return Exists(key) ? Convert.ToByte(List[key].Count) : (byte)0;
    //    }
    //    /// <summary>
    //    ///     Make a copy of of the network
    //    /// </summary>
    //    /// <param name="network"></param>
    //    public void CopyTo(TwoModesNetwork<TNode> network)
    //    {
    //        if (network is null)
    //        {
    //            throw new ArgumentNullException(nameof(network));
    //        }

    //        foreach (var keyValuePair in List)
    //        foreach (var node in keyValuePair.Value)
    //        {
    //            network.Add(keyValuePair.Key, node);
    //        }
    //    }
    //    public void Remove(IAgentId key)
    //    {
    //        if (Exists(key))
    //        {
    //            List.Remove(key);
    //        }
    //    }
    //}
}
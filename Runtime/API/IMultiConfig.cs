namespace Unibrics.Configuration.General
{
    using System.Collections.Generic;

    public interface IMultiConfig<TConfig>
    {
        TConfig GetBuyId(string id);

        IEnumerable<TConfig> GetAll();

        IEnumerable<(string key, TConfig value)> GetAllWithKeys();
    }
}
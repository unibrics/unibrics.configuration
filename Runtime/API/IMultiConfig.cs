namespace Unibrics.Configuration.General
{
    using System.Collections.Generic;

    public interface IMultiConfig<out TConfig>
    {
        TConfig GetBuyId(string id);

        IEnumerable<TConfig> GetAll();
    }
}
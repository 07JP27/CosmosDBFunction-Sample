using Newtonsoft.Json;

namespace CosmosDBFunction.Core.Entities.Base
{
    public abstract class EntityBase
    {
        [JsonProperty(PropertyName = "id")]
        public virtual string Id { get; set; }
    }
}

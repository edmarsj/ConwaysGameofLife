using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace ConwaysGameofLife.API.Models
{
    public class ResponseModel
    {
        [JsonProperty(Order = 0)]
        public string RequestId { get; private set; }
        [JsonProperty(Order = 1)]
        public string ErrorCode { get; set; }
        public ResponseModel()
        {
            // Usually we would use a correlation id
            RequestId = Guid.NewGuid().ToString();
        }
    }

    public class ResponseModel<T> : ResponseModel
    {
        [JsonProperty(Order = 99)]
        public T Result { get; }

        public ResponseModel(T result) : base()
        {
            Result = result;
        }
    }

    public class ValidationResponseModel : ResponseModel<Dictionary<string, string>>
    {
        public ValidationResponseModel(ModelStateDictionary modelState) : base(new Dictionary<string, string>())
        {
            foreach (var key in modelState.Keys)
            {
                Result.Add(key, string.Join(',', modelState[key].Errors.Select(e => e.ErrorMessage)));
            }
        }

        public ValidationResponseModel(Dictionary<string, string> errors) : base(errors)
        {

        }
    }
}

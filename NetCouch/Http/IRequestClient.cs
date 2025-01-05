namespace NetCouch.Http;

public interface IRequestClient
{
    Task<ResponseData<TOut>> GetAsync<TOut>(string path);
    ResponseData<TOut> Get<TOut>(string path);
    Task<ResponseData<TOut>> HeadAsync<TOut>(string path);
    ResponseData<TOut> Head<TOut>(string path);
    Task<ResponseData<TOut>> OptionsAsync<TOut>(string path);
    ResponseData<TOut> Options<TOut>(string path);
    Task<ResponseData<TOut>> DeleteAsync<TOut>(string path);
    ResponseData<TOut> Delete<TOut>(string path);
    ResponseData<TOut> Put<TIn, TOut>(RequestData<TIn>? requestData);
    ResponseData<TOut> Post<TIn, TOut>(RequestData<TIn> requestData);
    Task<ResponseData<TOut>> SendAsync<TIn, TOut>(RequestData<TIn> requestData, HttpMethod method);
}
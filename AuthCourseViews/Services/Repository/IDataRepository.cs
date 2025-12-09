namespace AuthCourseViews.Services.Repository
{
	public interface IDataRepository
	{

		public string BaseAddress { get; }

		public Task<HttpResponseWrapper<TData>> GetAsync<TData>(string url);
		public Task<HttpResponseWrapper<TResult>> GetAsync<TResult, TBody>(TBody data, string url);
		public Task<HttpResponseWrapper<object>> PostAsync<T>(T data, string url);
		public Task<HttpResponseWrapper<TResponse>> PostAsync<TData, TResponse>(TData data, string url);
		public Task<HttpResponseWrapper<object>> PutAsync<T>(T data, string url);
		public Task<HttpResponseWrapper<TResponse>> PutAsync<TData, TResponse>(TData data, string url);
		public Task<HttpResponseWrapper<object>> PutAsync(string url);
		public Task<HttpResponseWrapper<object>> DeleteAsync(string url);

	}
}

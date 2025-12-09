using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace AuthCourseViews.Services.Repository
{
	public class DefaultRepositoryService : IDataRepository
	{

		public HttpClient Client { get; set; }

		public string BaseAddress { get => Client.BaseAddress!.AbsoluteUri; }

		public DefaultRepositoryService(HttpClient client)
		{
			Client = client;
		}

		public JsonSerializerOptions serializerOptions => new JsonSerializerOptions()
		{
			PropertyNameCaseInsensitive = true
		};

		public async Task<HttpResponseWrapper<TData>> GetAsync<TData>(string url)
		{

			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

			request.Headers.Add("Accept", "application/json");

			var response = await Client.SendAsync(request);

			var responseWrapper = new HttpResponseWrapper<TData> { StatusCode = response.StatusCode };

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
				responseWrapper.ResponseData = await DeserializeResponse<TData>(response);

			return responseWrapper;

		}

		public async Task<HttpResponseWrapper<TResult>> GetAsync<TResult, TBody>(TBody data, string url)
		{

			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

			request.Headers.Add("Accept", "application/json");
			request.Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

			var response = await Client.SendAsync(request);

			var responseWrapper = new HttpResponseWrapper<TResult> { StatusCode = response.StatusCode };

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
				responseWrapper.ResponseData = await DeserializeResponse<TResult>(response);

			return responseWrapper;

		}

		public async Task<HttpResponseWrapper<object>> PostAsync<T>(T data, string url)
		{

			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

			request.Headers.Add("Accept", "application/json");
			request.Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

			var response = await Client.SendAsync(request);

			var responseWrapper = new HttpResponseWrapper<object> { StatusCode = response.StatusCode };

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
				responseWrapper.ResponseData = await DeserializeResponse<object>(response);

			return responseWrapper;

		}


		public async Task<HttpResponseWrapper<TResponse>> PostAsync<TData, TResponse>(TData data, string url)
		{

			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

			request.Headers.Add("Accept", "application/json");
			request.Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

			var response = await Client.SendAsync(request);

			var responseWrapper = new HttpResponseWrapper<TResponse> { StatusCode = response.StatusCode };

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
				responseWrapper.ResponseData = await DeserializeResponse<TResponse>(response);

			return responseWrapper;

		}

		public async Task<HttpResponseWrapper<object>> PutAsync<T>(T data, string url)
		{

			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url);

			request.Headers.Add("Accept", "application/json");
			request.Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

			var response = await Client.SendAsync(request);

			var responseWrapper = new HttpResponseWrapper<object> { StatusCode = response.StatusCode };

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
				responseWrapper.ResponseData = await DeserializeResponse<object>(response);

			return responseWrapper;

		}

		public async Task<HttpResponseWrapper<TResponse>> PutAsync<TData, TResponse>(TData data, string url)
		{

			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url);

			request.Headers.Add("Accept", "application/json");
			request.Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

			var response = await Client.SendAsync(request);

			var responseWrapper = new HttpResponseWrapper<TResponse> { StatusCode = response.StatusCode };

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
				responseWrapper.ResponseData = await DeserializeResponse<TResponse>(response);

			return responseWrapper;

		}

		public async Task<HttpResponseWrapper<object>> PutAsync(string url)
		{

			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url);

			request.Headers.Add("Accept", "application/json");
			//request.Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

			var response = await Client.SendAsync(request);

			var responseWrapper = new HttpResponseWrapper<object> { StatusCode = response.StatusCode };

			//if (response.StatusCode == System.Net.HttpStatusCode.OK)
			//	responseWrapper.ResponseData = await DeserializeResponse<object>(response);

			return responseWrapper;

		}

		public async Task<HttpResponseWrapper<object>> DeleteAsync(string url)
		{

			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);

			request.Headers.Add("Accept", "application/json");

			var response = await Client.SendAsync(request);

			var responseWrapper = new HttpResponseWrapper<object> { StatusCode = response.StatusCode };

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
				responseWrapper.ResponseData = await DeserializeResponse<object>(response);

			return responseWrapper;

		}

		private async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
		{
			try
			{
				return await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync(), serializerOptions);
			}
			catch (Exception)
			{
				return default;
			}
		}

	}
}

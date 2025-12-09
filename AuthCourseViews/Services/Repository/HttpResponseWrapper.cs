using System.Net;

namespace AuthCourseViews.Services.Repository
{
	public class HttpResponseWrapper<T>
	{
		public T ResponseData { get; set; }
		public HttpStatusCode StatusCode { get; set; }
		public string ErrorMessage { get; set; }

	}
}

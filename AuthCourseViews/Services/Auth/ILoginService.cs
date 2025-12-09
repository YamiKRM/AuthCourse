namespace AuthCourseViews.Services.Auth
{
	public interface ILoginService
	{

		public Task LogIn(string token);

		public Task LogOut();

	}
}

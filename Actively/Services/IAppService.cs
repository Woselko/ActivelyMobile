using Actively.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actively.Services
{
    public interface IAppService
    {
        Task<bool> RefreshToken();
        public Task<Response> AuthenticateUser(LoginModel loginModel);
        Task<(bool IsSuccess, string Message)> RegisterUser(RegisterUser registerUser);
        Task<List<string>> GetSupportedLanguages();

        public Task<Response> ChangeLanguage(string language);
		//Task<List<StudentModel>> GetAllStudents();
	}
}

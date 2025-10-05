using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monki.DAL.Models
{
	public class ServiceResult
	{
		public bool Success { get; set; }
		public string Message { get; set; } = string.Empty;
		public object? Data { get; set; }
		public static ServiceResult SuccessResult(string message = "Operation completed successfully.", object? data = null)
		{
			return new ServiceResult { Success = true, Message = message, Data = data };
		}
		public static ServiceResult FailureResult(string message = "Operation failed.", object? data = null)
		{
			return new ServiceResult { Success = false, Message = message, Data = data };
		}
	}
}

namespace HospitalManagementSystem.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForError(StatusCode);
        }
        private string? GetDefaultMessageForError(int statusCode)
        {
            return statusCode switch
            {
                400 => "A Bad Request, You Have Made",
                401 => "Authorized, You Are Not",
                404 => "Resource Was Not Found",
                500 => "Errors Are The Path To The Dark Side. Errors Leads To Anger. Anger Leads To Hate. Hate Leads To Carear Change",
                _ => null
            };
        }
    }
}

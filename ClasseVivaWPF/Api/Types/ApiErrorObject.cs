namespace ClasseVivaWPF.Api.Types
{
    public record class ApiErrorObject(int StatusCode, string Error, string Info, string Message) : ApiObject;
}

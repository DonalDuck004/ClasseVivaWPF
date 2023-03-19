namespace ClasseVivaWPF.Api.Types
{
    public record class Note(object[] NTTE, 
                             object[] NTCL, 
                             object[] NTWN,
                             object[] NTST) : ApiObject;
}

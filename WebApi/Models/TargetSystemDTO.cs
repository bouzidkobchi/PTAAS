namespace WebApi.Models
{
    public class TargetSystemDTO
    {
        public required string Name { get; set; }
        public TargetSystemDTO() { }
        [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
        public TargetSystemDTO(TargetSystem system)
        {
            Name = system.Name;
        }
        public TargetSystem ToTargetSystem()
        {
            return new TargetSystem { Name = Name };
        }
    }

}

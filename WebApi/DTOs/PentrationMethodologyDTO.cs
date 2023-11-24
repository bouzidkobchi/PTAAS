using WebApi.Models;

namespace WebApi.DTOs
{
    public class PentestingMethodologyDTO : IDTO<PentestingMethodology>
    {
        public string Name { get; set; }
        public PentestingMethodologyDTO(PentestingMethodology method)
        {
            this.Name = method.Name;
        }
        public PentestingMethodology ToBase()
        {
            throw new NotImplementedException();
        }
    }
}

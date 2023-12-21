using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.Models;

namespace WebApi.DTOs
{
    public class MethodologyDTO
    {
        public required string Name { get; set; }
        public MethodologyDTO() { }
        [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
        public MethodologyDTO(TestingMethodology method)
        {
            Name = method.Name;
        }
        public TestingMethodology ToPentestingMethodology()
        {
            return new TestingMethodology { Name = Name };
        }
    }

    //public record PentestingMethodologyDTO(PentestingMethodology pentestingMethodology)
    //{
    //    public string Name => pentestingMethodology.Name;
    //}
}

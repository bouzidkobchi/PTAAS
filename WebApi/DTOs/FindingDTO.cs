using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Enums;
using WebApi.Models;

namespace WebApi.DTOs
{
    public class FindingDTO
    {
        public required string Id { get; set; }
        public string? Description { get; set; }
        public Severity Severity { get; set; }
        public required string FounderId { get; set; }
        public required string TestId { get; set; }
        public FindingDTO() { }
        public FindingDTO(Finding finding)
        {
            Id = Guid.NewGuid().ToString();
            Description = finding.Description;
            Severity = finding.Severity;
            FounderId = finding.FounderId;
            TestId = finding.TestId;
        }
        public Finding ToFinding()
        {
            return new Finding { Id = Id, Description = Description, Severity = Severity, FounderId = FounderId, TestId = TestId };
        }
    }
}

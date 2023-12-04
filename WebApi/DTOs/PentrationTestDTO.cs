﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Enums;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.DTOs
{
    public class PentrationTestDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string SystemId { get; set; }
        public ICollection<PentestingMethodology> Methodologies { get; set; } = new List<PentestingMethodology>();
        private DateTime CreationTime { get; set; } = DateTime.Now;
        public required string OwnerId { get; set; }
        public ICollection<Pentester> Pentesters { get; set; } = new List<Pentester>();
        // scope :
        // ip ranges , 
        // report / reports

        public PentrationTest ToPentrationTest()
        {
            return new PentrationTest
            {
                Id = Id,
                SystemId = SystemId,
                Methodologies = Methodologies,
                CreationTime = CreationTime,
                OwnerId = OwnerId,
                Pentesters = Pentesters
            };
        }
    }

}
using Application.Models;
using Application.Models.Dtos;
using Application.Models.ViewModels;
using AutoMapper;
using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MappingProfiles
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<CustomerViewModel, Customer>().ReverseMap();
            CreateMap<CustomerDto, Customer>().ReverseMap();
        }
    }
}

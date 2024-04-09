using Data.Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using AutoMapper;
using Business;

namespace TradeMarket.Tests
{
    internal static class UnitTestBusinessHelper
    {
        public static IMapper CreateMapperProfile()
        {
            var myProfile = new AutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }
    }
}

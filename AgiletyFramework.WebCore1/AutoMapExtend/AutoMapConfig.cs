using AgiletyFramework.DbModels.Models;
using AgiletyFramework.IBusinessServices;
using AgiletyFramework.ModelDto;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiletyFramework.WebCore1.AutoMapExtend
{
    public class AutoMapConfig:Profile
    {
        public AutoMapConfig()
        {
            CreateMap<UserEntity, UserDto>()
                .ForMember(c=>c.UserId,s=>s.MapFrom(x=>x.UserId))//id映射到id
                .ForMember(c => c.Name, s => s.MapFrom(x => x.Name))//Name映射到Name
                .ReverseMap();//可以相互转换

            CreateMap<PagingData<UserEntity>, PagingData<UserDto>>();

            CreateMap<AddUserDto, UserEntity>()
                .ForMember(c=>c.UserId,s=>s.MapFrom(x=>x.UserId))
                .ForMember(c=>c.Name,s=>s.MapFrom(x=>x.Name));

            CreateMap<SystemLog, SystemLogDto>();
            CreateMap<PagingData<SystemLog>, PagingData<SystemLogDto>>();

            CreateMap<MenuEntity, MenuTreeDto>().ReverseMap();
            //CreateMap<List<MenuEntity>, List<MenuTreeDto>>().ReverseMap();
        }
    }
}

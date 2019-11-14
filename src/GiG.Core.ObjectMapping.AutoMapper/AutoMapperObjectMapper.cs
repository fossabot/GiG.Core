using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using IObjectMapper = GiG.Core.ObjectMapping.Abstractions.IObjectMapper;

[assembly: InternalsVisibleTo("GiG.Core.ObjectMapping.AutoMapper.Tests.Unit")]
namespace GiG.Core.ObjectMapping.AutoMapper
{
    internal class AutoMapperObjectMapper : IObjectMapper
    {
        private readonly IMapper _mapper;

        public AutoMapperObjectMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return _mapper.Map(source, sourceType, destinationType);
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            return _mapper.Map(source, destination, sourceType, destinationType);
        }

        public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, object parameters = null, params Expression<Func<TDestination, object>>[] membersToExpand)
        {
            return _mapper.ProjectTo(source, parameters, membersToExpand);
        }

        public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, IDictionary<string, object> parameters, params string[] membersToExpand)
        {
            return _mapper.ProjectTo<TDestination>(source, parameters, membersToExpand);
        }
    }
}

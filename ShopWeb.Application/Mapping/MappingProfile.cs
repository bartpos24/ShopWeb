using AutoMapper;
using ShopWeb.Application.TransferObjects.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShopWeb.Application.Mapping
{
    public class MappingProfile : Profile
    {
		//     public MappingProfile()
		//     {
		//// Configure AutoMapper to be more lenient
		//this.AllowNullCollections = true;
		//this.AllowNullDestinationValues = true;
		//ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
		//     }
		public MappingProfile()
		{
			// Manual mapping for Inventory
			
			// Apply other mappings from assembly
			ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
		}
		private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();
            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}

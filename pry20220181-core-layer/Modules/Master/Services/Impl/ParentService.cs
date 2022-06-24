using pry20220181_core_layer.Modules.Master.DTOs.Input;
using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Master.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.Services.Impl
{
    public class ParentService : IParentService
    {
        IParentRepository _parentRepository;

        public ParentService(IParentRepository parentRepository)
        {
            _parentRepository = parentRepository;
        }

        public async Task<int> RegisterParentAndChildrenAsync(ParentCreateDTO parentCreateDTO)
        {
            Parent parent = new Parent()
            {
                DNI = parentCreateDTO.DNI,
                Telephone = parentCreateDTO.Telephone,
                UbigeoId = parentCreateDTO.UbigeoId,
                UserId = parentCreateDTO.UserId,
                ChildParents = new List<ChildParent>()
            };

            foreach (var childItem in parentCreateDTO.Children)
            {
                Child child = new Child()
                {
                    DNI = childItem.DNI,
                    Firstname = childItem.FirstName,
                    Lastname = childItem.LastName,
                    Gender = childItem.Gender,
                    Birthdate = childItem.Birthdate
                };
                parent.ChildParents.Add(new ChildParent()
                {
                    Relationship = parentCreateDTO.Relationship == "Father" ? 'F' : 'M',
                    Parent = parent,
                    ParentId = parent.ParentId,
                    Child = child,
                    ChildId = child.ChildId
                });
            }

            return await _parentRepository.CreateWithChildrenAsync(parent);
        }
    }
}

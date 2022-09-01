using Microsoft.Extensions.Logging;
using pry20220181_core_layer.Modules.Master.DTOs.Input;
using pry20220181_core_layer.Modules.Master.DTOs.Output;
using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Master.Repositories;
using pry20220181_core_layer.Utils;
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
        ILogger<ParentService> _logger;

        public ParentService(IParentRepository parentRepository, ILogger<ParentService> logger)
        {
            _parentRepository = parentRepository;
            _logger = logger;
        }

        public async Task<List<ChildDTO>> GetChildrenAsync(int parentId)
        {
            var children = await _parentRepository.GetChildrenAsync(parentId);

            var childrenToReturn = new List<ChildDTO>();
            foreach (var child in children)
            {
                childrenToReturn.Add(new ChildDTO()
                {
                    ChildId = child.ChildId,
                    DNI = child.DNI,
                    Firstname = child.Firstname,
                    Lastname = child.Lastname,
                    Birthdate = child.Birthdate,
                    Gender = child.Gender,
                    Age = GetAgeFromBirthdate.GetAge(child.Birthdate)
                });
            }
            return childrenToReturn;
        }

        public async Task<ParentDTO> GetParentAsync(int parentId)
        {
            var parentFromDb = await _parentRepository.GetByIdAsync(parentId);
            return new ParentDTO()
            {
                DNI = parentFromDb.DNI,
                Email = parentFromDb.User.Email,
                FirstName = parentFromDb.User.FirstName,
                LastName = parentFromDb.User.LastName,
                Telephone = parentFromDb.Telephone,
                UbigeoId = parentFromDb.UbigeoId
            };
        }

        public async Task<int> RegisterParentAndChildrenAsync(ParentCreateDTO parentCreateDTO)
        {
            if(parentCreateDTO.DNI is null)
            {
                return 0;
            }

            Parent parent = new Parent()
            {
                DNI = parentCreateDTO.DNI,
                Telephone = parentCreateDTO.Telephone,
                UbigeoId = parentCreateDTO.UbigeoId,
                UserId = parentCreateDTO.UserId,
                ChildParents = new List<ChildParent>()
            };

            var relationship = parentCreateDTO.Relationship;

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
                    Relationship = (relationship == "Father" ? Relationship.Father : Relationship.Mother),
                    Parent = parent,
                    //ParentId = parent.ParentId,
                    Child = child,
                });
            }

            var createdParentId = await _parentRepository.CreateWithChildrenAsync(parent);

            _logger.LogInformation($"The parent with ID {createdParentId} and DNI {parent.DNI} was created. Has {parent.ChildParents.Count()} children");

            return createdParentId;
        }

        public async Task<RegisteredParentDTO> RegisterParentAndChildrenForValidationAsync(ParentCreateDTO parentCreateDTO)
        {
            if(parentCreateDTO.DNI is null)
            {
                return null;
            }

            Parent parent = new Parent()
            {
                DNI = parentCreateDTO.DNI,
                Telephone = parentCreateDTO.Telephone,
                UbigeoId = parentCreateDTO.UbigeoId,
                UserId = parentCreateDTO.UserId,
                ChildParents = new List<ChildParent>()
            };

            var relationship = parentCreateDTO.Relationship;

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
                    Relationship = (relationship == "Father" ? Relationship.Father : Relationship.Mother),
                    Parent = parent,
                    //ParentId = parent.ParentId,
                    Child = child,
                });
            }

            var createdParentId = await _parentRepository.CreateWithChildrenAsync(parent);

            _logger.LogInformation($"The parent with ID {createdParentId} and DNI {parent.DNI} was created. Has {parent.ChildParents.Count()} children");

            return new RegisteredParentDTO(){
                Children = parent.ChildParents.Select(c=> new RegisteredChildDTO() { ChildId = c.ChildId } ).ToList(),
                ParentId = parent.ParentId
            };
        }
    }
}

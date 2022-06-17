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
    public class ChildService : IChildService
    {
        private readonly IChildRepository _childRepository;

        public ChildService(IChildRepository childRepository)
        {
            _childRepository = childRepository;
        }

        public async Task<ChildDTO> GetChildByDniAsync(string DNI)
        {
            var childFromDb = await _childRepository.GetByDniAsync(DNI);
            var childToReturn = new ChildDTO()
            {
                ChildId = childFromDb.ChildId,
                DNI = childFromDb.DNI,
                Firstname = childFromDb.Firstname,
                Lastname = childFromDb.Lastname,
                Birthdate = childFromDb.Birthdate,
                Gender = childFromDb.Gender,
                Age = GetAgeFromBirthdate.GetAge(childFromDb.Birthdate)
            };
            return childToReturn;
        }
    }
}

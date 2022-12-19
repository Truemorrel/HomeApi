using FluentValidation;
using HomeApi.Contracts.Models.Rooms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeApi.Contracts.Validation
{
    public class RoomUpdateRequestValidator : AbstractValidator<RoomUpdateRequest>
    {
        public RoomUpdateRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}

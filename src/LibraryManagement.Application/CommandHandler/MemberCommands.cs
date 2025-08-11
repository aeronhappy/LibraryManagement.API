using AutoMapper;
using FluentResults;
using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Errors;
using LibraryManagement.Application.Response;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.ValueObjects;
using System.Xml.Linq;

namespace LibraryManagement.Application.CommandHandler
{
    public class MemberCommands : IMemberCommands
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public MemberCommands(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }




        public async Task<Result<MemberResponse>> CreateMemberAsync(string name, string email, CancellationToken cancellationToken)
        {
            var newMember = new Member()
            {
                Id = new MemberId(Guid.NewGuid()),
                Name = name,
                Email = email,
                MaxBooksAllowed = 5,
                BorrowedBooksCount = 0,

            };

            var newMemberResponse = _mapper.Map<MemberResponse>(newMember);
            await _unitOfWork.Members.CreateMemberAsync(newMember);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok(newMemberResponse);

        }


        public async Task<Result> RemoveMemberAsync(Guid id, CancellationToken cancellationToken)
        {

            Member? member = await _unitOfWork.Members.GetMemberByIdAsync(new MemberId(id));

            if (member is null)
                return Result.Fail(new EntityNotFoundError($"No Member ={id} found"));

            if (member.BorrowedBooksCount != 0)
                return Result.Fail(new UnableToDeleteError($"Member ={id} has borrowed books"));


            await _unitOfWork.Members.RemoveMemberAsync(new MemberId(id));
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }

        public async Task<Result> UpdateMemberAsync(Guid id, string? name, string? email, CancellationToken cancellationToken)
        {

            Member? member = await _unitOfWork.Members.GetMemberByIdAsync(new MemberId(id));
            if (member is null)
                return Result.Fail(new EntityNotFoundError($"No Member ={id} found"));

            await _unitOfWork.Members.UpdateMemberAsync(new MemberId(id), name ?? member.Name, email ?? member.Email);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }

        public async Task<Result> UpdateProfilePictureAsync(Guid id, string profilePic, CancellationToken cancellationToken)
        {
            Member? member = await _unitOfWork.Members.GetMemberByIdAsync(new MemberId(id));
            if (member is null)
                return Result.Fail(new EntityNotFoundError($"No Member ={id} found"));

            member.SetProfilePicture(profilePic);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
    }
}

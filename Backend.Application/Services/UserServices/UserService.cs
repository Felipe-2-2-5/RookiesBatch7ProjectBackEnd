﻿using AutoMapper;
using Backend.Application.AuthProvide;
using Backend.Application.Common;
using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AuthDTOs;
using Backend.Application.IRepositories;
using Backend.Domain.Entities;
using Backend.Domain.Enum;
using Backend.Domain.Exceptions;
using DocumentFormat.OpenXml.Office2016.Excel;
using FluentValidation;

namespace Backend.Application.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IValidator<UserDTO> _validator;

        public UserService(IUserRepository userRepo, ITokenService tokenService, IMapper mapper, IValidator<UserDTO> validator)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _mapper = mapper;
            _validator = validator;
        }
        public async Task<UserResponse> GetByIdAsync(int id)
        {
            var entity = await _userRepo.GetByIdAsync(id) ?? throw new NotFoundException();
            var dto = _mapper.Map<UserResponse>(entity);
            return dto;

        }
        public async Task<UserResponse> InsertAsync(UserDTO dto, string createName)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                throw new DataInvalidException(string.Join(", ", errors));
            }

            var user = _mapper.Map<User>(dto);
            user = await _userRepo.GenerateUserInformation(user);

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.CreatedBy = createName;
            user.CreatedAt = DateTime.Now;
            await _userRepo.InsertAsync(user);

            user = await FindUserByUserNameAsync(user.UserName);
            var res = _mapper.Map<UserResponse>(user);
            return res;

        }
        public async Task<bool> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO)
        {
            var user = await _userRepo.GetByIdAsync(changePasswordDTO.Id) ?? throw new NotFoundException();
            bool checkPassword = BCrypt.Net.BCrypt.Verify(changePasswordDTO.OldPassword, user.Password);
            if (!checkPassword)
            {
                throw new DataInvalidException("Password is incorrect");
            }
            if (changePasswordDTO.NewPassword == changePasswordDTO.OldPassword)
            {
                throw new DataInvalidException("Do not re-enter the old password");
            }
            var check = new PasswordRegex();
            if (!check.IsPasswordValid(changePasswordDTO.NewPassword))
                throw new DataInvalidException("The password having at least 8 characters, 1 uppercase letter, 1 lowercase letter, 1 number, and 1 symbol");

            user.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDTO.NewPassword);
            user.ModifiedAt = DateTime.UtcNow;
            user.ModifiedBy = user.FirstName;
            user.FirstLogin = false;

            await _userRepo.UpdateAsync(user);

            return true;
        }
        public async Task<User?> FindUserByUserNameAsync(string email) => await _userRepo.FindUserByUserNameAsync(email);

        public async Task<LoginResponse> LoginAsync(LoginDTO dto)
        {
            var getUser = await FindUserByUserNameAsync(dto.UserName!);
            if (getUser == null)
                return new LoginResponse(false, "User not found");

            if (getUser.IsDeleted == true)
                throw new NotAllowedException("Your account has been disabled");

            var checkPassword = BCrypt.Net.BCrypt.Verify(dto.Password, getUser.Password);

            return checkPassword ? new LoginResponse(true, "Login success", _tokenService.GenerateJWTWithUser(getUser)) : new LoginResponse(false, "Invalid username or password. Please try again.");
        }

        public async Task<PaginationResponse<UserResponse>> GetFilterAsync(UserFilterRequest request, Location location)
        {
            var res = await _userRepo.GetFilterAsync(request, location);
            var dtos = _mapper.Map<IEnumerable<UserResponse>>(res.Data);
            return new(dtos, res.TotalCount);

        }
        public async Task<PaginationResponse<UserResponse>> GetFilterChoosingAsync(int id, UserFilterRequest request, Location location)
        {
            var res = await _userRepo.GetFilterChoosingAsync(id, request, location);
            var dtos = _mapper.Map<IEnumerable<UserResponse>>(res.Data);
            return new(dtos, res.TotalCount);
        }

        public async Task DisableUserAsync(int userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            if (user.IsDeleted == true)
            {
                throw new NotAllowedException("User has been disabled");
            }
            if (await _userRepo.HasActiveAssignmentsAsync(userId))
            {
                throw new NotAllowedException("There are valid assignments belonging to this user. \nPlease close all assignments before disabling user.");
            }

            user.IsDeleted = true;
            await _userRepo.UpdateAsync(user);
        }

        public async Task<UserResponse> UpdateAsync(int id, UserDTO dto, string modifiedBy, Location location)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            if (user.IsDeleted == true)
            {
                throw new NotAllowedException("User has been disabled");
            }
            if (user.Location != location)
            {
                throw new ForbiddenException("No permission to update this user");
            }
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                throw new DataInvalidException(string.Join(", ", errors));
            }

            user.DateOfBirth = (DateTime)dto.DateOfBirth!;
            user.JoinedDate = (DateTime)dto.JoinedDate!;
            user.Gender = dto.Gender;
            user.Type = dto.Type;
            user.Location = dto.Location;
            user.ModifiedAt = DateTime.UtcNow;
            user.ModifiedBy = modifiedBy;

            try
            {
                await _userRepo.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                throw new DataInvalidException(ex.Message);
            }

            var res = _mapper.Map<UserResponse>(user);
            return res;
        }

        
    }
}
